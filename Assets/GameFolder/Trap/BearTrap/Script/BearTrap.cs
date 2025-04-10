using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    Transform player;
    public Transform skin;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {

            skin.GetComponent<Animator>().Play("stuck", -1);
            collision.transform.position = transform.position;
            collision.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            collision.GetComponent<PlayerController>().skin.GetComponent<Animator>().SetBool("PlayerRun", false);      
            collision.GetComponent<PlayerController>().skin.GetComponent<Animator>().Play("PlayerIdle", -1);
            GetComponent<BoxCollider2D>().enabled = false;
            player = collision.transform;
            collision.GetComponent<PlayerController>().enabled = false;
            Invoke("ReleasePlayer", 2);
        }
    }

    void ReleasePlayer()
    {
        player.GetComponent<PlayerController>().enabled = true;
    }





}
