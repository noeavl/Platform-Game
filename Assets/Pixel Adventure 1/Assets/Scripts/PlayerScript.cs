using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public TextMeshProUGUI text_apples;
    int countApples;
    SpriteRenderer sprite;
    Rigidbody2D rigidbody;
    BoxCollider2D boxCollider;
    public LayerMask layerMask;
    Animator animator;
    bool control;
    bool hit;
    bool water;
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
        water= false;

        countApples = 0;
        text_apples.SetText(""+countApples);
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
                if (water)
                {
                    rigidbody.linearVelocity = new Vector2(0,4f);
                    doubleJump = true;
                } else if (isGrounded()){
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

         if(collision.gameObject.tag == "Rino")
        {
            if (isGrounded())
            {
                animator.SetTrigger("delete");
            }
            else
            {
                rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocityX,20);
                collision.gameObject.GetComponent<Animator>().SetTrigger("rinoTrigger");
            }
        }

        if(collision.gameObject.tag == "Limite" || collision.gameObject.tag == "Spikehead" )
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

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Plataforma" )
        {
            transform.SetParent(collision.gameObject.transform);
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Plataforma" )
        {
            transform.SetParent(null);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spikehead")
        {
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            collision.GetComponent<Rigidbody2D>().freezeRotation = true;
            collision.GetComponent<Rigidbody2D>().gravityScale = 5;
            collision.GetComponent<Rigidbody2D>().mass = 100;

        }

        if (collision.gameObject.tag == "Apple")
        {
            countApples++;
            text_apples.SetText(""+countApples);
            Destroy(collision.gameObject);

        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trampoline")
        {
            collision.gameObject.GetComponent<Animator>().SetInteger("tramp_status",0);
        }

        if (collision.gameObject.tag == "Water")
        {
              rigidbody.gravityScale = 5.5f;
              water = false;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            rigidbody.gravityScale = 0.5f;   
        }

        if (collision.gameObject.tag == "Trampoline")
        {
            rigidbody.linearVelocity = new Vector2(0,9f);
            animator.gameObject.GetComponent<Animator>().SetInteger("tramp_status",1);
        }

        if (collision.gameObject.tag == "Fan")
        {
            rigidbody.linearVelocity = new Vector2(0,4f);
        }
    }

    public void goToDelete()
    {
        SceneManager.LoadScene(0);
    }
}
