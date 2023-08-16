using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoginResponse
{
    public string token;
    public string status;
    public GameAccount user;
}
