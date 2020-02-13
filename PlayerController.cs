using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{

    public float jumpTakeOffSpeed = 7;
    public float doubleJumpSpeed = 7;
    public float maxSpeed = 7;
    public float wallSlideSpeedMax = 3;
    private bool m_FacingRight;
    
    private SpriteRenderer spriteRenderer;

    private bool canDoubleJump;

    public Vector2 wallLeap;

    // Start is called before the first frame update
    void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Computevelocity()
    {
        if (grounded)
        {
            canDoubleJump = true;
        }

       bool wallSliding = false;
        if (touchingWall && !grounded && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }
        }

        if (velocity.x < 0 && !m_FacingRight) { Flip(); } else if (velocity.x > 0 && m_FacingRight) { Flip(); }

        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("horizontal");

        if (Input.GetButtonDown("jump"))
        {
            if (grounded)
            {
                velocity.y = jumpTakeOffSpeed;
            }
            else if (Input.GetButtonDown("jump") && canDoubleJump && !wallSliding)
            {
                canDoubleJump = false;
                velocity.y = doubleJumpSpeed;
            }
            else if (wallSliding)
            {
                velocity.y = wallLeap.y;
                
            }
        }
        else if (Input.GetButtonUp("jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        /*bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        */

        targetVelocity = move * maxSpeed;
       
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
