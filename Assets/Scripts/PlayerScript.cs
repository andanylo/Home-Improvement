using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public Rigidbody2D player;
    public GameObject showTaskBtn;

    private GameObject taskMenu;

    bool isCloseToAnyOfTheTask = false;
    // Start is called before the first frame update
    void Start()
    {
        
        showTaskBtn.SetActive(isCloseToAnyOfTheTask);
        
    }

    //Check if player is close to any of the task

    public bool checkIfCloseToTask(float mindistance){
        GameObject[] tasks = GameObject.FindGameObjectsWithTag("Task");

        foreach(GameObject task in tasks){
            float distance = Vector2.Distance(player.transform.position, task.transform.position);
            if(distance <= mindistance){
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        bool oldIsClose = isCloseToAnyOfTheTask;
        isCloseToAnyOfTheTask = checkIfCloseToTask(1.5f);
        
        
        if(oldIsClose != isCloseToAnyOfTheTask){
           
            showTaskBtn.SetActive(isCloseToAnyOfTheTask);
        }
    }
}
