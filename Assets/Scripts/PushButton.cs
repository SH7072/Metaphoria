using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody button;
    public float multiplier;
    public bool isOnButton;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isOnButton)
        {
            button.AddForce(new Vector3(0, -1, 0) * multiplier);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MainPlayer"))
        {
            Debug.Log("player on button");
            isOnButton = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainPlayer"))
        {
            Debug.Log("player left button");
            isOnButton = false;
        }
    }
}
