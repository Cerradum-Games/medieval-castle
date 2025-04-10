using UnityEngine;

public class BatController : MonoBehaviour
{
    public Transform player;
    public float attackTime;
    void Start()
    {
        attackTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // Ir� desligar o BatController caso o morcego morra
        if (GetComponent<Character>().life <= 0)
        {

            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            this.enabled = false;
        }


        if (Vector2.Distance(transform.position, player.position) > 0.8f)
        {
            attackTime = 0;
            transform.position = Vector2.MoveTowards(transform.position, player.position, 1 * Time.deltaTime);
        }
        else
        {
            attackTime = attackTime + Time.deltaTime;
            if(attackTime >= 1)
            {
                attackTime = 0;
                player.GetComponent<Character>().life--;
            }
        }
    }
}