using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformAttach : MonoBehaviour
{
    private Transform originalParent;

    void Start()
    {
        // Guardar o parent original (geralmente null)
        originalParent = transform.parent;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar se colidiu com uma plataforma com o script PlatformElevator
        if (collision.gameObject.GetComponent<PlatformElevator>() != null)
        {
            // Fazer o jogador filho da plataforma para se mover junto
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Verificar se saiu de uma plataforma com o script PlatformElevator
        if (collision.gameObject.GetComponent<PlatformElevator>() != null)
        {
            // Restaurar o parent original
            transform.SetParent(originalParent);
        }
    }
}