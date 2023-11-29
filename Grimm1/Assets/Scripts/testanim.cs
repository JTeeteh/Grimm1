using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testanim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.GetComponent<Animator>().Play("idle_anim");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.GetComponent<Animator>().Play("summon");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.GetComponent<Animator>().Play("skill1");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //this.GetComponent<Animator>().Play("dead");
        }
    }

    void kys()
    {
        Destroy(this.gameObject);
    }

}
