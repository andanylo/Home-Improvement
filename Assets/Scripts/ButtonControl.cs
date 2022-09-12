using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityStandardAssets.CrossPlatformInput;

//Control of the player
public class ButtonControl : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator;
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

    public void flipIfNeeded(Vector2 offset){
        Vector2 currentScale = rb.transform.localScale;
        //Flip right or left
        if((offset.x > 0 && currentScale.x < 0) || (offset.x < 0 && currentScale.x > 0)){
            currentScale.x = -1 * currentScale.x;
            rb.transform.localScale =  currentScale;
        }
    }

    public void Update()
    {
        animator.SetBool("isMoving", this.touchedDown);

        if (touchedDown == true){

            Touch touch = Input.GetTouch(0);
            
            //Get direction from touch position
            Vector2 offset = touch.position - new Vector2(btn.transform.position.x, btn.transform.position.y);
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);

            rb.velocity = direction * movementSpeed;
            
            //Facing of the player
            flipIfNeeded(offset);
        }
        else{
            rb.velocity = Vector2.zero;
        }
    }
}
