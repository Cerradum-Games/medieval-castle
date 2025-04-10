using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeperController : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public bool goRight;

    public  Transform skin;
    public Transform keeperRange;



    void Start()
    {
        
    }


    void Update()
    {

        if(GetComponent<Character>().life <= 0)
        {
            keeperRange.GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            this.enabled = false;
        }

        if(skin.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("KeeperAttack"))
        {
            return;
        }



        if(goRight == true)
        {

            skin.localScale = new Vector3(-1, 1, 1);

            if(Vector2.Distance(transform.position, PointB.position) < 0.1f)
            {
                goRight = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, PointB.position, 0.2f * Time.deltaTime);
        }
        else
        {   
            skin.localScale = new Vector3(1, 1, 1);
            if(Vector2.Distance(transform.position, PointA.position) < 0.1f)
            {
                goRight = true;
            }            
            transform.position = Vector2.MoveTowards(transform.position, PointA.position, 0.2f * Time.deltaTime);
        }
        
       
    }
}