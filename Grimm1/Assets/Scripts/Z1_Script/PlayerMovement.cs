using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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

    private void start()
    {
        life = healthController.hearts.Length;
    }
    // Update is called once per frame
    void Update()
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
        //move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //life -= d;
            //Destroy(hearts[life].gameObject);
            //SceneManager.LoadScene("MainMenu");
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

    //public void TakeDamage(int d)
    //{
    //    life = life - d;


    //    if (life < 1)
    //    {
    //        SceneManager.LoadScene("MainMenu");
    //    }
    //}

}
