using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<GameObject> furnitures = new List<GameObject>();

    public List<GameObject> rooms = new List<GameObject>();

    public UnityMessageManager Manager
    {
        get
        {
            return GetComponent<UnityMessageManager>();
        }
    }

    //Get Edit furniture script
    private EditFurniture editFurniture
    {
        get
        {
            return GetComponent<EditFurniture>();
        }
    }

    //Get Edit Room script
    public EditRoom editRoom
    {
        get
        {
            return GetComponent<EditRoom>();
        }
    }

    public CinemachineVirtualCamera virtualCamera;

    private float cameraSpeed = 3F;

    public bool isDraggingCamera = false;

    public bool editing = false;

    public FurnitureAdder furnitureAdder;

    public RoomAdder roomAdder;

    public GameObject player;

    public GameObject canvas;

    void Start()
    {
    }

    //Change editing status
    public void changeEditingStatus(string isEditing)
    {
        this.editing = isEditing == "true";

        editFurniture.reset();
        editRoom.reset();

        virtualCamera.m_Follow = editing ? null : player.transform;
        virtualCamera.m_Lens.OrthographicSize = 5;

        editRoom.enableAddButtons(rooms, this.editing);

        player.SetActive(!this.editing);
        canvas.SetActive(!this.editing);
    }

    //MESSAGE FROM FLUTTER: CALL EDIT FURNITURE
    public void addNewFurniture(string furnitureJson)
    {
        editFurniture.addNewFurniture (furnitureJson);
    }

    public void cancelFurnitureEditing(string message)
    {
        editFurniture.cancelFurnitureEditing (message);
    }

    public void rotateBy90Degrees(string message)
    {
        editFurniture.rotateBy90Degrees (message);
    }

    //Done button for furniture was clicked
    public void saveFurniture(string message)
    {
        FurnitureData data = editFurniture.convertToFurnitureData();

        bool isEditing =
            !(
            string.IsNullOrWhiteSpace(editFurniture.getCurrentFurniture().id)
            );
        SaveManager.saveOrUpdateFurniture(Manager, data, isEditing == true);

        if (isEditing)
        {
            furnitureAdder.updateFurniture (data);
        }
        else
        {
            furnitureAdder.addFurniture (data);
        }

        editFurniture.reset();
    }

    //MESSAGE FROM FLUTTER: CALL EDIT ROOm
    public void addRoom(string furnitureJson)
    {
        editRoom.addNewRoom (furnitureJson);
    }

    //Done button for room was clicked
    public void saveRoom(string message)
    {
        RoomData newRoom = editRoom.convertToRoomData();

        RoomData updatedRoom = editRoom.updateSenderConnections(newRoom);

        //Find updated room to open door
        foreach (GameObject roomObject in rooms)
        {
            RoomData currData = roomObject.GetComponent<RoomScript>().roomData;
            if (currData.id == updatedRoom.id)
            {
                roomObject.GetComponent<RoomScript>().roomData = updatedRoom;
            }
        }
        bool isEditing =
            !(string.IsNullOrWhiteSpace(editRoom.getCurrentRoom().id));

        SaveManager.saveOrUpdateRoom(Manager, updatedRoom, true);
        SaveManager.saveOrUpdateRoom(Manager, newRoom, isEditing == true);
        if (isEditing)
        {
        }
        else
        {
            roomAdder.addRoom (newRoom);
        }
        editRoom.reset();
    }

    public void cancelRoomEditing(string message)
    {
        editRoom.reset();
    }

    void Update()
    {
        //Move around camera
        if (Input.touchCount > 0 && virtualCamera.m_Follow == null)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                this.isDraggingCamera = true;

                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                virtualCamera
                    .transform
                    .Translate(-touchDeltaPosition.x * Time.deltaTime * 0.1f,
                    -touchDeltaPosition.y * Time.deltaTime * 0.1f,
                    0);
            }
            else
            {
                this.isDraggingCamera = false;
            }
        }
    }
}
