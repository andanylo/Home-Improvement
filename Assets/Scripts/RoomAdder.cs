using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class RoomAdder : MonoBehaviour
{
    public UIManager uimanager;

    // Start is called before the first frame update
    void Start()
    {
        //recieveRoomsFromDatabase(" {\"connections\":[],\"xPos\": 2.092411994934082,\"xRot\": 0,\"yPos\": 0.10121464729309082,\"yRot\": 0}");
    }

    //Get furniture data object based on parameters
    private RoomData
    getRoomData(string key_word, string roomID, RoomJSON jsonValues)
    {
        RoomData newRoom = new RoomData(jsonValues);
        newRoom.key_word = key_word;
        newRoom.id = roomID;
        return newRoom;
    }

    public void recieveRoomsFromDatabase(string jsonString)
    {
        //Create a standard living room
        if (string.IsNullOrWhiteSpace(jsonString))
        {
            RoomData standardLivingRoom = new RoomData();
            standardLivingRoom.id = System.Guid.NewGuid().ToString();
            standardLivingRoom.key_word = "living_room";
            standardLivingRoom.pos = new Vector2(0.0f, 0.0f);

            addRoomToScene (standardLivingRoom);

            SaveManager
                .saveOrUpdateRoom(uimanager.Manager, standardLivingRoom, false);
        }
        else
        {
            Dictionary<string, JObject> values =
                JsonConvert
                    .DeserializeObject<Dictionary<string, JObject>>(jsonString);

            List<RoomData> list = new List<RoomData>();

            // //Foreach room type
            foreach (KeyValuePair<string, JObject> entry in values)
            {
                string key_word = entry.Key;
                Dictionary<string, JObject> obj =
                    entry.Value.ToObject<Dictionary<string, JObject>>();

                //Foreach room object
                foreach (KeyValuePair<string, JObject> room in obj)
                {
                    string roomID = room.Key;

                    RoomJSON jsonValues =
                        RoomJSON
                            .convertFromJSON(room
                                .Value
                                .ToString(Newtonsoft.Json.Formatting.None));
                    RoomData roomData =
                        getRoomData(key_word, roomID, jsonValues);
                    if (roomData != null)
                    {
                        list.Add (roomData);
                    }
                }
            }
            fetchRooms (list);
        }
    }

    //Add furniture from fdatabase based on list of data
    public void fetchRooms(List<RoomData> roomList)
    {
        foreach (GameObject roomObject in uimanager.rooms)
        {
            Destroy (roomObject);
        }

        foreach (RoomData data in roomList)
        {
            addRoomToScene (data);
        }
    }

    public void updateRoom(RoomData roomData)
    {
        foreach (GameObject room in uimanager.rooms)
        {
            room.SetActive(true);
            RoomScript roomScript = room.GetComponent<RoomScript>();
            if (roomScript != null)
            {
                //Check ids of room data
                if (roomScript.roomData.id == roomData.id)
                {
                    room.GetComponent<RoomScript>().roomData = roomData;
                }
            }
        }
    }

    public void addRoomToScene(RoomData roomData)
    {
        GameObject room;

        GameObject prefab =
            Resources.Load<GameObject>("Prefabs/Rooms/" + roomData.key_word);

        room = Instantiate(prefab) as GameObject;
        room.AddComponent<RoomScript>();
        room.name = "Room_Object";

        room.GetComponent<RoomScript>().roomData = roomData;
        room.GetComponent<RoomScript>().editingRoom = uimanager.editing;
        room.GetComponent<RoomScript>().setEditRoomManager(uimanager.editRoom);

        uimanager.rooms.Add (room);
    }
}
