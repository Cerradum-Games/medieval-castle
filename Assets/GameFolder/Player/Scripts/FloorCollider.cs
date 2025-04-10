using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : MonoBehaviour
{
    public bool canJump;
    public bool canCrouch;
    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Floor"))
        {
            canJump = true;
            canCrouch = true;
        }
    }


}