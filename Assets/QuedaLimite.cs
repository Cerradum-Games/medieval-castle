using UnityEngine;

public class QuedaLimite : MonoBehaviour
{
    private BoxCollider2D bxCollision;
    public Transform player;

    void Start()
    {
        // Não é necessário chamar GetComponent aqui, pois já é referenciado no Inspector
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.position = new Vector2(-31f, 0f); // Define a posição desejada
        }
    }
}
