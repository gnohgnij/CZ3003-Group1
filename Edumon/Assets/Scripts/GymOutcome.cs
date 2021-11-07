using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GymOutcome : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public Button DoneButton;
    private static string URL = StateManager.apiUrl;
    private static string GYM1 = "DeDnjKAqlzb5cKOJrIzk";
    private static string GYM2 = "9vF4uGW4b7oop1EUK1Sm";
    private static string GYM3 = "IFBMLqL6Mb16fagAChaO";
    private static string GYM4 = "bcO5PTXYNsHaREccZovg";
    private string currentGym;
    private int passingScore, maxScore;

    // Start is called before the first frame update
    void Start()
    {
        currentGym = StateManager.currentGym;
        string userEmail = StateManager.user.email;
        StartCoroutine(getGymScores());
        StartCoroutine(CheckIfPass(userEmail));
    }

    private IEnumerator getGymScores()
    {
        string gymID="";
        if (currentGym == "Gym1") { gymID = GYM1; }
        else if (currentGym == "Gym2") { gymID = GYM2; }
        else if (currentGym == "Gym3") { gymID = GYM3; }
        else if (currentGym == "Gym4") { gymID = GYM4; }
        string gymURL = URL + "gym/" + gymID;

        using (UnityWebRequest gymRequest = UnityWebRequest.Get(gymURL))
        {
            yield return gymRequest.SendWebRequest();

            if (gymRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(gymRequest.error);
            }
            else
            {
                GymResult gymResult = JsonUtility.FromJson<GymResult>(gymRequest.downloadHandler.text);
                Gym gym = gymResult.data;
                passingScore = gym.passing_score;
                maxScore = gym.question_count;
            }
        }
    }

    private IEnumerator CheckIfPass(string userEmail)
    {
        // get attempt score
        string userAttemptsURL = URL + "attempt/email/" + userEmail;
        using (UnityWebRequest attemptRequest = UnityWebRequest.Get(userAttemptsURL))
        {
            yield return attemptRequest.SendWebRequest();

            if (attemptRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(attemptRequest.error);
            }
            else
            {
                yield return new WaitForSecondsRealtime(1);
                AttemptGetResult attemptResult = JsonUtility.FromJson<AttemptGetResult>(attemptRequest.downloadHandler.text);              
                AttemptModel[] attempts = attemptResult.data;
                Debug.Log("number of attempts:" + attempts.Length.ToString());
                AttemptModel currAttempt = attempts[attempts.Length-1];
                
                int score = currAttempt.score;
                string text;
                if (score >= passingScore)
                {
                    text = "Good job! You passed the gym battle with a score of "+ score.ToString() +" out of "+ maxScore.ToString() +".";
                    if (currentGym != "Gym4") {text += "You may now move on to the next map!";}
                    StartCoroutine(UpdateStateManagerUser(userEmail));
                }
                else
                {
                    text = "You failed, but don't worry! Study more and try the battle again.";
                }

                Debug.Log("quiz results: " + text);
                RenameTMPLabel(text, Text);
            }
        }
    }

    public void RenameTMPLabel(string text, TextMeshProUGUI label) => label.text = text;

    private IEnumerator UpdateStateManagerUser(string userEmail)
    {
        string userURL = URL + "account/" + userEmail;

        using (UnityWebRequest userRequest = UnityWebRequest.Get(userURL))
        {
            yield return userRequest.SendWebRequest();

            if (userRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(userRequest.error);
            }
            else
            {
                UserResult userResult = JsonUtility.FromJson<UserResult>(userRequest.downloadHandler.text);
                User user = userResult.data;
                Array.Copy(user.unlocked_map, StateManager.user.unlocked_map, user.unlocked_map.Length);
                // StateManager.user.unlocked_map = user.unlocked_map;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Button_Clicked()
    {
        SceneManager.LoadScene(currentGym);
    }
}
