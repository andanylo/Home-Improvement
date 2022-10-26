using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D player;

    public GameObject showTaskBtn;

    public UIManager uimanager;

    bool isCloseToAnyOfTheTask = false;

    private float taskmindistancedetection = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        showTaskBtn.SetActive (isCloseToAnyOfTheTask);
    }

    //Check if player is close to any of the task
    public bool checkIfCloseToTask()
    {
        GameObject[] tasks = GameObject.FindGameObjectsWithTag("Task");

        foreach (GameObject task in tasks)
        {
            float distance =
                Vector2
                    .Distance(player.transform.position,
                    task.transform.position);
            if (distance <= taskmindistancedetection)
            {
                return true;
            }
        }
        return false;
    }

    //Get closest task to a player
    public GameObject getClosestTask()
    {
        GameObject[] tasks = GameObject.FindGameObjectsWithTag("Task");
        if (tasks.Length > 0)
        {
            float closestDistance =
                Vector2
                    .Distance(player.transform.position,
                    tasks[0].transform.position);
            GameObject closestTask = tasks[0];

            foreach (GameObject task in tasks)
            {
                float distance =
                    Vector2
                        .Distance(player.transform.position,
                        task.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTask = task;
                }
            }
            return closestTask;
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        bool oldIsClose = isCloseToAnyOfTheTask;
        isCloseToAnyOfTheTask = checkIfCloseToTask();

        if (oldIsClose != isCloseToAnyOfTheTask)
        {
            showTaskBtn.SetActive (isCloseToAnyOfTheTask);
        }
    }
}
