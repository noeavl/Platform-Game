using UnityEngine;

public class RinoScript : MonoBehaviour
{
    Rigidbody2D rigigbody;
    SpriteRenderer sprite;
    float time_move;

    int par_impar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigigbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        time_move = Time.time;
        par_impar = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > time_move)
        {
            if (par_impar % 2 == 0)
            {
                rigigbody.linearVelocity = new Vector2(10,0);
                sprite.flipX = true;
            }
            else
            {
                rigigbody.linearVelocity = new Vector2(-10,0);   
                sprite.flipX = false;
            }

            par_impar++;
            time_move++;
        }
    }

    public void goToEnd()
    {
        Destroy(gameObject);
    }
}
