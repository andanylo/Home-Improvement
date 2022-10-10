using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
     private UnityMessageManager Manager
        {
         get { return GetComponent<UnityMessageManager>(); }
        } 

    //Get Edit furniture script
    private EditFurniture editFurniture{
        get{ return GetComponent<EditFurniture>(); }
    }

    //Get save manager
    private SaveManager saveManager{
        get{return GetComponent<SaveManager>();}
    }

    public FurnitureAdder furnitureAdder;

    public GameObject player;
    public GameObject canvas;

    void Start()
    {
        saveManager.setEditFurnitureObject(editFurniture);
    }
    //Change editing status
    public void changeEditingStatus(string isEditing){
        bool editing = isEditing == "true";
        editFurniture.cancelFurnitureEditing("");
        player.SetActive(!editing);
        canvas.SetActive(!editing);
    }


    //MESSAGE FROM FLUTTER: CALL EDIT FURNITURE
    public void addNewFurniture(string furnitureJson){
        editFurniture.addNewFurniture(furnitureJson);
    }
    public void cancelFurnitureEditing(string message){
        editFurniture.cancelFurnitureEditing(message);
    }
    public void rotateBy90Degrees(string message){
        editFurniture.rotateBy90Degrees(message);
    }

    //Done button was clicked
    public void saveFurniture(string message){


        FurnitureData data = editFurniture.convertToFurnitureData();
        saveManager.saveFurniture(Manager, data);
        furnitureAdder.addFurniture(data);
    }

}
