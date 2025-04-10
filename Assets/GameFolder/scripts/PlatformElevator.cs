using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class PlatformElevator : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float waitTime = 0.5f;

    [Header("Options")]
    [SerializeField] private bool startAtPointB = false;
    [SerializeField] private bool moveOnStart = true;
    [SerializeField] private bool smoothMovement = true;

    private Vector3 targetPosition;
    private bool movingToPointB;
    private bool isWaiting = false;

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            //Debug.LogError("Os pontos A e B precisam ser definidos no Inspector!");
            this.enabled = false;
            return;
        }

        if (startAtPointB)
        {
            transform.position = pointB.position;
            targetPosition = pointA.position;
            movingToPointB = false;
        }
        else
        {
            transform.position = pointA.position;
            targetPosition = pointB.position;
            movingToPointB = true;
        }

        if (!moveOnStart)
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        if (isWaiting)
            return;

        if (smoothMovement)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
        }
        else
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    private IEnumerator WaitAtPoint()
    {
        transform.position = targetPosition;
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        movingToPointB = !movingToPointB;
        targetPosition = movingToPointB ? pointB.position : pointA.position;
        isWaiting = false;
    }

    public void StartElevator()
    {
        this.enabled = true;
    }

    public void StopElevator()
    {
        this.enabled = false;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
        }
    }

    // 🔄 Aqui entra o comportamento de carregar o player na plataforma
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
