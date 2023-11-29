using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    public CharacterController2D controller;
    public BoxCollider2D boxCollider;
    public Animator animator;
    public float runSpeed = 40f;
    public bool crouch = false;

    float horizontalMove = 0f;
    bool jump = false;
    bool canBeDamaged = true;

    // Weapon-related variables
    public Transform firePoint, firePointCrouch;
    private GameObject bulletPrefab;
    public GameObject[] bullet;
    public GameObject[] gun;

    public float fireRate = 10f;
    private bool canFire = true;
    private bool timerStarted = false;

    // Buff system variables
    private bool isBuffActive = false;

    public float buffDuration = 10f;
    private float timeLeftForBuff;

    //playerindicator
    public GameObject localIndicatorPrefab;
    public Transform playerIndicator;

    private GameObject localIndicator; // Declare localIndicator at the class level

    public float life;

    [SerializeField]
    private GameObject hpbar;

    PhotonView view;

    private void Start()
    {
        bulletPrefab = bullet[0];
        view = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            // Instantiate the local indicator only for the local player
            if (playerIndicator != null)
            {
                localIndicator = Instantiate(localIndicatorPrefab, playerIndicator);
            }
            else
            {
                Debug.LogError("PlayerIndicator transform is not set. Please assign it in the Inspector.");
            }
        }
        life = 5;
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

        hpbar.transform.localScale = new Vector2(0.6302553f * (life / 5), 0.03f);
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
                TakeDamage(1);
                IFrame();
                Debug.Log("llllllllll");
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

    [PunRPC]
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (photonView.IsMine)
        {
            if (target.tag == "Weapon")
            {
                // Check if the buff is not already active
                if (!isBuffActive)
                {
                    if (target.name == gun[0].name)
                    {
                        ApplyBuff(bullet[0], 10f);
                    }
                    else if (target.name == gun[1].name)
                    {
                        ApplyBuff(bullet[1], 5f);
                    }
                    else if (target.name == gun[2].name)
                    {
                        ApplyBuff(bullet[2], 25f);
                    }
                    else if (target.name == gun[3].name)
                    {
                        ApplyBuff(bullet[3], 20f);
                    }
                    else if (target.name == gun[4].name)
                    {
                        ApplyBuff(bullet[4], 0.5f);
                    }

                    Debug.Log("Gun pickup - upgrade.");
                    Destroy(target.gameObject);
                }
            }
        }
    }

    public void ApplyBuff(GameObject newBulletPrefab, float newFireRate)
    {
        // Apply the buff
        bulletPrefab = newBulletPrefab;
        fireRate = newFireRate;

        // Start the buff timer
        isBuffActive = true;
        timeLeftForBuff = buffDuration;

        // Start the timer coroutine
        if (!timerStarted)
        {
            StartCoroutine(BuffTimer());
            timerStarted = true;
        }
    }

    private IEnumerator BuffTimer()
    {
        while (timeLeftForBuff > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeftForBuff--;

            // Optionally, you can update UI to display the remaining time
            // UIManager.UpdateBuffTimer(timeLeftForBuff);
        }

        // Buff timer is finished, reset to default bullet
        bulletPrefab = bullet[0];
        fireRate = 10f;
        isBuffActive = false;

        // Optionally, you can update UI to indicate that the buff has ended
        // UIManager.DisplayBuffEnded();

        timerStarted = false; // Reset the timer flag
    }

    void OnDestroy()
    {
        // Destroy the local indicator only for the local player when the player is destroyed
        if (photonView.IsMine && localIndicator != null)
        {
            Destroy(localIndicator);
        }
    }

    public void TakeDamage(int d)
    {
        life -= d;
        if (life <= 0)
        {
            life = 0;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(life);
        }
        else
        {
            // Network player, receive data
            life = (float)stream.ReceiveNext();
        }
    }
}
