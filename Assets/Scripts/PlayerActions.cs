using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Threading.Tasks;
using agora_gaming_rtc;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using AdvancedPeopleSystem;
using System.Collections;
using System.Linq;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif




public class PlayerActions : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshPro UseText;
    [SerializeField]
    private TextMeshProUGUI InfoText;
    [SerializeField]
    private GameObject InfoTextHolder;
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private LayerMask UseLayers;
    [SerializeField]
    public GameObject playerMesh;
    [SerializeField]
    public GameObject voiceImg;

    private float MaxUseDistance = 5f;

    private IRtcEngine rtcEngine;

    public Canvas menuCanvas;

    public CartMenu cartMenu;

    public CheckoutCanvas checkout;

    public AddedToCart adc;

    public PauseMenu pauseMenu;

    private bool tryingOn = false;
    private bool isMuted = true;
    private CharacterElementType tryingOnType;
    private int tryingOnTypeInitialValue;
    private CharacterCustomization characterCustomization;
    private string characterType = "Female";



    private string addToCartEndpoint = "https://walmart-server.onrender.com/api/cart/addToCart/";
    private string fetchCartEndpoint = "https://walmart-server.onrender.com/api/cart/getCart";

    private void Awake()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        //_voiceNetwork = PhotonVoiceNetwork.Instance;
        //_voiceNetwork.PrimaryRecorder.TransmitEnabled = false;

        menuCanvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();

        cartMenu = menuCanvas.GetComponent<CartMenu>();

        pauseMenu = menuCanvas.GetComponent<PauseMenu>();

        checkout = menuCanvas.GetComponent<CheckoutCanvas>();

        adc = menuCanvas.GetComponent<AddedToCart>();

        InfoText = menuCanvas.GetComponentInChildren<TextMeshProUGUI>();

        characterCustomization = playerMesh.GetComponent<CharacterCustomization>();

        characterType = PlayerPrefs.GetString("characterType");

        rtcEngine = VoiceChatManager.Instance.GetRtcEngine();
    }

    public void Start()
    {
        rtcEngine.MuteLocalAudioStream(true);
    }


    public void OnUse()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.TryGetComponent<Door>(out Door door))
            {
                if (door.isOpen)
                {
                    door.Close();
                }
                else
                {
                    door.Open(transform.position);
                }
            }
        }
    }

    public void OnBuy()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.TryGetComponent<ShoppingItem>(out ShoppingItem shoppingItem))
            {
                StartCoroutine(adc.Blink());
                // CODE TO BUY THE PRODUCT
                addToCart(shoppingItem.id);
            }
        }
    }

    public void OnCheckout()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.TryGetComponent<CheckoutManager>(out CheckoutManager cm))
            {
                cm.checkout();
                cartMenu.clearItems(cm.flag);
                StartCoroutine(checkout.Blink());
            }
        }
    }

    public void OnVoiceToggle(InputValue value)
    {
        Debug.Log(value.isPressed);
        if (value.isPressed)
        {
            rtcEngine.MuteLocalAudioStream(false);
            voiceImg.SetActive(true);
        }
        else
        {
            rtcEngine.MuteLocalAudioStream(true);
            voiceImg.SetActive(false);
        }
    }

    public void OnTryOn(InputValue value)
    {
        if (tryingOn)
        {
            characterCustomization.SetElementByIndex(tryingOnType, tryingOnTypeInitialValue);
            tryingOn = false;
        }
        else if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.TryGetComponent<ShoppingItem>(out ShoppingItem shoppingItem))
            {
                if (hit.collider.TryGetComponent<TryOnItem>(out TryOnItem tryOnItem) && tryOnItem.gender == characterType)
                {
                    tryingOn = true;
                    tryingOnType = tryOnItem.type;
                    tryingOnTypeInitialValue = characterCustomization.characterSelectedElements.GetSelectedIndex(tryingOnType);
                    playerMesh.GetComponent<CharacterCustomization>().SetElementByIndex(tryOnItem.type, tryOnItem.elementIndex);
                }
            }
        }


    }
    public void OnCart()
    {
        // CODE TO DISPLAY THE CART

        Debug.Log(PauseMenu.GameIsPaused);
        if (!PauseMenu.GameIsPaused)
        {
            if (!CartMenu.MenuOpen)
            {
                fetchCart();
                cartMenu.OpenMenu();
            }
            else
            {
                cartMenu.CloseMenu();
            }
        }
    }

    public void OnCloseMenu()
    {

        if (!CartMenu.MenuOpen)
        {
            if (PauseMenu.GameIsPaused)
            {
                pauseMenu.Resume();
            }
            else
            {
                pauseMenu.Pause();
            }
        }
        else
        {
            cartMenu.CloseMenu();
        }
    }


    public void Update()
    {
        if (photonView.IsMine)
        {
            if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
            {

                if (hit.collider.TryGetComponent<Door>(out Door door))
                {

                    if (door.isOpen)
                    {
                        InfoText.text = "Close \"E\"";

                    }
                    else
                    {
                        InfoText.text = "Open \"E\"";
                    }
                    // UseText.transform.position = hit.point - (hit.point - Camera.position).normalized * 0.1f;
                    // UseText.transform.rotation = Quaternion.LookRotation((hit.point - Camera.position).normalized);
                }
                else if (hit.collider.TryGetComponent<ShoppingItem>(out ShoppingItem shoppingItem))
                {
                    if (hit.collider.TryGetComponent<TryOnItem>(out TryOnItem tryOnItem) && tryOnItem.gender == characterType)
                    {
                        InfoText.text = "Select \"B\" to buy " + shoppingItem.name + " or hold \"T\" to try on";
                    }
                    else
                    {
                        InfoText.text = "Select \"B\" to buy " + shoppingItem.name;
                    }
                }
                else if (hit.collider.TryGetComponent<CheckoutManager>(out CheckoutManager cm))
                {
                    InfoText.text = "Click \"C\" to checkout this store";
                }
            }
            else
            {
                InfoText.text = "";
            }
        }


    }


    // ADD TO CART
    public async void addToCart(string _id)
    {
        if (_id != "")
        {
            try
            {
                UnityWebRequest request = UnityWebRequest.Get(addToCartEndpoint + _id);
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
                    Debug.Log("SUCCESSFULLY ADDED TO CART");
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    // FETCH CART
    public async void fetchCart()
    {
        try
        {
            UnityWebRequest request = UnityWebRequest.Get(fetchCartEndpoint);
            request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
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
                Debug.Log(request.downloadHandler.text);
                CartResponse response = JsonUtility.FromJson<CartResponse>(request.downloadHandler.text);
                if (response.data.Length == 0)
                {
                    Debug.Log("empty");
                    cartMenu.clearAllItems();
                }
                else
                {
                    Debug.Log(response.data[0].name);
                    cartMenu.setItems(response.data);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}

