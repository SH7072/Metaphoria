using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

public class CheckoutManager : MonoBehaviour
{
    [SerializeField]
    public string flag;
    private string checkoutEndPoint = "http://localhost:3001/api/cart/cart_checkout/";

    public void checkout()
    {
        Debug.Log(this.flag);
        _checkout();
    }

    public async void _checkout()
    {
        try
        {
            WWWForm form = new WWWForm();
            form.AddField("flag", flag);
            UnityWebRequest request = UnityWebRequest.Post(checkoutEndPoint , form);
            request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
            Debug.Log(request.ToString());
            var handler = request.SendWebRequest();

            while (!handler.isDone)
            {
                await Task.Yield();
            }

            Debug.Log(request.result);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("SUCCESSFULLY CHECKED OUT");
            }
            request.Dispose();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
