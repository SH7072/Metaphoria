using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [Serializable]
    public struct PortalPair
    {
        public GameObject portal1;
        public GameObject portal2;
    }

    [SerializeField]
    public List<PortalPair> portals;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

