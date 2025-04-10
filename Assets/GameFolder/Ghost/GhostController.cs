using Unity.Mathematics;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public GameObject enemySkin;
    public Rigidbody2D rb;
    public Transform player;
    public Transform playerSkin;
    public Transform skin;
    public Vector3 playerPos;
    public float speedX;
    public float speedY;
    public float distance;
    public float ySpeed;
    public float playerLook, ghostLook;
    public bool playerIsLooking;
    public float relativeDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        distance = 100f;
        speedX = 1.5f;
        speedY= 4.5f;
    }

    // Update is called once per frame
    void Update()
    {
        playerLook = playerSkin.localScale.x / -math.abs(playerSkin.localScale.x);
        ghostLook = skin.localScale.x / -math.abs(skin.localScale.x);
        playerPos = player.transform.position;
        distance = math.pow((playerPos.x - gameObject.GetComponent<Transform>().transform.position.x),2);
        distance += math.pow((playerPos.y - gameObject.GetComponent<Transform>().transform.position.y),2);
        distance = math.sqrt(distance);
        relativeDistance = transform.position.x - player.position.x;
        speedY = (speedY >= 1f) ? -1f : speedY + 0.0075f;
    }

    void FixedUpdate()
    {
        if (true)
        {
            move();
            playerDirection();
        }
    }

    private void move()
    {
        if (distance <= 12f && !playerIsLooking)
        {
            if (playerPos.x < gameObject.GetComponent<Transform>().transform.position.x)
            {
                if (playerPos.y < gameObject.GetComponent<Transform>().transform.position.y)
                {
                    ySpeed = (speedY < 0) ? speedY * 1.5f : speedY * 0.5f;
                    rb.linearVelocity = new Vector2(-speedX, ySpeed);
                }
                else if (playerPos.y > gameObject.GetComponent<Transform>().transform.position.y)
                {
                    ySpeed = (speedY > 0) ? speedY * 1.5f : speedY * 0.5f;
                    rb.linearVelocity = new Vector2(-speedX, ySpeed);
                }
                skin.localScale = new Vector3(4f,4f,4f);
            }
            else if (playerPos.x > gameObject.GetComponent<Transform>().transform.position.x)
            {
                if (playerPos.y < gameObject.GetComponent<Transform>().transform.position.y)
                {
                    ySpeed = (speedY < 0) ? speedY * 1.5f : speedY * 0.5f;
                    rb.linearVelocity = new Vector2(speedX, ySpeed);
                }
                else if (playerPos.y > gameObject.GetComponent<Transform>().transform.position.y)
                {
                    ySpeed = (speedY > 0) ? speedY * 1.5f : speedY * 0.5f;
                    rb.linearVelocity = new Vector2(speedX, ySpeed);
                }
                skin.localScale = new Vector3(-4f,4f,4f);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void playerDirection()
    {
        if (playerLook == ghostLook && relativeDistance * ghostLook > 0)
        {
            playerIsLooking = false;
        }
        else if (playerLook == ghostLook)
        {
            playerIsLooking = true;
        }
        else
        {
            playerIsLooking = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player.GetComponent<Character>().hitTaken(10f);
        }
    }
}
