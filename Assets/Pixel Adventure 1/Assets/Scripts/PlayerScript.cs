using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

    SpriteRenderer sprite;
    Rigidbody2D rigidbody;
    BoxCollider2D boxCollider;
    public LayerMask layerMask;
    Animator animator;
    bool control;
    bool hit;

    bool doubleJump;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        doubleJump = false;
        control = true;
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Control

        if (control)
        {
            // Left Arrow
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                sprite.flipX = true;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    rigidbody.linearVelocity = new Vector2(-10f,rigidbody.linearVelocity.y);
                    animator.speed = 2f;
                }
                else
                {
                    rigidbody.linearVelocity = new Vector2(-4f,rigidbody.linearVelocity.y);
                    animator.speed = 1f;
                }
                

            }

            // Right Arrow
            if (Input.GetKey(KeyCode.RightArrow))
            {
                sprite.flipX = false;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    rigidbody.linearVelocity = new Vector2(10f,rigidbody.linearVelocity.y);
                    animator.speed = 2f;
                }
                else
                {
                    rigidbody.linearVelocity = new Vector2(4f,rigidbody.linearVelocity.y);
                    animator.speed = 1f;
                }    
            }

            // Space
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded()){
                    sprite.flipX = false;
                    rigidbody.linearVelocity = new Vector2(0,5f);
                    doubleJump = true;
                }
                else if (doubleJump)
                {
                    rigidbody.linearVelocity = new Vector2(0,5f);
                    doubleJump = false;
                    
                }
            }
        }


        if (!hit)
        {
            //Animations
            if (isGrounded() && !Input.GetKey(KeyCode.LeftArrow) &&  !Input.GetKey(KeyCode.RightArrow))
            {
                animator.SetInteger("status",0);
            }
            else if(isGrounded() && ( Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
            {
                animator.SetInteger("status",1);
            }
            else if(rigidbody.linearVelocity.y > 0)
            {
                if(doubleJump)
                {
                    animator.SetInteger("status",2);
                }
                else
                {
                    animator.SetInteger("status",4);
                }
            }
            else if(rigidbody.linearVelocity.y < 0)
            {
                animator.SetInteger("status",3);
            }    
        }
        

    }

    public void goToIdle()
    {
        control = true;
        hit = false;

        animator.SetInteger("status",1);
    }


    public bool isGrounded()
    {
        return Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0f,
            Vector2.down,
            0.1f,
            layerMask
             );
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Limite")
        {
            animator.SetTrigger("delete");
        }
        if(collision.gameObject.tag == "Hit")
        {
            control = false;
            hit = true;   

            animator.SetInteger("status",5);

            if (sprite.flipX)
            {
                rigidbody.linearVelocity = new Vector2(5f,5f);
            }
            else
            {
                rigidbody.linearVelocity = new Vector2(-5f,5f);
            }
        }
    }

    public void goToDelete()
    {
        SceneManager.LoadScene(0);
    }
}
