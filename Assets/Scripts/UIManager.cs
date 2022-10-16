using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UIManager : MonoBehaviour
{

    public List<GameObject> furnitures = new List<GameObject>();
    public List<GameObject> rooms = new List<GameObject>();
    public UnityMessageManager Manager
        {
         get { return GetComponent<UnityMessageManager>(); }
        } 

    //Get Edit furniture script
    private EditFurniture editFurniture{
        get{ return GetComponent<EditFurniture>(); }
    }

    //Get Edit Room script
    public EditRoom editRoom{
        get{ return GetComponent<EditRoom>();}
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
      
    }
    //Change editing status
    public void changeEditingStatus(string isEditing){
        this.editing = isEditing == "true";

        editFurniture.reset();
        editRoom.reset();

        virtualCamera.m_Follow = editing ? null : player.transform;


        editRoom.enableAddButtons(rooms, this.editing);
        
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

    //Done button for furniture was clicked
    public void saveFurniture(string message){

        FurnitureData data = editFurniture.convertToFurnitureData();

        bool isEditing = !(string.IsNullOrWhiteSpace(editFurniture.getCurrentFurniture().id));
        SaveManager.saveOrUpdateFurniture(Manager, data, isEditing == true);

        if(isEditing){
            furnitureAdder.updateFurniture(data);
        }
        else{
            furnitureAdder.addFurniture(data);
        }
        

        editFurniture.cancelFurnitureEditing("");
    }

    //MESSAGE FROM FLUTTER: CALL EDIT ROOm

    //Done button for room was clicked
    public void saveRoom(string message){

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
