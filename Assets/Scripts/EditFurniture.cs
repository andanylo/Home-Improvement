using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class EditFurniture : MonoBehaviour
{
    //Unity messager
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

    private bool isMoving = false;

    private Furniture _currentFurniture;

    private Furniture currentFurniture{
        get => _currentFurniture;
        set{
            this._currentFurniture = value;
            editingStatusDidChange(value);
        }
    }
    //If editing or canceled editing fuurniture
    public void editingStatusDidChange(Furniture value){
            uimanager.editRoom.enableAddButtons(uimanager.rooms, value != null ? false : uimanager.editing);
            if(value != null){
                bool isInside = checkIfFurnitureIsInside();
                furniture.GetComponent<SpriteRenderer>().color = isInside ? new Color(0, 215, 0, 115) : new Color(215, 0, 0, 115);
                Manager
                    .SendMessageToFlutter("inside:" +
                    isInside.ToString());
                
                 uimanager.virtualCamera.m_Follow = furniture.transform;
            }
            else{

            }
    }

    public Furniture getCurrentFurniture()
    {
        return this.currentFurniture;
    }

    private float currentFurnitureRotation = 0;

    //Furniture game object that was clicked to edit
    private GameObject editingFurnitureObject;

    public GameObject furniture;

    //Add a new furniture to canvas
    public void addEmptyFurniture(string furnitureJson)
    {
        //Convert string to json
        JObject json = JObject.Parse(furnitureJson);
        Furniture editFurniture =
            new Furniture("",
                json["furnitureName"].ToString(),
                json["furnitureImage"].ToString());

        setTemplateActive (editFurniture);
    }

    //Set empty furniture template active
    public void setTemplateActive(Furniture template)
    {
        GameObject prefab =
            Resources
                .Load<GameObject>("Prefabs/Furnitures/" + template.imageName);

        furniture.transform.localScale =
            new Vector2(prefab.transform.localScale.x,
                prefab.transform.localScale.y);
        furniture.transform.position = Camera.main.transform.position;
        furniture.SetActive(true);
        
        Sprite texture =
            Resources.Load<Sprite>("Textures/Furnitures/" + template.imageName);
        furniture.GetComponent<SpriteRenderer>().sprite = texture;


       
        
        this.currentFurniture = template;
    }

    //Set furniture template object from furniture object
    public void setTemplateActive(
        FurnitureData data,
        GameObject editingFurnitureObject
    )
    {
        Furniture templateFromData =
            new Furniture(data.furnitureID, data.name, data.name);

        furniture.transform.position =
            editingFurnitureObject.transform.position;
        furniture.transform.rotation =
            editingFurnitureObject.transform.rotation;
        furniture.transform.localScale =
            editingFurnitureObject.transform.localScale;

        //Set texture if not null
        Sprite furnitureObjectTexture =
            editingFurnitureObject
                .GetComponentInChildren<SpriteRenderer>()
                .sprite;
        if (furnitureObjectTexture != null)
        {
            furniture.GetComponent<SpriteRenderer>().sprite =
                furnitureObjectTexture;
        }

        furniture.SetActive(true);

        
        this.currentFurniture = templateFromData;

        
        this.currentFurnitureRotation = data.rot.z;
    }

    //Enable furniture editing inside flutter app
    private void enableFurnitureEditing(FurnitureData furnitureToEdit)
    {
        Manager
            .SendMessageToFlutter("enableFurnitureEditing:{\"name\": \"" +
            furnitureToEdit.name +
            "\", \"id\": \"" +
            furnitureToEdit.furnitureID +
            "\"}");
    }

    //Convert furniture template into furniture data
    public FurnitureData convertToFurnitureData()
    {
        if (currentFurniture != null)
        {
            FurnitureData furnitureData = new FurnitureData();

            furnitureData.name =
                currentFurniture.name.ToLower().Replace(' ', '_');
            if (string.IsNullOrWhiteSpace(currentFurniture.id))
            {
                furnitureData.furnitureID = System.Guid.NewGuid().ToString();
            }
            else
            {
                furnitureData.furnitureID = currentFurniture.id;
            }
            furnitureData.pos = furniture.transform.position;
            furnitureData.rot =
                new Vector3(0f, 0f, furniture.transform.eulerAngles.z);

            return furnitureData;
        }
        return null;
    }

    //Cancel editing of furniture and reset values
    public void cancelEditing(string message)
    {
        reset();
    }

    public void reset()
    {
        currentFurniture = null;
        currentFurnitureRotation = 0f;

        //Reset furniture
        furniture.transform.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        furniture.transform.position = new Vector2(0.0f, 0.0f);

        furniture.SetActive(false);
        if(uimanager.editing){
            uimanager.virtualCamera.m_Follow = null;
        }
        

        //Set object to active again after completing editing
        if (editingFurnitureObject != null)
        {
            editingFurnitureObject.SetActive(true);
        }
        this.editingFurnitureObject = null;
    }

    //Check if funiture is inside the house
    public bool checkIfFurnitureIsInside()
    {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");

        Bounds furnitureBounds = furniture.GetComponent<Renderer>().bounds;
        Vector2 size = furnitureBounds.size;
        Vector2 position = furniture.transform.position;

        float minX = position.x - size.x / 2;
        float maxX = position.x + size.x / 2;

        float minY = position.y - size.y / 2;
        float maxY = position.y + size.y / 2;

        //Check if current furniture is not inside other furniture
        foreach (GameObject furn in uimanager.furnitures)
        {
            if (!ReferenceEquals(furn, this.editingFurnitureObject))
            {
                Bounds objectBounds =
                    furn.GetComponentInChildren<Renderer>().bounds;
                if (furnitureBounds.Intersects(objectBounds))
                {
                    return false;
                }
            }
        }

        //Check in each room
        foreach (GameObject room in rooms)
        {
            Bounds bounds = room.GetComponent<Collider>().bounds;
            if (
                bounds.Contains(new Vector2(minX, minY)) &&
                bounds.Contains(new Vector2(minX, maxY)) &&
                bounds.Contains(new Vector2(maxX, minY)) &&
                bounds.Contains(new Vector2(maxX, maxY))
            )
            {
                return true;
            }
        }

        return false;
    }

    //Delete furniture
    public void deleteFurniture(string furnitureID)
    {
        GameObject furnitureToDelete =
            uimanager
                .furnitures
                .Find(o =>
                    o
                        .GetComponent<FurnitureScript>()
                        .furnitureData
                        .furnitureID ==
                    furnitureID);

        if (furnitureToDelete != null)
        {
            uimanager.furnitures.Remove (furnitureToDelete);
            Destroy (furnitureToDelete);
        }
    }

    public void rotateBy90Degrees(string message)
    {
        currentFurnitureRotation += 90.0f;

        furniture.transform.eulerAngles = new Vector3(0f, 0f, currentFurnitureRotation);
    }

    //Begin movement
    private void beginMovement()
    {
        isMoving = true;
        Manager.SendMessageToFlutter("movement:" + isMoving.ToString());
    }

    //Update movement
    private void updateMovement(Touch touch)
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        furniture.transform.position = touchPosition;
    }

    //End movement
    private void endMovement()
    {
        isMoving = false;
        Manager.SendMessageToFlutter("movement:" + isMoving.ToString());
    }

    void Start()
    {
        uimanager.editing = true;
    }

    private bool previousFurnitureStatus = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && currentFurniture != null)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                //send message to flutter that furniture is moving
                case TouchPhase.Began:
                    beginMovement();

                    break;
                //send message to flutter that furniture stopped moving
                case TouchPhase.Ended:
                    endMovement();
                    break;
                default:
                    break;
            }

            if (isMoving)
            {
                updateMovement (touch);
            }

            bool newFurnitureStatus = checkIfFurnitureIsInside();
            if (previousFurnitureStatus != newFurnitureStatus)
            {
                furniture.GetComponent<SpriteRenderer>().color =
                    newFurnitureStatus
                        ? new Color(0, 215, 0, 115)
                        : new Color(215, 0, 0, 115);
                Manager
                    .SendMessageToFlutter("inside:" +
                    newFurnitureStatus.ToString());
            }
            this.previousFurnitureStatus = newFurnitureStatus;
        } //Else activate furniture editing if clicked on furniture during editing
        else if (
            Input.touchCount > 0 &&
            currentFurniture == null &&
            uimanager.editing == true && uimanager.editRoom.currentRoom == null
        )
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit =
                    Physics2D
                        .Raycast(Camera.main.ScreenToWorldPoint(touch.position),
                        Vector2.zero);
                if (hit.collider != null)
                {
                    //Check if Furniture was clicked to
                    if (
                        hit
                            .collider
                            .gameObject
                            .GetComponent<FurnitureScript>() !=
                        null
                    )
                    {
                        GameObject clickedFurniture = hit.collider.gameObject;
                        FurnitureData clickedData =
                            clickedFurniture
                                .GetComponent<FurnitureScript>()
                                .furnitureData;

                        if (clickedFurniture != null && clickedData != null)
                        {
                            this.editingFurnitureObject = clickedFurniture;
                            clickedFurniture.SetActive(false);

                            enableFurnitureEditing (clickedData);
                            setTemplateActive (clickedData, clickedFurniture);
                        }
                    }
                }
            }
        }
    }
}
