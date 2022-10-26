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

    private float movementSpeed = 3.5f;

    private bool touchedDown = false;

    public Button btn;

    public UnityEvent MovingRight = new UnityEvent();

    public UnityEvent MovingLeft = new UnityEvent();

    public UnityEvent MovingUp = new UnityEvent();

    public UnityEvent MovingDown = new UnityEvent();

    public void touchDownAction()
    {
        this.touchedDown = true;
    }

    public void touchUpAction()
    {
        this.touchedDown = false;
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    public void flipIfNeeded(Vector2 offset)
    {
        Vector2 currentScale = rb.transform.localScale;

        //Flip right or left
        if (
            (offset.x > 0 && currentScale.x < 0) ||
            (offset.x < 0 && currentScale.x > 0)
        )
        {
            currentScale.x = -1 * currentScale.x;
            rb.transform.localScale = currentScale;
        }
    }

    public void Update()
    {
        animator.SetBool("isMoving", this.touchedDown);

        if (touchedDown == true)
        {
            Touch touch = Input.GetTouch(0);

            //Get direction from touch position
            Vector2 offset =
                touch.position -
                new Vector2(btn.transform.position.x, btn.transform.position.y);
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);

            if (offset.x > 100)
            {
                MovingRight.Invoke();
            }
            else if (offset.x < -100)
            {
                MovingLeft.Invoke();
            }
            else if (offset.y > 100)
            {
                MovingUp.Invoke();
            }
            else if (offset.y < -100)
            {
                MovingDown.Invoke();
            }

            rb.velocity = direction * movementSpeed;

            //Facing of the player
            flipIfNeeded (offset);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
