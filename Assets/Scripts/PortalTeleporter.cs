using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PortalTeleporter : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private GameObject player;
    private GameObject receiver;
    private List<PortalManager.PortalPair> portalList;
    private bool playerIsOverlapping = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainPlayer");
        portalList = GetComponentInParent<PortalManager>().portals;
        portalList.ForEach((pair) =>
        {
            if(GameObject.ReferenceEquals(pair.portal1, gameObject))
            {
                receiver = pair.portal2;
            }
            else if (GameObject.ReferenceEquals(pair.portal2, gameObject))
            {
                receiver = pair.portal1;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("MainPlayer");
        }
        else
        {
            if(playerIsOverlapping)
            {
                Vector3 portalToPlayer = player.transform.position - transform.position;
                float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
                //Player moved across the portal
                    float rotationDifference = -Quaternion.Angle(transform.rotation, receiver.transform.rotation);
                    rotationDifference += 180;
                    player.transform.Rotate(Vector3.up, rotationDifference);
                    Vector3 positionOffset = Quaternion.Euler(0f, rotationDifference, 0f) * portalToPlayer;
                    Vector3 pos = receiver.transform.Find("Spawn").position;
                    player.GetComponent<CharacterController>().enabled = false;
                    player.transform.position = pos;
                    player.GetComponent<CharacterController>().enabled = true;
                    playerIsOverlapping = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MainPlayer"))
        {
            playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainPlayer"))
        {
            playerIsOverlapping = false;
        }
    }
}
