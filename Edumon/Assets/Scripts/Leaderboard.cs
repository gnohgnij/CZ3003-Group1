using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Leaderboard : MonoBehaviour
{
    private const string URL = "https://cz3003-edumon.herokuapp.com/attempt";

    public void GenerateRequest()
    {
        StartCoroutine(ProcessRequest(URL));
    }

    private IEnumerator ProcessRequest(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}
