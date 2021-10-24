using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LeaderboardController : MonoBehaviour
{
    private const string attemptURL = "https://cz3003-edumon.herokuapp.com/attempt/email";
    private const string accountURL = "https://cz3003-edumon.herokuapp.com/account";

    void Start()
    {
        StartCoroutine(GetAccountRequest(accountURL));
    }

    IEnumerator GetAccountRequest(string uri)
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
                AccountResponse accountResponse = JsonUtility.FromJson<AccountResponse>(jsonString);

                //List of users
                User[] userData = accountResponse.data;
                Debug.Log(userData[0].username);

                //Get only the users with type == student
                List<User> students = new List<User>();
                foreach(User u in userData)
                {
                    if(u.type == "student")
                    {
                        students.Add(u);
                    }
                }

                Debug.Log("students = " + students[0].username);

                //get score from each student
                foreach(User u in students)
                {
                    StartCoroutine(GetAttemptRequest(attemptURL + u.email));
                }
            }
        }
    }

    IEnumerator GetAttemptRequest(string uri)
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
            }
        }
    }
}

[System.Serializable]
public class AccountResponse
{
    public bool success;
    public User[] data;
}
