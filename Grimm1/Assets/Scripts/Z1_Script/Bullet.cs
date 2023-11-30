using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Bullet : MonoBehaviour
{

    public float speed = 10f;
    public Rigidbody2D rb;
    public float lifeTime = 3.0f;

    PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        Destroy(this.gameObject, lifeTime);
        view = GetComponent<PhotonView>();

    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //if(collision.gameObject.CompareTag("Enemy"))
        //    Destroy(this.gameObject);

        if (collision.gameObject.layer == 9)
        {
            collision.gameObject.GetComponent<BossMain>().Bosshealth -= 1.0f;
            Destroy(this.gameObject);
            Debug.Log("enemydetected");
        }


    }


}
