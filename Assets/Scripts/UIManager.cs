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
    public EditFurniture editFurniture
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

    private GameObject clickedObject;

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
        virtualCamera.m_Lens.OrthographicSize = 7;

        editRoom.enableAddButtons(rooms, this.editing);

        player.SetActive(!this.editing);
        canvas.SetActive(!this.editing);
    }

    //MESSAGE FROM FLUTTER: CALL EDIT FURNITURE
    public void addNewFurniture(string furnitureJson)
    {
        editFurniture.addEmptyFurniture (furnitureJson);
    }

    public void cancelFurnitureEditing(string message)
    {
        editFurniture.cancelEditing (message);
    }

    public void rotateFurniture(string message)
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
        if (editRoom.canBeEdited != false)
        {
            RoomData newRoom = editRoom.convertToRoomData();

            bool isEditing =
                !(string.IsNullOrWhiteSpace(editRoom.getCurrentRoom().id));

            RoomData updatedRoom = editRoom.updateSenderConnections(newRoom);

            if (updatedRoom != null)
            {
                //Find updated room to open door
                foreach (GameObject roomObject in rooms)
                {
                    RoomData currData =
                        roomObject.GetComponent<RoomScript>().roomData;
                    if (currData.id == updatedRoom.id)
                    {
                        roomObject.GetComponent<RoomScript>().roomData =
                            updatedRoom;
                    }
                }

                SaveManager.saveOrUpdateRoom(Manager, updatedRoom, true);
            }
            SaveManager.saveOrUpdateRoom(Manager, newRoom, isEditing == true);
            if (isEditing)
            {
                roomAdder.updateRoom (newRoom);
            }
            else
            {
                roomAdder.addRoomToScene (newRoom);
            }
            editRoom.reset();
        }
        else
        {
            Manager.SendMessageToFlutter("saveRoom: ");
        }
    }

    public void rotateRoom(string message)
    {
        editRoom.rotateCurrentRoom();
    }

    public void cancelRoomEditing(string message)
    {
        editRoom.reset();
    }

    public void didTapOnObject(GameObject tapped)
    {
        if (editing)
        {
            if (tapped.GetComponent<FurnitureScript>() != null)
            {
                editFurniture.didTapOnFurniture (tapped);
            }
            else if (tapped.GetComponent<RoomScript>() != null)
            {
                editRoom.didTapOnRoom (tapped);
            }
        }
    }

    void Update()
    {
        //Move around camera
        if (Input.touchCount > 0)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    RaycastHit2D clickedStart =
                        Physics2D
                            .Raycast(Camera
                                .main
                                .ScreenToWorldPoint(Input.GetTouch(0).position),
                            Vector2.zero);

                    clickedObject = clickedStart.collider.gameObject;
                    this.isDraggingCamera = false;
                    break;
                case TouchPhase.Moved:
                    clickedObject = null;

                    if (
                        editFurniture.getCurrentFurniture() == null &&
                        editRoom.currentRoom == null
                    )
                    {
                        this.isDraggingCamera = true;

                        Vector2 touchDeltaPosition =
                            Input.GetTouch(0).deltaPosition;

                        virtualCamera
                            .transform
                            .Translate(-touchDeltaPosition.x *
                            Time.deltaTime *
                            0.25f,
                            -touchDeltaPosition.y * Time.deltaTime * 0.25f,
                            0);
                    }

                    break;
                case TouchPhase.Ended:
                    RaycastHit2D clickedEnd =
                        Physics2D
                            .Raycast(Camera
                                .main
                                .ScreenToWorldPoint(Input.GetTouch(0).position),
                            Vector2.zero);

                    if (
                        ReferenceEquals(clickedEnd.collider.gameObject,
                        clickedObject) &&
                        clickedObject != null
                    )
                    {
                        didTapOnObject (clickedObject);
                    }
                    clickedObject = null;
                    this.isDraggingCamera = false;

                    break;
                default:
                    break;
            }
        }
    }
}
