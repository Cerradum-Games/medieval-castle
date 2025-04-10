using System.ComponentModel;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject skin;
    public float life = 100f;
    public bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            skin.GetComponent<Animator>().Play("Die");
        }
    }

    public void hitTaken(float damage)
    {
        skin.GetComponent<Animator>().Play("Hit");
        life -= damage;
        isDead = life <= 0 ? true : false;
    }
}
