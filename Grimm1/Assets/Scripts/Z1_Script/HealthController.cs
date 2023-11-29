using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    public float life;
    private bool dead;
    [SerializeField]
    private GameObject hpbar;

    private void Start()
    {
        // If you're not using an array, you might initialize life to a default value
        life = 5; // You can adjust this value based on your needs
    }

    void Update()
    {
        hpbar.transform.localScale = new Vector2(0.6302553f * (life / 5), 0.03f);

    }

    public void TakeDamage(int d)
    {
        life -= d;
        if (life <= 0)
        {
            life = 0;
        }
    }
}
