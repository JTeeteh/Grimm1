using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public CharacterController2D controller;
    public HealthController healthController;
    public BoxCollider2D boxCollider;
    public Animator animator;
    public float runSpeed = 40f;
    public bool crouch = false;

    float horizontalMove = 0f;
    bool jump = false;
    bool canBeDamaged = true;

    public static int life = 5;

    // Weapon-related variables
    public Transform firePoint, firePointCrouch;
    private GameObject bulletPrefab;
    public GameObject[] bullet;
    public GameObject[] gun;

    private float nextTimeOfFire = 0;
    public float fireRate;
    private bool canFire;
    private bool timerStarted;

    PhotonView view;

    private void Start()
    {
        fireRate = 10f;
        bulletPrefab = bullet[0];
        canFire = true;
        timerStarted = false;
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                animator.SetBool("IsJumping", true);
            }

            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                crouch = false;
            }

            onCrouching(crouch);

            if (Input.GetButton("Fire1") && canFire)
            {
                StartCoroutine(Fire());
            }
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void onCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (canBeDamaged)
            {
                healthController.TakeDamage(1);
                IFrame();
            }
        }
    }

    void IFrame()
    {
        boxCollider.enabled = false;
        canBeDamaged = false;
        StartCoroutine(EyeFrameTimer());
    }

    IEnumerator EyeFrameTimer()
    {
        yield return new WaitForSeconds(1);
        boxCollider.enabled = true;
        canBeDamaged = true;
    }

    void Shoot(bool isCrouching)
    {
        if (photonView.IsMine)
        {
            // Spawn the local player's bullet only for themselves
            GameObject bullet = null;
            if (!isCrouching)
            {
                bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
            }
            else
            {
                bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePointCrouch.position, firePointCrouch.rotation);
            }

            // Disable the renderer for the local player's bullets on the network
            if (bullet != null && !bullet.GetPhotonView().IsMine)
            {
                Renderer bulletRenderer = bullet.GetComponent<Renderer>();
                if (bulletRenderer != null)
                {
                    bulletRenderer.enabled = false;
                }
            }

            photonView.RPC("ShootRPC", RpcTarget.Others, isCrouching);
        }
    }

    [PunRPC]
    void ShootRPC(bool isCrouching)
    {
        // Spawn the remote player's bullet for all players
        if (photonView.IsMine)
        {
            GameObject bullet = null;
            if (!isCrouching)
            {
                bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
            }
            else
            {
                bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePointCrouch.position, firePointCrouch.rotation);
            }
        }
    }

    IEnumerator Fire()
    {
        canFire = false;
        Shoot(crouch);
        yield return new WaitForSeconds(1f / fireRate);
        canFire = true;
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Weapon")
        {
            if (target.name == gun[0].name)
            {
                bulletPrefab = bullet[0];
                fireRate = 10f;
            }
            else if (target.name == gun[1].name)
            {
                bulletPrefab = bullet[1];
                fireRate = 5f;
            }
            else if (target.name == gun[2].name)
            {
                bulletPrefab = bullet[2];
                fireRate = 25f;
            }
            else if (target.name == gun[3].name)
            {
                bulletPrefab = bullet[3];
                fireRate = 20f;
            }
            else if (target.name == gun[4].name)
            {
                bulletPrefab = bullet[4];
                fireRate = 0.5f;
            }

            Debug.Log("Gun pickup - upgrade.");
            Destroy(target.gameObject);
        }
    }
}