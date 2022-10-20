using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomScript : MonoBehaviour
{
    public List<GameObject> addButtons;
    public List<GameObject> doors;

    private RoomData _roomData;
    public RoomData roomData
    {
        get => _roomData;
        set{
            this._roomData = value;
            transform.position = this._roomData.pos;
            transform.Rotate(this._roomData.rot);

            //Change direction of doors and buttons based on rotation
            foreach(GameObject door in doors){
                door.GetComponent<DoorScript>().direction = DirectionManager.Instance.getNewDirectionFromDegrees(this._roomData.rot.z, door.GetComponent<DoorScript>().direction);
            }
            foreach(GameObject addButton in addButtons){
                addButton.GetComponent<AddRoomButtonScript>().buttonDirection = DirectionManager.Instance.getNewDirectionFromDegrees(this._roomData.rot.z, addButton.GetComponent<AddRoomButtonScript>().buttonDirection);
            }

            changeDoorVisibility(true);

        }
    }


    private EditRoom editManager;

    private bool editingRoom = false; 
    //Enable / disable edit buttons with doors
    public void changeEditingStatus(bool isEditing){
        if(roomData != null){
            this.editingRoom = isEditing;

            changeDoorVisibility(!isEditing);
            changeAddButtonVisibility(isEditing);
            

        }
    }

    public void changeAddButtonVisibility(bool isEditing){
        if(isEditing){
                var addButtonsWithoutConnections = addButtons.Where(addButton => !(roomData.connections.Any(connection => connection.direction == addButton.GetComponent<AddRoomButtonScript>().buttonDirection))).ToList();
            
                //Enable or disable add buttons that don't have connections
                foreach(GameObject addButton in addButtonsWithoutConnections){
                    addButton.SetActive(true);
                }
            }
            else{
                foreach(GameObject addButton in addButtons){
                    addButton.SetActive(false);
                }
            }
    }

    //Change visibility of doors, including those that doesn't have connections
    public void changeDoorVisibility(bool shouldShowDoorsWithoutConnections){
        var doorsWithoutConnections = doors.Where(door => !(roomData.connections.Any(connection => connection.direction == door.GetComponent<DoorScript>().direction))).ToList();
        var doorsWithConnections =  doors.Where(door => roomData.connections.Any(connection => connection.direction == door.GetComponent<DoorScript>().direction)).ToList();
        foreach(GameObject activeDoors in doorsWithConnections){
            activeDoors.SetActive(false);
        }

        foreach(GameObject inactiveDoors in doorsWithoutConnections){
            inactiveDoors.SetActive(shouldShowDoorsWithoutConnections);
        }
    }

    public void setEditRoomManager(EditRoom editRoom){
        if(this.editManager == null){
            this.editManager = editRoom;
        }
    } 

    void Update(){

        //Check if add button was clicked
        if(Input.touchCount > 0 && editingRoom){
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began){
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                
                if(hit.collider != null){



                    //Check if click hit has addbutton script
                    if(hit.collider.gameObject.GetComponent<AddRoomButtonScript>() != null){
                        Debug.Log(hit.collider.gameObject);
                        this.editManager.didClickOnAddButton(transform.gameObject, roomData, hit.collider.gameObject.GetComponent<AddRoomButtonScript>().buttonDirection);
                    }
                }

            }
        }
    }



    



    
}
