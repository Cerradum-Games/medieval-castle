using UnityEngine;

public class BatTrigger : MonoBehaviour
{

    public Transform[] bat;
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
            foreach(Transform obj in bat)
            {
                obj.GetComponent<BatController>().enabled = true;
            }
        }
    }
}