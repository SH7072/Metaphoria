using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject CheckoutMenu;

    public IEnumerator Blink()
    {
        CheckoutMenu.SetActive(true);
        yield return new WaitForSeconds(2);
        CheckoutMenu.SetActive(false);
    }

}
