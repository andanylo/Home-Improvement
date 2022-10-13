using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UIManager : MonoBehaviour
{

    public List<GameObject> furnitures = new List<GameObject>();
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

    public CinemachineVirtualCamera virtualCamera;

    private float cameraSpeed = 3F;
    public bool isDraggingCamera = false;


    public bool editing = false;
    public FurnitureAdder furnitureAdder;

    public GameObject player;
    public GameObject canvas;

    void Start()
    {
        saveManager.setEditFurnitureObject(editFurniture);
    }
    //Change editing status
    public void changeEditingStatus(string isEditing){
        this.editing = isEditing == "true";
        editFurniture.cancelFurnitureEditing("");
        virtualCamera.m_Follow = editing ? null : player.transform;
        player.SetActive(!this.editing);
        canvas.SetActive(!this.editing);
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

        bool isEditing = !(string.IsNullOrWhiteSpace(editFurniture.getCurrentFurniture().id));
        saveManager.saveOrUpdateFurniture(Manager, data, isEditing == true);

        if(isEditing){
            furnitureAdder.updateFurniture(data);
        }
        else{
            furnitureAdder.addFurniture(data);
        }
        

        editFurniture.cancelFurnitureEditing("");
    }
    void Update(){

        //Move around camera
        if (Input.touchCount > 0 && virtualCamera.m_Follow == null) {
            if(Input.GetTouch(0).phase == TouchPhase.Moved){
                this.isDraggingCamera = true;

                 Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            
                virtualCamera.transform.Translate(-touchDeltaPosition.x * Time.deltaTime * 0.1f, -touchDeltaPosition.y * Time.deltaTime * 0.1f, 0);
            }
            else{
                this.isDraggingCamera = false;
            }
            
        }
        
    }
}
