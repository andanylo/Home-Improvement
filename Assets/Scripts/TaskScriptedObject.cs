using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TaskScriptedObject", order = 1)]
public class TaskScriptedObject : ScriptableObject
{
    public MaintenanceTask currentTask;
}
