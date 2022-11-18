using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    private DateTime currentTime = DateTime.Now;

    //[SerializeField]
    //public Button displayButton;
    [SerializeField]
    public PlayerScript _player;

    private UIManager uimanager
    {
        get
        {
            return GetComponent<UIManager>();
        }
    }

    private List<PlayerTask> playerTasks = new List<PlayerTask>();

    private List<Task> availableTasks = new List<Task>();

    //Get player task from furniture
    public PlayerTask getPlayerTaskFromFurniture(FurnitureData furnitureData)
    {
        PlayerTask find =
            playerTasks.Find(t => t.furnitureID == furnitureData.furnitureID);
        return find;
    }

    // Start is called before the first frame update
    void Start()
    {
        getPlayerTasks();
        getTasks();

        _player.makeFurnitureInteractble.AddListener (
            enableDisplayButtonAndCreatePlayerTaskIfNeeded
        );
    }

    //Activate display task button if task is not checked and create a playerTask if not found
    public void enableDisplayButtonAndCreatePlayerTaskIfNeeded(
        string furnitureID,
        string furnitureName
    )
    {
        PlayerTask playerTask =
            playerTasks.Find(p => p.furnitureID == furnitureID);
        if (playerTask == null)
        {
            playerTask = createPlayerTask(furnitureID, furnitureName);
            SaveManager
                .saveOrUpdatePlayerTask(uimanager.Manager, playerTask, false);
        }
        if (playerTask.complete_Status == false)
        {
            uimanager
                .Manager
                .SendMessageToFlutter("taskButtonStatus:" + true.ToString());
        }
        else
        {
            uimanager
                .Manager
                .SendMessageToFlutter("taskButtonStatus:" + false.ToString());
            //displayButton.gameObject.SetActive(false);
        }
    }

    //Activate task button based on complete status
    public void enableDisplayButton(PlayerTask playerTask)
    {
        if (uimanager.currentClosestFurnitureData != null)
        {
            if (
                uimanager.currentClosestFurnitureData.furnitureID ==
                playerTask.furnitureID
            )
            {
                if (playerTask.complete_Status == false)
                {
                    uimanager
                        .Manager
                        .SendMessageToFlutter("taskButtonStatus:" +
                        true.ToString());
                    //Button.gameObject.SetActive(true);
                }
                else
                {
                    uimanager
                        .Manager
                        .SendMessageToFlutter("taskButtonStatus:" +
                        false.ToString());
                }
            }
        }
    }

    //Create playerTask and save to database
    public void createAndSavePlayerTask(
        string furnitureID,
        string furnitureName
    )
    {
        PlayerTask newPlayerTask = createPlayerTask(furnitureID, furnitureName);
        playerTasks.Add (newPlayerTask);
        SaveManager
            .saveOrUpdatePlayerTask(uimanager.Manager, newPlayerTask, false);
    }

    //Create player task from furniture
    public PlayerTask createPlayerTask(string furnitureID, string furnitureName)
    {
        PlayerTask playerTask = new PlayerTask();

        //Find task
        Task task =
            this.availableTasks.Find(t => t.furnitureName == furnitureName);

        DateTime result = getDelayedTime(task.CheckUpStatus);

        playerTask.task_title = task.task_title;
        playerTask.DelayTime = result.ToString();
        playerTask.award = task.award;
        playerTask.checkUpStatus = task.CheckUpStatus;
        playerTask.furnitureID = furnitureID;

        return playerTask;
    }

    private DateTime getDelayedTime(string checkUpStatus)
    {
        DateTime newData = currentTime;
        TimeSpan addedTime = new TimeSpan();

        if (checkUpStatus.Trim().ToLower() == "daily")
        {
            addedTime = new TimeSpan(1, 0, 0, 0); //first par: day, second par: hour, third par: minute, fourth par: second
        }
        else if (checkUpStatus.Trim().ToLower() == "weekly")
        {
            addedTime = new TimeSpan(7, 0, 0, 0); //first par: day, second par: hour, third par: minute, fourth par: second
        }
        else if (checkUpStatus.Trim().ToLower() == "bi-weekly")
        {
            addedTime = new TimeSpan(14, 0, 0, 0); //first par: day, second par: hour, third par: minute, fourth par: second
        }
        else if (checkUpStatus.Trim().ToLower() == "monthly")
        {
            addedTime = new TimeSpan(31, 0, 0, 0); //first par: day, second par: hour, third par: minute, fourth par: second
        }
        return newData.Add(addedTime);
    }

    //Complete player task
    public void completePlayerTask(PlayerTask playerTask)
    {
        DateTime newDelayedTime = getDelayedTime(playerTask.checkUpStatus);
        playerTask.DelayTime = newDelayedTime.ToString();
        playerTask.complete_Status = true;
        if (playerTask.didChangeCompleteStatus != null)
        {
            //Notify listeners
            playerTask
                .didChangeCompleteStatus
                .Invoke(playerTask.complete_Status);
        }
        uimanager
            .Manager
            .SendMessageToFlutter("didCompleteTaskWithAward:" +
            playerTask.award.ToString());
        SaveManager.saveOrUpdatePlayerTask(uimanager.Manager, playerTask, true);
    }

    //Delete player task
    public void deletePlayerTask(string furnitureID)
    {
        PlayerTask playerTask =
            playerTasks.Find(p => p.furnitureID == furnitureID);
        if (playerTask != null)
        {
            playerTasks.Remove (playerTask);
        }
    }

    //Send message to flutter to get player tasks
    private void getPlayerTasks()
    {
        uimanager.Manager.SendMessageToFlutter("getPlayerTasks:");
    }

    //Did fetch player tasks from firebase
    public void didGetPlayerTasks(string playerTaskJSON)
    {
        if (!string.IsNullOrWhiteSpace(playerTaskJSON))
        {
            this.playerTasks = fetchPlayerTasks(playerTaskJSON);
        }
    }

    //Send message to flutter to get tasks
    private void getTasks()
    {
        uimanager.Manager.SendMessageToFlutter("getTasks:");
    }

    //Did fetch tasks from firebase
    public void didGetTasks(string taskJSON)
    {
        if (!string.IsNullOrWhiteSpace(taskJSON))
        {
            this.availableTasks = fetchTasks(taskJSON);
        }
    }

    //Get tasks from database
    public List<Task> fetchTasks(string tasksJSON)
    {
        List<Task> tasks = new List<Task>();

        Dictionary<string, JObject> json =
            JsonConvert
                .DeserializeObject<Dictionary<string, JObject>>(tasksJSON);

        foreach (KeyValuePair<string, JObject> entry in json)
        {
            string furnitureName = entry.Key;

            Task task =
                Task
                    .convertFromJSON(entry
                        .Value
                        .ToString(Newtonsoft.Json.Formatting.None));
            task.furnitureName = furnitureName;

            tasks.Add (task);
        }
        return tasks;
    }

    //Get Player tasks from database
    public List<PlayerTask> fetchPlayerTasks(string playerTaskJSON)
    {
        List<PlayerTask> playerTasks = new List<PlayerTask>();

        Dictionary<string, JObject> json =
            JsonConvert
                .DeserializeObject<Dictionary<string, JObject>>(playerTaskJSON);

        foreach (KeyValuePair<string, JObject> entry in json)
        {
            string furnitureID = entry.Key;

            PlayerTask playerTask =
                PlayerTask
                    .convertFromJSON(entry
                        .Value
                        .ToString(Newtonsoft.Json.Formatting.None));
            playerTask.furnitureID = furnitureID;

            playerTasks.Add (playerTask);
        }
        return playerTasks;
    }

    //Update player tasks if needed
    public void updateTasksIfNeeded(List<PlayerTask> playerTasks)
    {
        foreach (PlayerTask playerTask in playerTasks)
        {
            DateTime date =
                DateTime
                    .Parse(playerTask.DelayTime,
                    System.Globalization.CultureInfo.InvariantCulture);
            if (currentTime >= date)
            {
                bool previousCompleteStatus = playerTask.complete_Status;
                playerTask.complete_Status = false;

                if (previousCompleteStatus != playerTask.complete_Status)
                {
                    if (playerTask.didChangeCompleteStatus != null)
                    {
                        //Notify listeners
                        playerTask
                            .didChangeCompleteStatus
                            .Invoke(playerTask.complete_Status);
                    }

                    enableDisplayButton (playerTask);

                    SaveManager
                        .saveOrUpdatePlayerTask(uimanager.Manager,
                        playerTask,
                        true);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateTasksIfNeeded(this.playerTasks);

        currentTime = DateTime.Now;
    }
}

[Serializable]
public class Task
{
    public string task_title = "";

    public string furnitureName;

    public string CheckUpStatus;

    public int award;

    public Task()
    {
        this.furnitureName = "";
        this.CheckUpStatus = "";
        this.award = 0;
    }

    public static Task convertFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Task>(jsonString);
    }
}

[Serializable]
public class PlayerTask
{
    public string furnitureID;

    public string task_title;

    public string DelayTime;

    public int award;

    public string checkUpStatus;

    public bool complete_Status;

    public DidChangeCompleteStatus didChangeCompleteStatus;

    public PlayerTask()
    {
        this.DelayTime = "";
        this.award = 0;
        this.task_title = "";

        this.checkUpStatus = "Daily";
        this.complete_Status = false;
    }

    public static PlayerTask convertFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlayerTask>(jsonString);
    }
}

public class DidChangeCompleteStatus : UnityEvent<bool> { }
