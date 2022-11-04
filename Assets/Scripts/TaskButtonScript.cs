using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TaskButtonScript : MonoBehaviour
{
    //private int sceneID;
    public UIManager manager;

    // private GameObject taskMenu;
    // Start is called before the first frame update
    void Start()
    {
        //       GameObject parent = GameObject.Find("/Canvas");
        //       taskMenu = FindChildInParent.FindObject(parent, "TaskMenu");
        //       taskMenu.SetActive(false);
        //taskMenu = GameObject.Find("Canvas/TaskMenu").GetComponent<Renderer>().enabled;
        //GameObject.Find("Canvas/TaskMenu").GetComponent<Renderer>().enabled = false;
        //       sceneID = SceneManager.GetActiveScene().buildIndex;
    }

    //methods for Main Menu buttons
    // public void SelectApartment()
    // {
    //     SceneManager.LoadScene("Apartment");
    //     sceneID = SceneManager.GetActiveScene().buildIndex;
    // }
    // public void SelectHouse()
    // {
    //     SceneManager.LoadScene("HouseGroundFloor");
    //     sceneID = SceneManager.GetActiveScene().buildIndex;
    // }
    //method to present task cards
    // public void SelectTaskCard(){
    //     GameObject parent = GameObject.Find("/Canvas");
    //     taskMenu = parent.FindObject("TaskMenu");
    //     taskMenu.SetActive(true);
    //     GameObject player = GameObject.Find("Player");
    //     if(player != null){
    //         PlayerScript playerScipt = (PlayerScript) player.GetComponent(typeof(PlayerScript));
    //         GameObject taskObject = playerScipt.getClosestTask();
    //         Task taskScipt = (Task) taskObject.GetComponent(typeof(Task));
    //         //Get task object
    //         MaintenanceTask task = taskScipt.maintenanceTask;
    //         TaskMenu menuScript = (TaskMenu) taskMenu.GetComponent(typeof(TaskMenu));
    //         menuScript.changeHeader(task.title);
    //     }
    // }
    //method to change from ground floor to basement and vice versa in house level
    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (sceneID == 2)
    //     {
    //         SceneManager.LoadScene(3);
    //     }
    //     else
    //     {
    //         SceneManager.LoadScene(2);
    //     }
    //     sceneID = SceneManager.GetActiveScene().buildIndex;
    // }
    // Update is called once per frame
    void Update()
    {
    }
}
