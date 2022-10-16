using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditRoom : MonoBehaviour
{
    

    private UnityMessageManager Manager
    {
     get { return GetComponent<UnityMessageManager>(); }
    }
    private UIManager uimanager{
        get{return GetComponent<UIManager>();}
    }

    public Room getCurrentRoom(){
        return currentRoom;
    }

    private Room currentRoom;
    private float currentRoomRotation = 0f;

    private GameObject editingRoom;

    public GameObject room;

    private GameObject senderRoom;
    private RoomData senderData;
    private Direction? senderDirection;
    private Direction? originalConnectionDirection;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Convert current room into room data
    public void convertToRoomData(){

    }   

    //Enable add buttons
    public void enableAddButtons(List<GameObject> rooms, bool isEditing){
        foreach(GameObject room in rooms){
            room.GetComponent<RoomScript>().setEditRoomManager(this);
            room.GetComponent<RoomScript>().changeEditingStatus(isEditing);
        }
    }

    
    // public Vector2 getRoomCenter(Direction senderConnectionDirection){
    //     if(this.senderDirection != null && editingRoom != null){
    //         switch(senderDirection){
    //             case Direction.right:

    //         }
    //     }
    //     return null;
    // }

     

    public void didClickOnAddButton(GameObject senderRoom, RoomData senderData, Direction directionToAdd){
        this.senderData = senderData;
        this.senderDirection = directionToAdd;
        this.originalConnectionDirection = DirectionManager.Instance.findOppositeDirection(directionToAdd);

        Manager.SendMessageToFlutter("presentRoomPopUp:true");
    }

    public void reset(){
        this.currentRoom = null;
        this.senderData = null;
        this.senderDirection = null;
        this.originalConnectionDirection = null;
        this.senderRoom = null;

    }

}
