using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;

[System.Serializable]
public class ShoppingItem : MonoBehaviour
{
    [SerializeField]
    public string id;

    public string name;

    private string price;

    private string img;

    private string link;

    private string fetchProductEndpoint = "https://walmart-server.onrender.com/api/products/getProduct/";

    public void Start()
    {
        fetchData();
    }

    public async void fetchData()
    {
        if (this.id != "")
        {
            try
            {
                UnityWebRequest request = UnityWebRequest.Get(fetchProductEndpoint + this.id);
                request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
                var handler = request.SendWebRequest();

                while (!handler.isDone)
                {
                    await Task.Yield();
                }


                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    ProductResponse response = JsonUtility.FromJson<ProductResponse>(request.downloadHandler.text);
                    this.name = response.name;
                    this.price = response.price;
                    this.img = response.img;
                    this.link = response.link;
                }
                request.Dispose();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}

[System.Serializable]
public class ProductResponse
{
    public string status;
    public string name;
    public string price;
    public string img;
    public string link;
}