using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            if(player.GetComponent<PlayerController>().comboNumber == 1)
            {
                collision.GetComponent<Character>().life--;
            }
            if(player.GetComponent<PlayerController>().comboNumber == 2)
            {
                collision.GetComponent<Character>().life -= 2;
            }
        }
    }
}