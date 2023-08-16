using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;

public class CartMenu : MonoBehaviour
{
    public static bool MenuOpen = false;

    public GameObject cartMenu;
    [SerializeField]
    public GameObject cartMenuContent;
    public GameObject cartItemPrefab;
    public List<ICartItem> cartItems;

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }


    public void OpenMenu()
    {
        cartMenu.SetActive(true);
        MenuOpen = true;
        SetCursorState(false);
    }

    public void CloseMenu()
    {
        cartMenu.SetActive(false);
        MenuOpen = false;
        SetCursorState(true);
    }

    public void setItems(ICartItem[] items)
    {
        this.clearAllItems();
        foreach (ICartItem item in items)
        {
            Debug.Log(item.ToString());

            GameObject temp = Instantiate(cartItemPrefab);
            temp.transform.SetParent(cartMenuContent.transform);
            temp.transform.localScale = Vector3.one;
            CartItem ct = temp.GetComponent<CartItem>();
            ct.setItem(item);
            cartItems.Add(item);
        }

    }

    public void clearItems(string flag)
    {
        int num=-1;
        this.cartItems = this.cartItems.Where(item =>
        {
            num++;
            if (item.link.Contains(flag))
            {
                Destroy(cartMenuContent.transform.GetChild(num).gameObject);
                return false;
            }
            return true;
        }).ToList();
    }

    public void clearAllItems()
    {
        foreach (Transform tr in cartMenuContent.transform)
        {
            Destroy(tr.gameObject);
        }
        cartItems.Clear();
    }
}

[System.Serializable]
public class CartResponse
{
    public string status;
    public ICartItem[] data;
}

[System.Serializable]
public class ICartItem
{
    public string _id;
    public string name;
    public string link;
    public string img;
    public string price;
    public int quantity;
    public int __v;
}