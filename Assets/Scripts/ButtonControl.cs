using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonControl : MonoBehaviour
{

    public Rigidbody2D rb;

    private float movementSpeed = 5f;

    

    public void RightDown(){
        rb.velocity = Vector2.right * movementSpeed;
    }

    public void TopDown(){
        rb.velocity = Vector2.up * movementSpeed;
    }

    public void DownDown(){
        rb.velocity = Vector2.down * movementSpeed;
    }

    public void LeftDown(){
        rb.velocity = Vector2.left * movementSpeed;
    }

    public void StopMoving(){
        rb.velocity = Vector2.zero;
    }


    void FixedUpdate()
    {
       // rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }
}
