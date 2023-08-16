using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;
using System;

public class CartItem : MonoBehaviour
{
    public string id;
    public TextMeshProUGUI name, price, quantity;
    public RawImage img;
    [SerializeField]
    public Button del;

    private string decCartEndPoint = "http://localhost:3001/api/cart/decQuantity/";
    private string delFromCartEndPoint = "http://localhost:3001/api/cart/removeFromCart/";
    // Start is called before the first frame update
    void Start()
    {

    }

    public void setQuantity(int quantity)
    {
        this.quantity.text = "Quantity : " + quantity.ToString();
    }

    public int getQuantity()
    {
        return int.Parse(this.quantity.text.Remove(0, 11));
    }

    public async void delItem()
    {
        Debug.Log(this.quantity.text);
        if (getQuantity() == 1)
        {
            Destroy(gameObject);
            if (id != "")
            {
                try
                {
                    WWWForm form = new WWWForm();
                    UnityWebRequest request = UnityWebRequest.Post(delFromCartEndPoint + id, form);
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
                        Debug.Log("SUCCESSFULLY Deleted FROM CART");
                    }
                    request.Dispose();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
        else
        {
            setQuantity(getQuantity() - 1);
            if (id != "")
            {
                try
                {
                    WWWForm form = new WWWForm();
                    UnityWebRequest request = UnityWebRequest.Post(decCartEndPoint + id, form);
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
                        Debug.Log("SUCCESSFULLY DECREASED FROM CART");
                    }
                    request.Dispose();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
    }

    public async void setItem(ICartItem item)
    {
        this.id = item._id;
        this.name.text = item.name;
        this.price.text = "$" + item.price.Remove(0, 1);
        setQuantity(item.quantity);
        await DownloadImage(item.img);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async Task DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        var handler = request.SendWebRequest();
        while (!handler.isDone)
        {
            await Task.Yield();
        }
        Debug.Log(request.downloadHandler);
        if (request.result != UnityWebRequest.Result.Success)
            Debug.Log(request.error);
        else
            this.img.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        request.Dispose();
    }
}
