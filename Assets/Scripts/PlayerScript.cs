using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Button taskButton;

    [SerializeField]
    public ButtonControl moveButton;

    public Rigidbody2D player;

    public UIManager uimanager;

    private RaycastHit2D hit;

    public PlayerRay makeFurnitureInteractble = new PlayerRay();

    private float taskmindistancedetection = 1f;

    // Start is called before the first frame update
    void Start()
    {
        hit =
            Physics2D
                .Raycast(new Vector2(this.transform.position.x + 1,
                    this.transform.position.y),
                Vector2.right);

        Vector2 right = transform.TransformDirection(Vector2.right) * 1;
        Debug
            .DrawRay(new Vector2(this.transform.position.x + 1,
                this.transform.position.y),
            right,
            Color.red);

        // moveButton.MovingRight.AddListener (playerRaycastRight);
        // moveButton.MovingLeft.AddListener (playerRaycastLeft);
        // moveButton.MovingUp.AddListener (playerRaycastUp);
        // moveButton.MovingDown.AddListener (playerRaycastDown);
    }

    // public void playerRaycastUp()
    // {
    //     hit =
    //         Physics2D
    //             .Raycast(new Vector2(this.transform.position.x,
    //                 this.transform.position.y + 1),
    //             Vector2.up);
    //     Vector2 up = transform.TransformDirection(Vector2.up) * 1;
    //     Debug
    //         .DrawRay(new Vector2(this.transform.position.x,
    //             this.transform.position.y + 1),
    //         up,
    //         Color.red);
    // }
    // public void playerRaycastDown()
    // {
    //     hit =
    //         Physics2D
    //             .Raycast(new Vector2(this.transform.position.x,
    //                 this.transform.position.y - 1),
    //             Vector2.down);
    //     Vector2 down = transform.TransformDirection(Vector2.down) * 1;
    //     Debug
    //         .DrawRay(new Vector2(this.transform.position.x,
    //             this.transform.position.y - 1),
    //         down,
    //         Color.red);
    // }
    // public void playerRaycastLeft()
    // {
    //     hit =
    //         Physics2D
    //             .Raycast(new Vector2(this.transform.position.x - 1,
    //                 this.transform.position.y),
    //             Vector2.left);
    //     Vector2 left = transform.TransformDirection(Vector2.left) * 1;
    //     Debug
    //         .DrawRay(new Vector2(this.transform.position.x - 1,
    //             this.transform.position.y),
    //         left,
    //         Color.red);
    // }
    // public void playerRaycastRight()
    // {
    //     hit =
    //         Physics2D
    //             .Raycast(new Vector2(this.transform.position.x + 1,
    //                 this.transform.position.y),
    //             Vector2.right);
    //     Vector2 right = transform.TransformDirection(Vector2.right) * 1;
    //     Debug
    //         .DrawRay(new Vector2(this.transform.position.x + 1,
    //             this.transform.position.y),
    //         right,
    //         Color.red);
    // }
    //Check if player is close to any of the task
    public bool checkIfCloseToFurniture()
    {
        foreach (GameObject furniture in uimanager.furnitures)
        {
            Collider2D furnitureBounds =
                furniture.GetComponent<BoxCollider2D>();

            //Calculate closest point to a collider from player center
            Vector2 closestPoint =
                furnitureBounds.ClosestPoint(player.transform.position);

            float distance =
                DistanceFinder
                    .distance(closestPoint, player.transform.position);

            //Debug.Log($"{closestPoint} {distance}");
            if (distance <= taskmindistancedetection)
            {
                return true;
            }
        }
        return false;
    }

    //Get closest task to a player
    public GameObject getClosestTask()
    {
        if (uimanager.furnitures.Count > 0)
        {
            GameObject closestFurniture = uimanager.furnitures[0];
            Collider2D playerBounds =
                uimanager.player.GetComponent<BoxCollider2D>();
            Collider2D furnitureBounds =
                closestFurniture.GetComponent<BoxCollider2D>();

            Vector2 closestPoint =
                furnitureBounds.ClosestPoint(player.transform.position);

            float closestDistance =
                DistanceFinder
                    .distance(closestPoint, player.transform.position);

            foreach (GameObject furniture in uimanager.furnitures)
            {
                furnitureBounds = furniture.GetComponent<BoxCollider2D>();
                closestPoint =
                    furnitureBounds.ClosestPoint(player.transform.position);
                float distance =
                    DistanceFinder
                        .distance(closestPoint, player.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFurniture = furniture;
                }
            }
            return closestFurniture;
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkIfCloseToFurniture())
        {
            GameObject furniture = getClosestTask();
            if (furniture.GetComponent<FurnitureScript>() != null)
            {
                uimanager.currentClosestFurnitureData =
                    furniture.GetComponent<FurnitureScript>().furnitureData;
                makeFurnitureInteractble
                    .Invoke(furniture
                        .GetComponent<FurnitureScript>()
                        .furnitureData
                        .furnitureID,
                    furniture
                        .GetComponent<FurnitureScript>()
                        .furnitureData
                        .name);
            }
            else
            {
                taskButton.gameObject.SetActive(false);
            }
        }
        else
        {
            taskButton.gameObject.SetActive(false);
        }
        // if (hit.collider != null)
        // {
        //     if (hit.collider.GetComponent<FurnitureScript>())
        //     {
        //         Debug.Log("ssssssssssssssssssssssssssss");
        //         GameObject furnitureObj = hit.collider.gameObject;
        //         Debug
        //             .Log("iddddddd " +
        //             furnitureObj
        //                 .GetComponent<FurnitureScript>()
        //                 .furnitureData
        //                 .furnitureID);
        //         makeFurnitureInteractble
        //             .Invoke(furnitureObj
        //                 .GetComponent<FurnitureScript>()
        //                 .furnitureData
        //                 .furnitureID,
        //             furnitureObj
        //                 .GetComponent<FurnitureScript>()
        //                 .furnitureData
        //                 .name);
        //     }
        //     else
        //     {
        //     }
        // }
        // else
        // {
        // }
    }
}

public class PlayerRay : UnityEvent<string, string> { }

public class DistanceFinder
{
    //Closest point to
    public static float distance(Vector2 closestPoint, Vector2 toCenter)
    {
        float distance = Vector2.Distance(closestPoint, toCenter);

        return distance;
    }
}
