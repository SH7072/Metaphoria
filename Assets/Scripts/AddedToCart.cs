using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddedToCart : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject AddedToCartMenu;

    public IEnumerator Blink()
    {
        AddedToCartMenu.SetActive(true);
        yield return new WaitForSeconds(2);
        AddedToCartMenu.SetActive(false);
    }
}
