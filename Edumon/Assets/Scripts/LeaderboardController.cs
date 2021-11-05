using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.SceneManagement;

public class LeaderboardController : MonoBehaviour
{
    private const string attemptURL = "https://cz3003-edumon.herokuapp.com/attempt/email/";
    private const string accountURL = "https://cz3003-edumon.herokuapp.com/account";
    public Text emailAddresses;
    public Text scores;
    public Dictionary<string, int> users = new Dictionary<string, int>();
    // public Dictionary<string, int>[] users = new Dictionary<string, int>[] {
    //     new Dictionary<string, int>(),
    //     new Dictionary<string, int>(),
    //     new Dictionary<string, int>(),
    //     new Dictionary<string, int>(),
    //     new Dictionary<string, int>()
    // };
    public int counter = 0;

    public void Btn_Return_To_Homepage_Clicked()
    {
        SceneManager.LoadScene("StudentHome");
    }

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
                Account[] accounts = accountResponse.data;

                //Get only the users with type == student
                List<Account> students = new List<Account>();
                foreach(Account a in accounts)
                {
                    if(a.type == "student")
                    {
                        students.Add(a);
                    }
                }

                //get score from each student
                foreach(Account a in students)
                {
                    yield return StartCoroutine(GetAttemptRequest(attemptURL + a.email));
                }

            }

            // yield return new WaitForSeconds(10);
            var val = from ele in users orderby ele.Value descending select ele;
            foreach (var group in val){
                Debug.Log("Key: " + group.Key + " Value: " + group.Value);
                emailAddresses.text += "\n" + group.Key + "\n";
                scores.text += "\n" + group.Value + "\n";
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
                string jsonString = webRequest.downloadHandler.text;
                AttemptEmailResponse attemptEmailResponse = JsonUtility.FromJson<AttemptEmailResponse>(jsonString);
                Attempt[] attempt = attemptEmailResponse.data;
                
                //Calculation of total points
                int totalPoints = 0;

                //key - question_set, value - score
                Dictionary<string, int> map = new Dictionary<string, int>();
                foreach(Attempt a in attempt)
                {   
                    //if student has multiple attempts on the same qn set, 
                    //take the attempt with the highest score
                    if(!map.ContainsKey(a.question_set))
                        map.Add(a.question_set, a.score);
                    else
                    {
                        int initialScore = map[a.question_set];
                        if(initialScore < a.score)
                        {
                            map.Remove(a.question_set);
                            map.Add(a.question_set, a.score);
                        }
                    }
                }

                foreach(int score in map.Values)
                {
                    totalPoints += score;
                }
                // Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                // emailAddresses.text += "\n" + pages[page] + "\n";
                // if (webRequest.downloadHandler.text.IndexOf("score") == -1){
                //     scores.text += "\n" + "NA" + "\n";
                // }
                // else{
                //     scores.text += "\n" + totalPoints + "\n";
                //     // scores.text += "\n" + webRequest.downloadHandler.text[webRequest.downloadHandler.text.IndexOf("score") + 7] + "\n";
                // }
                // scores.text += "\n" + Regex.Replace(webRequest.downloadHandler.text, '{.*?{', string.Empty) + "\n";
                
                // += "\n" + pages[page] + ":\nReceived: " + webRequest.downloadHandler.text;
                // var user = new Dictionary <string, int> {
                //     pages[page], totalPoints
                // };
                users.Add(pages[page], totalPoints);
                // Debug.Log(pages[page] + totalPoints + counter);
                // Debug.Log(users[pages[page]]);
                counter++;
                // Debug.Log("Does this work?" + totalPoints);

            }
        }
    }
}

[System.Serializable]
public class AccountResponse
{
    public bool success;
    public Account[] data;
}

[System.Serializable]
public class Account
{
    public string uid;
    public string email;
    public string[] unlocked_map;
    public string username;
    public string studentid;
    public string type;
}

[System.Serializable]
public class AttemptEmailResponse
{
    public bool success;
    public Attempt[] data;
}

[System.Serializable]
public class Attempt
{
    public string attempt_id;
    public string question_set;
    public string question_group;
    public string user_email;
    public int score;
    public object user_answers;
    public string completion_status;
}
