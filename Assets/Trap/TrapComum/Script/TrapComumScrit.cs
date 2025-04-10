using UnityEngine;

public class TrapComumScrit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.transform.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            collision.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 275));
            collision.transform.GetComponent<Character>().life--;
        }
    }
}
