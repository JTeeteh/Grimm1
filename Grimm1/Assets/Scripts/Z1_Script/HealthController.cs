using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    public GameObject[] hearts;
    private int life;
    private bool dead;
    // Start is called before the first frame update

    private void Start()
    {
        life = hearts.Length;
    }
    // Update is called once per frame
    void Update()
    {
     if (dead == true)
        {
            Debug.Log("GameOver");
        }
    }
    public void TakeDamage(int d)
    {
        life -= d;
        Destroy(hearts[life].gameObject);
        if(life == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
