using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Photon.Pun;
public class BossMain : MonoBehaviourPunCallbacks
{
    private Animator animator;
    public float animationModifier = 1;
    public float Bosshealth = 100.0f;
    public float AttackTimer = 0;
    public float MoveSpeed = 10.0f;
    bool Melee = false;
    [SerializeField]
    private Transform player;
    public float distancePlayer = 0.0f;
    // Start is called before the first frame update

    [SerializeField]
    private List<GameObject> playerList;
    [SerializeField]
    private Transform markedTarget;
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {

        playerList = GameObject.FindGameObjectsWithTag("Player").ToList();

     AttackTimer += Time.deltaTime * animationModifier;
        if (AttackTimer > 3)
        {
            if (Melee) { Attack(); }
            else { Spawn(); }
            AttackTimer = 0;
        }
       
      distancePlayer = Vector3.Distance(transform.position, player.position);
      animator.SetFloat("playerDistance", distancePlayer);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            Melee = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(Melee);
            Melee = false;
        }

    }
    public void Neutral()
    {

    }
    public void Attack ()
    {
        
    }
    public void Spawn()
    {
        animator.SetBool("Spawn", true);
    }
    public void Reset()
    {
        animationModifier = 2;
    }
    public void MarkNewTarget()
    {
        GameObject newTarget = playerList.OrderByDescending(p => Vector2.Distance(transform.position, p.transform.position)).ToList()[0];

        markedTarget = newTarget.transform;
    }

    public void TeleportToMarkedTarget()
    {
        if (markedTarget == null) return;

        transform.position = markedTarget.position;
    }
}
