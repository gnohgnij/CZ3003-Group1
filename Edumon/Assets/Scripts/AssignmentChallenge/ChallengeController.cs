using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class ChallengeController : MonoBehaviour
{
    // Start is called before the first frame update

    public string challengeAPI = "https://cz3003-edumon.herokuapp.com/challenge/email";

    public User user = StateManager.user;
    void Start()
    {
        string email = user.email;
        StartCoroutine(GetChallengeRequest(challengeAPI + email));
    }

    IEnumerator GetChallengeRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if(webRequest.result == UnityWebRequest.Result.ConnectionError)
                Debug.Log(pages[page] + ": Error: " + webRequest.error);

            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                string jsonString = webRequest.downloadHandler.text;        
            }
        }
    }
}
