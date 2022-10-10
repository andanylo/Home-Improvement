using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Cinemachine;


public class EditFurniture : MonoBehaviour
{
    //Unity messager
    private UnityMessageManager Manager
{
    get { return GetComponent<UnityMessageManager>(); }
}


    private bool isMoving = false;
    private Furniture currentFurniture;
    public Furniture getCurrentFurniture(){
        return this.currentFurniture;
    }

    private float currentFurnitureRotation = 0;

    
    public GameObject furniture;
    public GameObject player;

    public CinemachineVirtualCamera virtualCamera;


    //Add a new furniture to canvas
     public void addNewFurniture(string furnitureJson){

        //Convert string to json
        furniture.transform.position = new Vector2(0.0f, 0.0f);
        JObject json = JObject.Parse(furnitureJson);
        Furniture editFurniture =  new Furniture("", json["furnitureName"].ToString(), json["furnitureImage"].ToString());
      
        furniture.SetActive(true);



        virtualCamera.m_Follow = furniture.transform;
        this.currentFurniture = editFurniture;
    }

    //Convert furniture template into furniture data
    public FurnitureData convertToFurnitureData(){
        if(currentFurniture != null){
            FurnitureData furnitureData = new FurnitureData();

            furnitureData.name = currentFurniture.name;
            if(string.IsNullOrWhiteSpace(currentFurniture.id)){
                furnitureData.furnitureID = System.Guid.NewGuid().ToString();
            }
            else{
                furnitureData.furnitureID = currentFurniture.id;
            }
            furnitureData.pos = furniture.transform.position;
            furnitureData.rot = new Vector3(0f, 0f, furniture.transform.eulerAngles.z);

            return furnitureData;
        }
        return null;
        


    }

    //Cancel editing of furniture
    public void cancelFurnitureEditing(string message){
    
        furniture.SetActive(false);
        virtualCamera.m_Follow = player.transform;
        currentFurniture = null;
        currentFurnitureRotation = 0f;
    }


    //Check if funiture is inside the house
    public bool checkIfFurnitureIsInside(){
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        
        Vector2 size = furniture.GetComponent<Renderer>().bounds.size;
        Vector2 position = furniture.transform.position;


        float minX = position.x - size.x / 2;
        float maxX = position.x + size.x / 2;

        float minY = position.y - size.y / 2;
        float maxY = position.y + size.y / 2;

        //Check in each room
        foreach(GameObject room in rooms){
            Bounds bounds = room.GetComponent<Collider>().bounds;

            if (bounds.Contains(new Vector2(minX, minY))
            && bounds.Contains(new Vector2(minX, maxY)) &&
            bounds.Contains(new Vector2(maxX, minY)) &&
            bounds.Contains(new Vector2(maxX, maxY))){
                return true;
            }
        }
        return false;
    }

    //Delete furniture
    public void deleteFurniture(string playerID, string furnitureName, string furnitureID){

    }

    public void rotateBy90Degrees(string message){
        currentFurnitureRotation += 90.0f;

        
        //furniture.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, currentFurnitureRotation));
        furniture.transform.Rotate(new Vector3(0f, 0f, currentFurnitureRotation));
        // Vector2 rotationToAdd = new Vector2(0, 90);
        // furniture.transform.Rotate(rotationToAdd);
    }

    //Begin movement
    private void beginMovement(){
        isMoving = true;
        Manager.SendMessageToFlutter("movement:" + isMoving.ToString());
    }

    //Update movement
    private void updateMovement(Touch touch){
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        furniture.transform.position = touchPosition;
    }

    //End movement
    private void endMovement(){
        isMoving = false;
        Manager.SendMessageToFlutter("movement:" + isMoving.ToString());
    }
    void Start(){
     
    }

    private bool previousFurnitureStatus = false;
    // Update is called once per frame
    void Update()
    {
        ;
        if (Input.touchCount > 0 && currentFurniture != null){
            Touch touch = Input.GetTouch(0);
            
            switch (touch.phase)
            {
                //send message to flutter that furniture is moving
                case TouchPhase.Began:
                    beginMovement();
                    
                    break;

                //Change position of furniture
                case TouchPhase.Moved:
                    updateMovement(touch);
                    
                    break;

                //send message to flutter that furniture stopped moving
                case TouchPhase.Ended:
                    endMovement();
                    break;
            }
            
            bool newFurnitureStatus = checkIfFurnitureIsInside();
            if(previousFurnitureStatus != newFurnitureStatus){
                furniture.GetComponent<SpriteRenderer>().color = newFurnitureStatus ? new Color(0, 215, 0, 115) : new Color(215, 0, 0, 115);
            }
            this.previousFurnitureStatus = newFurnitureStatus;



        }
    }
}
