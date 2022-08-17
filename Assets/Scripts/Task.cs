using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public MaintenanceTask maintenanceTask;

    // Start is called before the first frame update
    void Start()
    {
        maintenanceTask = new MaintenanceTask();
        maintenanceTask.title = "Testing task";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
