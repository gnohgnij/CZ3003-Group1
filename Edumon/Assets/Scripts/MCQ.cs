using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MCQ : MonoBehaviour
{
    private const string URL = "https://cz3003-edumon.herokuapp.com/question";
    public Button submitButton;

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
                Debug.Log(UnityWebRequest.Get(url+"/SM3AlYWKEDIrOo02UJtl"));
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        submitButton.onClick.AddListener(TaskOnClick);
        GenerateRequest();
    }

    // Update is called once per frame
    void TaskOnClick()
    {
        //Output this to console when submitButton is clicked
        GenerateRequest();
    }
}
