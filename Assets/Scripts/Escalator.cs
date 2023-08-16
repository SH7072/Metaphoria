using UnityEngine;

public class Escalator : MonoBehaviour
{
    public Rigidbody body;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Escalator"))
        {
            Debug.Log("On esc");
            body.AddForce(0, 0, 100 * Time.deltaTime);
        }
    }
}
