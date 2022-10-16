using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAdder : MonoBehaviour
{

    public UIManager uimanager;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void recieveRoomsFromDatabase(string jsonString){

        //Create a standard living room
        if(string.IsNullOrWhiteSpace(jsonString)){
            RoomData standardLivingRoom = new RoomData();
            standardLivingRoom.id = System.Guid.NewGuid().ToString();
            standardLivingRoom.key_word = "living_room";
            standardLivingRoom.pos = new Vector2(0.0f, 0.0f);
            
            addRoom(standardLivingRoom);

            SaveManager.saveOrUpdateRoom(uimanager.Manager, standardLivingRoom, false);
        }
    }


    public void addRoom(RoomData roomData){
        GameObject room;

        GameObject prefab = Resources.Load<GameObject>("Prefabs/Rooms/" + roomData.key_word);
        
        room = Instantiate(prefab) as GameObject;
        room.AddComponent<RoomScript>();
        room.name = "Room_Object";
        
        room.GetComponent<RoomScript>().roomData = roomData;
        room.GetComponent<RoomScript>().setEditRoomManager(uimanager.editRoom);

        uimanager.rooms.Add(room);
    }

}
