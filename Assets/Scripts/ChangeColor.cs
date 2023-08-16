using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(PhotonView))]

public class ChangeColor : MonoBehaviour
{
    private PhotonView photonView;
    private List<Vector3> colors = new List<Vector3>()
    {
            new Vector3(113, 217, 217),
            new Vector3(8, 166, 121),
            new Vector3(74, 217, 35),
            new Vector3(242, 213, 68),
            new Vector3(191, 15, 15),
            new Vector3(3, 76, 140),
            new Vector3(112, 115, 93),
            new Vector3(242, 183, 5),
            new Vector3(242, 159, 5),
            new Vector3(242, 135, 5)
    };

    public void ChangeColors()
    {
        this.photonView = this.GetComponent<PhotonView>();
        if (this.photonView.IsMine)
        {
            int random = Random.Range(0, 10);
            Renderer renderer = this.GetComponent<Renderer>();
            Debug.Log(colors[random]);
            renderer.material.color = new Color(colors[random].x / 255, colors[random].y / 255, colors[random].z / 255);
            //renderer.material.SetColor("_Color", new Color(colors[random].x, colors[random].y, colors[random].z));
        }
    }
}
