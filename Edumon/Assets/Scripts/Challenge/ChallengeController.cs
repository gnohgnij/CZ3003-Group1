using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class ChallengeController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject buttonPrefab;
    public RectTransform ParentPanel;

    void Start()
    {
        User user = StateManager.user;
        string email = user.email;
        StartCoroutine(GetChallengeRequest("https://cz3003-edumon.herokuapp.com/challenge/email/"+email));
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
                ChallengeResponse challengeResponse = JsonUtility.FromJson<ChallengeResponse>(jsonString);
                Challenge[] challenges = challengeResponse.data;

                int i = 0;
                foreach(Challenge c in challenges)
                {
                    GameObject button = GameObject.Instantiate(buttonPrefab, new Vector3(1, 150 - (i*100), 1), Quaternion.identity) as GameObject;
                    button.transform.SetParent(ParentPanel, false);
                    button.GetComponentInChildren<Text>().text = "Challenge from " + c.from_email;
                    i++;

                    Button btn = button.GetComponent<Button>();
                    btn.onClick.AddListener(() => ButtonClicked(c.question_list));
                }        
            }
        }
    }

    public void ButtonClicked(string[] question_list)
    {
        StateManager.challengeQuestionList = question_list;
        StateManager.challengeQuestionSize = question_list.Length;
        StateManager.challengeQuestionIndex = 0;
        Debug.Log("Loading Challenge with " + StateManager.challengeQuestionSize + " questions");
        SceneManager.LoadScene("MCQSingle");
    }
}

[System.Serializable]
public class ChallengeResponse
{
    public bool success;
    public Challenge[] data;
}
