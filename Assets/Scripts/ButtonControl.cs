using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonControl : MonoBehaviour
{

    public Rigidbody2D rb;

    public float movementSpeed = 5f;
    Vector2 movement;
    // Start is called before the first frame update
    public void buttonAction(){

        


        movement.Set(1, 0);
    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }
}
