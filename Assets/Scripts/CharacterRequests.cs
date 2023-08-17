using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;

public class CharacterRequests : MonoBehaviour
{
     private static string saveEndpoint = "https://walmart-server.onrender.com/api/users/sendCharacterData";
     private static string loadEndpoint = "https://walmart-server.onrender.com/api/users/getUserData/";
    AdvancedPeopleSystem.CharacterCustomization character;

    private void Start()
    {
        character = GetComponent<AdvancedPeopleSystem.CharacterCustomization>();
        GetCharacter(PlayerPrefs.GetString("userId"), (response) =>
        {
            if (response != null)
            {
                PlayerPrefs.SetString("characterType", response.characterType);
                if (response.characterType == "Male")
                {
                    character.SwitchCharacterSettings(0);
                }
                else if (response.characterType == "Female")
                {
                    character.SwitchCharacterSettings(1);
                }
                if (response.characterData != "")
                {
                    character.ApplyCharacterData(response.characterData);
                }
            }
        });
    }

    public void InitializeCharacter(string userId)
    {
        character = GetComponent<AdvancedPeopleSystem.CharacterCustomization>();
        //Debug.Log(character);
        GetCharacter(userId, (response) =>
        {
            //Debug.Log(response.characterType);
            //Debug.Log(PlayerPrefs.GetString("token"));
            if (response.characterType == "Male")
            {
                character.SwitchCharacterSettings(0);
            }
            else if (response.characterType == "Female")
            {
                character.SwitchCharacterSettings(1);
            }
            character.ApplyCharacterData(response.characterData);
        });
    }

    public static async void SaveCharacter(string data, System.Action<PostResponse> Callback)
    {
        try
        {
            if (data.Length <= 0)
            {
                return;
            }
            //Debug.Log(data);
            WWWForm form = new WWWForm();
            form.AddField("character_data", data);
            UnityWebRequest request = UnityWebRequest.Post(saveEndpoint, form);
            request.SetRequestHeader("authorization", "Bearer " + PlayerPrefs.GetString("token"));
            var handler = request.SendWebRequest();

            while (!handler.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                PostResponse response = JsonUtility.FromJson<PostResponse>(request.downloadHandler.text);
                Callback(response);
            }
            else
            {
                Debug.Log("Error connecting to the server...");
                PlayerPrefs.DeleteKey("token");
                Callback(null);
            }
            request.Dispose();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static async void GetCharacter(string userId, System.Action<GetResponse> Callback)
    {
        try
        {
            UnityWebRequest request = UnityWebRequest.Get(loadEndpoint + userId);
            request.SetRequestHeader("authorization", "Bearer " + PlayerPrefs.GetString("token"));
            var handler = request.SendWebRequest();
            while (!handler.isDone)
            {
                await Task.Yield();
            }

            //Debug.Log(request.result.ToString());

            if (request.result == UnityWebRequest.Result.Success)
            {
                //Debug.Log(request.downloadHandler.text);
                GetResponse response = JsonUtility.FromJson<GetResponse>(request.downloadHandler.text);
                Callback(response);
            }
            else
            {
                Debug.Log("Error connecting to the server: " + request.result);
                PlayerPrefs.DeleteKey("token");
                Callback(null);
            }
            request.Dispose();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }


    [System.Serializable]
    public class PostResponse
    {
        public int code;
        public string msg;
        public string data;
    }

    public class GetResponse
    {
        public string status;
        public string characterData;
        public string characterType;
        public string name;
    }


}
