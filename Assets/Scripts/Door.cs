using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen=false;

    [SerializeField]
    private bool isRotatingDoor = true;
    [SerializeField]
    private float Speed = 1f;
    [SerializeField]
    private int directionControl = 1;

    [Header("Rotating Configs")]
    [SerializeField]
    private float RotationAmount = 90f;
    [SerializeField]
    private float ForwardDirection = 0;

    private Vector3 StartRotation;
    private Vector3 Forward;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        // Direction that will be forward
        Forward = transform.forward * directionControl;
    }

    public void Open(Vector3 UserPositon)
    {
        if(!isOpen)
        {
            if(AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
        }

        if(isRotatingDoor)
        {
            float dot=Vector3.Dot(Forward, (UserPositon-transform.position).normalized);
            Debug.Log($"Dot :{dot.ToString("N3")} ");
            AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
        }
    }

    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if(ForwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y - RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));
        }

        isOpen = true;

        float time = 0;

        while(time<1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
        }

        if (isRotatingDoor)
        {
            AnimationCoroutine = StartCoroutine(DoRotationClose());
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation=Quaternion.Euler(StartRotation);

        isOpen = false;

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }



}
