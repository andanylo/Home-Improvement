using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityStandardAssets.CrossPlatformInput;
public class ButtonControl : MonoBehaviour
{

    public Rigidbody2D rb;

    private float movementSpeed = 2.5f;
    private bool touchedDown = false;
    public Button btn;

    public void touchDownAction(){
        this.touchedDown = true;
    }
    public void touchUpAction(){
        this.touchedDown = false;
    }



    public void StopMoving(){
        rb.velocity = Vector2.zero;
    }



    public void Update()
    {
        if (touchedDown == true){

            Touch touch = Input.GetTouch(0);
            
            Vector2 offset = touch.position - new Vector2(btn.transform.position.x, btn.transform.position.y);
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);

            rb.velocity = direction * movementSpeed;
            Debug.Log(direction);
        }
        else{
            rb.velocity = Vector2.zero;
        }
    }
}
