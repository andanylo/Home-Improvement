using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System;

public class EditRoom : MonoBehaviour
{
    //Communicator with flutter
    private UnityMessageManager Manager
    {
        get
        {
            return GetComponent<UnityMessageManager>();
        }
    }

    private UIManager uimanager
    {
        get
        {
            return GetComponent<UIManager>();
        }
    }

    public Room getCurrentRoom()
    {
        return currentRoom;
    }

    //Did set room template
    private Room _currentRoom;
    public Room currentRoom{
        get => _currentRoom;
        set{
            this._currentRoom = value;

            bool intersects = canPlaceRoom();

            if(currentRoom == null){
                room.GetComponent<SpriteRenderer>().color = new Color(255,255,255);
            }
            else{
                uimanager.virtualCamera.m_Follow = room.transform;
                room.GetComponent<SpriteRenderer>().color = intersects ? new Color(215, 0, 0, 115) : new Color(0, 215, 0, 115);

            }
            
        }
    }

    private float currentRoomRotation = 0f;

    public GameObject room;

    private GameObject senderRoom;

    private RoomData senderData;

    private Direction? senderDirection;

    private Direction? currentDirection;

    private GameObject editingRoom;

    public bool? canBeEdited = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    //Convert current room  into room data
    public RoomData convertToRoomData()
    {
        if (currentRoom != null)
        {
            RoomData newRoomData = new RoomData();
            newRoomData.key_word = currentRoom.key_word;
            if (string.IsNullOrWhiteSpace(currentRoom.id))
            {
                newRoomData.id = System.Guid.NewGuid().ToString();
            }
            else
            {
                newRoomData.id = currentRoom.id;
            }

            newRoomData.pos = room.transform.position;
            newRoomData.rot = new Vector3(0f, 0f, this.room.transform.eulerAngles.z);
            if(senderData != null){
                string idConnectedWith = senderData.id;
            
                Connection connection =
                    new Connection(idConnectedWith,
                    this.currentDirection ?? Direction.right);

                newRoomData.connections.Add (connection);
            }
            
            return newRoomData;
        }
        return null;
    }

    //Update sender room data connection on new room
    public RoomData updateSenderConnections(RoomData newRoomData)
    {
        if(editingRoom == null && senderData != null){
            Connection connection =
            new Connection(newRoomData.id, senderDirection ?? Direction.right);
            senderData.connections.Add (connection);
            return senderData;
        }
        return null;
        
    }

    //Enable add buttons
    public void enableAddButtons(List<GameObject> rooms, bool isEditing)
    {
        foreach (GameObject room in rooms)
        {
            room.GetComponent<RoomScript>().setEditRoomManager(this);
            room.GetComponent<RoomScript>().changeEditingStatus(isEditing);
        }
    }

    //Get new room center
    public Vector2
    getRoomCenter(
        Direction senderConnectionDirection,
        Vector2 roomSize,
        GameObject? senderObject
    )
    {
        if (senderConnectionDirection != null && room != null)
        {
            float edgeX = 0.0f;
            float edgeY = 0.0f;

            
            Vector2 senderSize =
                senderObject.GetComponent<BoxCollider2D>().bounds.size;

            if(senderObject != null){
                switch (senderConnectionDirection)
                {
                case Direction.right:
                    edgeX =
                        senderObject.transform.position.x + (senderSize.x / 2) + 0.015f;
                    edgeY = senderObject.transform.position.y;
                    return new Vector2(edgeX + roomSize.x / 2, edgeY);
                case Direction.left:
                    edgeX =
                        senderObject.transform.position.x - (senderSize.x / 2) - 0.015f;
                    edgeY = senderObject.transform.position.y;
                    return new Vector2(edgeX - (roomSize.x / 2), edgeY);
                case Direction.top:
                    edgeX = senderObject.transform.position.x;
                    edgeY =
                        senderObject.transform.position.y + (senderSize.y / 2) + 0.015f;
                    return new Vector2(edgeX, edgeY + roomSize.y / 2);
                case Direction.bottom:
                    edgeX = senderObject.transform.position.x;
                    edgeY =
                        senderObject.transform.position.y - (senderSize.y / 2) - 0.015f;
                    return new Vector2(edgeX, edgeY - roomSize.y / 2);
                }
            }
            
        }
        return Vector2.zero;
    }

    //Add an empty room
    public void addNewRoom(string jsonRoom)
    {
        JObject jobject = JObject.Parse(jsonRoom);
        Room editRoom = new Room("", jobject["key_word"].ToString());

        uimanager.virtualCamera.m_Lens.OrthographicSize = 12;

        foreach (GameObject otherRooms in uimanager.rooms)
        {
            otherRooms.GetComponent<RoomScript>().changeAddButtonVisibility(false);
        }
        setTemplateActive (editRoom);
    }

    //Set empty room template active
    public void setTemplateActive(Room template)
    {
        GameObject prefab =
            Resources.Load<GameObject>("Prefabs/Rooms/" + template.key_word);

        Sprite texture =
            Resources.Load<Sprite>("Textures/Rooms/" + template.key_word);
        room.GetComponent<SpriteRenderer>().sprite = texture;

        room.transform.localScale = prefab.transform.localScale;

        room.SetActive(true);

        room.transform.position =
            getRoomCenter(this.senderDirection ?? Direction.right,
            room.GetComponent<Renderer>().bounds.size,
            this.senderRoom);

        this.currentRoom = template;
    }

    public void setTemplateActive(RoomData data, GameObject roomToBeEdited){
        Room template = new Room(data.id, data.key_word);

        //Set texture if not null
        Sprite roomObjectTexture =
            Resources.Load<Sprite>("Textures/Rooms/" + template.key_word);;
        if (roomToBeEdited != null)
        {
            room.GetComponent<SpriteRenderer>().sprite =
                roomObjectTexture;
        }

        uimanager.virtualCamera.m_Lens.OrthographicSize = 12;

        room.transform.position =
            roomToBeEdited.transform.position;
        room.transform.rotation =
            roomToBeEdited.transform.rotation;
        room.transform.localScale =
            roomToBeEdited.transform.localScale;

        room.SetActive(true);

        foreach (GameObject otherRooms in uimanager.rooms)
        {
            otherRooms.GetComponent<RoomScript>().changeAddButtonVisibility(false);
        }

        this.currentRoom = template;

        this.currentRoomRotation = data.rot.z;
    }

    //Check if intersects with other rooms
    public bool intersectsWithOthers(List<GameObject> rooms){
        Bounds currBounds = new Bounds(new Vector3(room.transform.position.x, room.transform.position.y, 1.0f), room.GetComponent<Renderer>().bounds.size); //room.GetComponent<Renderer>().bounds;

        foreach(GameObject otherRooms in rooms){
            Bounds roomBounds = otherRooms.GetComponent<BoxCollider2D>().bounds;
            if (!ReferenceEquals(otherRooms, this.editingRoom))
            {
                if(roomBounds != null && currBounds != null){
      

                    if(currBounds.Intersects(roomBounds)){
                        return true;
                    }
                }
                else{
                   return true;
                }
            }
        }
        return false;
    }

    //Check if it is allowed to add and send messaage to flutter
    public bool canPlaceRoom(){
        bool intersects = intersectsWithOthers(uimanager.rooms);
        uimanager.Manager.SendMessageToFlutter("intersectsWithOtherRooms:" + intersects.ToString());

        return intersects;
    }

    //Rotate room
    public void rotateCurrentRoom(){
        this.currentRoomRotation += 90f;

        room.transform.eulerAngles = new Vector3(0f, 0f, this.currentRoomRotation);

        //Get new center
        room.transform.position = getRoomCenter(this.senderDirection ?? Direction.right, room.GetComponent<Renderer>().bounds.size,
            this.senderRoom);

        bool canPlace = !canPlaceRoom();

        room.GetComponent<SpriteRenderer>().color = canPlace ? new Color(0, 215, 0, 115) : new Color(215, 0, 0, 115);
    }



    public void didClickOnAddButton(
        GameObject senderRoom,
        RoomData senderData,
        Direction directionToAdd
    )
    {
        this.senderData = senderData;
        this.senderDirection = directionToAdd;
        this.senderRoom = senderRoom;
        this.currentDirection =
            DirectionManager.Instance.findOppositeDirection(directionToAdd);

        Manager.SendMessageToFlutter("presentRoomPopUp:true");
    }

    private void enableRoomEditing(RoomData roomToEdit)
    {
        bool hasConnections = roomToEdit.connections.Count > 1;
        this.canBeEdited = !hasConnections;
        Manager
            .SendMessageToFlutter("enableRoomEditing:{\"id\": \"" +
            roomToEdit.id +
            "\", " + "\"hasConnections\": \"" + hasConnections.ToString() + "\", " + "\"key_word\": \"" +
            roomToEdit.key_word +
            "\"}");
    }

    public void didTapOnRoom(GameObject clickedRoom)
    {
        if(currentRoom == null && uimanager.editing == true && uimanager.editFurniture.getCurrentFurniture() == null){
            RoomData clickedData = clickedRoom.GetComponent<RoomScript>().roomData;

            if (clickedData != null && clickedRoom != null)
            {
                this.editingRoom = clickedRoom;
                this.editingRoom.SetActive(false);

                
                if( clickedData.connections.Count == 1){
                    
                    this.senderDirection = DirectionManager.Instance.findOppositeDirection(clickedData.connections[0].direction);
                    this.currentDirection = clickedData.connections[0].direction;//;
                    GameObject senderRoom = uimanager.rooms.Find(r => r.GetComponent<RoomScript>().roomData.id == clickedData.connections[0].id);
                    this.senderData = senderRoom.GetComponent<RoomScript>().roomData;
                    this.senderRoom = senderRoom;
                }
                
                enableRoomEditing(clickedData);
                setTemplateActive(clickedData, clickedRoom);
            }

        }
    }

    //Delete furniture
    public void deleteRoom(string roomID)
    {
        GameObject roomToDelete =
            uimanager
                .rooms
                .Find(r =>
                    r
                        .GetComponent<RoomScript>()
                        .roomData
                        .id ==
                    roomID);
        roomToDelete.SetActive(true);
        if (roomToDelete != null)
        {
            //Find each connected room and remove connections
            foreach(GameObject room in uimanager.rooms){
                var connection = room.GetComponent<RoomScript>().roomData.connections.Find(c => c.id == roomID);
                if(connection != null){
                    RoomData roomData = room.GetComponent<RoomScript>().roomData;
                    roomData.connections.Remove(connection);

                    room.GetComponent<RoomScript>().roomData = roomData;
                    SaveManager.saveOrUpdateRoom(uimanager.Manager, roomData, true);
                }
            }
            
            Bounds roomBounds = roomToDelete.GetComponent<BoxCollider2D>().bounds;

            //Remove every furniture inside the room
            foreach(GameObject furniture in uimanager.furnitures.ToArray()){
                Bounds furnitureBounds = furniture.GetComponent<BoxCollider2D>().bounds;
                if(roomBounds.Contains(furnitureBounds.max) && roomBounds.Contains(furnitureBounds.min)){
                    uimanager.Manager.SendMessageToFlutter("deleteFurniture:{\"id\":\"" + furniture.GetComponent<FurnitureScript>().furnitureData.furnitureID + "\", \"name\":\"" + furniture.GetComponent<FurnitureScript>().furnitureData.name + "\"}");
                    uimanager.editFurniture.deleteFurniture(furniture.GetComponent<FurnitureScript>().furnitureData.furnitureID);
                }
            }


            uimanager.rooms.Remove (roomToDelete);
            Destroy (roomToDelete);
        }
    }

    public void reset()
    {
        this.currentRoom = null;
        this.senderData = null;
        this.senderDirection = null;
        this.currentDirection = null;
        this.senderRoom = null;
        this.canBeEdited = null;
        if(this.editingRoom != null){
            this.editingRoom.SetActive(true);
        }
        if(uimanager.editing){
            uimanager.virtualCamera.m_Follow = null;
        }

        this.editingRoom = null;

        this.currentRoomRotation = 0f;
        this.room.transform.eulerAngles = new Vector3(0f, 0f, 0f);

        room.SetActive(false);

        foreach (GameObject room in uimanager.rooms)
        {
            room
                .GetComponent<RoomScript>()
                .changeAddButtonVisibility(uimanager.editing);
        }

        uimanager.virtualCamera.m_Lens.OrthographicSize = 7;
    }
}
