using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;

[System.Serializable]
public class Login : MonoBehaviour
{
    public GameObject loginCanvas;
    public GameObject SignupCanvas;
     private string loginEndpoint = "https://walmart-server.onrender.com/api/users/login";
     private string signupEndpoint = "https://walmart-server.onrender.com/api/users/signup";

    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private TextMeshProUGUI signupAlertText;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button createButton;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField confirmPasswordInputField;
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TMP_InputField signupEmailInputField;
    [SerializeField] private TMP_InputField signupPasswordInputField;
    public static void Hide(GameObject obj)
    {
        obj.SetActive(false);
    }
    public static void Show(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
    }

    public void Start()
    {
        Hide(loginCanvas);
        Hide(SignupCanvas);
    }

    public void OnLoginClick()
    {
        string username = emailInputField.text;
        string password = passwordInputField.text;
        Hide(SignupCanvas);
        Show(loginCanvas);
        //Debug.Log(password);
        if (username.Length > 0 && password.Length > 0)
        {
            alertText.text = "Signing in...";
            TryLogin(username, password);
        }
    }



    public void OnCreateClick()
    {
        string email = signupEmailInputField.text;
        string password = signupPasswordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;
        string username = userNameInputField.text;
        Hide(loginCanvas);
        Show(SignupCanvas);
        if (email.Length > 0 && password.Length > 0 && confirmPassword.Length > 0 && username.Length > 0)
        {
            signupAlertText.text = "Creating account...";
            TryCreate(email, password, username, confirmPassword);
        }
    }

    private async void TryLogin(string username, string password)
    {
        try
        {
            WWWForm form = new WWWForm();
            form.AddField("email", username);
            form.AddField("password", password);
            if (username.Length <= 0 && password.Length <= 0)
            {
                alertText.text = "Enter Email and Password";
                return;
            }
            UnityWebRequest request = UnityWebRequest.Post(loginEndpoint, form);
            var handler = request.SendWebRequest();

            while (!handler.isDone)
            {
                await Task.Yield();
            }
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Debug.Log(request.downloadHandler.text);
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                Debug.Log(response.user._id);
                PlayerPrefs.SetString("token", response.token);
                PlayerPrefs.SetString("userId", response.user._id);
                PlayerPrefs.SetString("username", response.user.name);
                SceneManager.LoadScene(1);
            }
            else
            {
                PlayerPrefs.DeleteKey("token");
                PlayerPrefs.DeleteKey("userId");
                PlayerPrefs.DeleteKey("username");
                alertText.text = "Error Connecting to Server...";
                ActivateButtons(true);
            }
            request.Dispose();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            PlayerPrefs.DeleteKey("token");
            PlayerPrefs.DeleteKey("userId");
            PlayerPrefs.DeleteKey("username");
            alertText.text = ex.Message;
            ActivateButtons(true);
        }
    }
    private async void TryCreate(string email, string password, string username, string confirmPassword)
    {
        try
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("password", password);
            form.AddField("name", username);
            form.AddField("confirmPassword", confirmPassword);
            UnityWebRequest request = UnityWebRequest.Post(signupEndpoint, form);
            // request.SetRequestHeader("Content-Type", "application/json");
            // Debug.Log(request.body);
            var handler = request.SendWebRequest();
            while (!handler.isDone)
            {
                await Task.Yield();
            }
            Debug.Log(request.result);
            if (request.result == UnityWebRequest.Result.Success)
            {
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                PlayerPrefs.SetString("token", response.token);
                PlayerPrefs.SetString("userId", response.user._id);
                PlayerPrefs.SetString("username", response.user.name);
                SceneManager.LoadScene(1);
            }
            else
            {
                signupAlertText.text = "Error connecting to the server...";
                PlayerPrefs.DeleteKey("token");
                PlayerPrefs.DeleteKey("userId");
                PlayerPrefs.DeleteKey("username");
            }
            request.Dispose();
            ActivateButtons(true);
        }
        catch (Exception ex)
        {
            PlayerPrefs.DeleteKey("token");
            PlayerPrefs.DeleteKey("userId");
            PlayerPrefs.DeleteKey("username");
            signupAlertText.text = ex.Message;
            ActivateButtons(true);
        }
    }

    private void ActivateButtons(bool toggle)
    {
        loginButton.interactable = toggle;
        createButton.interactable = toggle;
    }
}
