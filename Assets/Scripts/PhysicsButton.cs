using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private float threshold = .1f;
    [SerializeField] private float deadZone = 0.025f;
    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;

    public AudioSource pressedSound;
    public UnityEvent onPressed, onReleased;



    private void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        if (!_isPressed && GetValue()+ threshold >= 1)
        {
            Pressed();
        }
        if (_isPressed && GetValue() - threshold <= 0)
        {
            
            Released();
        }
    }

    public float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;
        if(Mathf.Abs(value) < deadZone)
        {
            value = 0;
        }
        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        _isPressed = true;
        pressedSound.Play();
        onPressed.Invoke();
        Debug.Log("Pressed");
    }
    public void Released()
    {
        _isPressed=false;
        onReleased.Invoke();
        Debug.Log("Released");
    }
}
