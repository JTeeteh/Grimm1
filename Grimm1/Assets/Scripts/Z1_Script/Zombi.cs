using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombi : MonoBehaviour
{
    [SerializeField]
    Transform Player;
    [SerializeField]
    float agrorange;
    [SerializeField]
    float speed;


    [SerializeField]
    float kolength;
    [SerializeField]
    float koforce;


    [SerializeField]
    float HP;
    bool lookother;
    bool IsAgro;
    Rigidbody2D rb2d;

    public Animator animator;
    public GameObject spawner;
    public GameObject blood;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        rb2d = GetComponent<Rigidbody2D>();
        spawner = GameObject.Find("Spawner");

    }

    // Update is called once per frame
    void Update()
    {
        float distoplayer = Vector2.Distance(transform.position, Player.position);
        //print(distoplayer);
        if (distoplayer < agrorange)
        {
            IsAgro = true;
            chase();
        }
        else
        {
            patrol();
            IsAgro = false;
            //stopchase();
        }
        //Debug.Log("start contact with " + collision.gameObject.name);
        //if we collide with a bullet, destroy self and the bullet gameobject
        /*
        //METHOD 1 checking for collision with bullet: Check if the gameobject has component attached to it
        if(collision.gameObject.GetComponent<Bullet>() != null)
        {
            Debug.Log("collided with a bullet.");
        }*/
        //METHOD 2 checking for collision with bullet: Check the tag of the gameobject
        if (gameObject.tag == "playerBullet")
        {
            //Debug.Log("collided with a bullet.");
            HP -= 1.0f;
            Destroyobj(gameObject);
            
        }
        
        if (HP <= 0)
        { 
            Destroy(gameObject);
            
        }
        

    }

    void chase()
    {

        if (transform.position.x < Player.position.x)
        {
         rb2d.velocity = new Vector2(speed, 0);
         transform.localScale = new Vector2(1, 1);
            lookother = false;
        }
        else if (transform.position.x > Player.position.x)
        {
         rb2d.velocity = new Vector2(-speed, 0);
         transform.localScale = new Vector2(-1, 1);
            lookother = true;
        }
        animator.SetBool("IsAgro", true);

    }

    void patrol()
    {
        if(lookother == false)
        {
        rb2d.velocity = new Vector2(speed / 4, 0);

        }
        else
        rb2d.velocity = new Vector2(-speed / 4, 0);
        animator.SetBool("IsAgro", false);
    }

    /*public void OnAgro()
    {
        animator.SetBool("IsAgro", true);
    }*/

    /*
    private void flip()
    {
       transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
       speed *= -1;
    }
    
    */
    private void stopchase()
    {
        rb2d.velocity = new Vector2(0, 0);
    }


    

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("start contact with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Bullet")
        {
            Instantiate(blood, transform.position, Quaternion.identity);
            HP -= 1.0f;
            Destroyobj(collision.gameObject);
            rb2d.velocity = new Vector2(kolength, koforce);
            

        }
        if (HP <= 0)
        {
            Destroy(gameObject);
            ScoreScript.scoreValue += 1;
        }
    }
    
    private void Destroyobj(GameObject collidedObject)
    {
        Destroy(collidedObject);
    }
    


}