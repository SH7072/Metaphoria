using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (collision.collider.CompareTag("CollisionObjects"))
        {
            Debug.Log(collision.collider);
        }    
    }

}
