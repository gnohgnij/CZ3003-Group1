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
    private string currentGym, nextMap;
    private int passingScore, maxScore;

    // Start is called before the first frame update
    void Start()
    {
        currentGym = StateManager.currentGym;
        string userEmail = StateManager.user.email;
        // StartCoroutine(getGymScores());
        StartCoroutine(UpdateStateManagerUser(userEmail));
        StartCoroutine(CheckIfPass(userEmail));
    }

    // private IEnumerator getGymScores()
    // {
    //     string gymID="";
    //     if (currentGym == "Gym1") { gymID = GYM1; nextMap = "Map2"; }
    //     else if (currentGym == "Gym2") { gymID = GYM2; nextMap = "Map3"; }
    //     else if (currentGym == "Gym3") { gymID = GYM3; nextMap = "Map4"; }
    //     else if (currentGym == "Gym4") { gymID = GYM4; nextMap = "Map5"; }
    //     string gymURL = URL + "gym/" + gymID;

    //     using (UnityWebRequest gymRequest = UnityWebRequest.Get(gymURL))
    //     {
    //         yield return gymRequest.SendWebRequest();

    //         if (gymRequest.result == UnityWebRequest.Result.ConnectionError)
    //         {
    //             Debug.Log(gymRequest.error);
    //         }
    //         else
    //         {
    //             GymResult gymResult = JsonUtility.FromJson<GymResult>(gymRequest.downloadHandler.text);
    //             Gym gym = gymResult.data;
    //             passingScore = gym.passing_score;
    //             maxScore = gym.question_count;
    //         }
    //     }
    // }

    private IEnumerator CheckIfPass(string userEmail)
    {
        // get attempt score
        string userAttemptsURL = URL + "attempt/email/" + userEmail;

        if (currentGym == "Gym1") { nextMap = "Map2"; }
        else if (currentGym == "Gym2") { nextMap = "Map3"; }
        else if (currentGym == "Gym3") { nextMap = "Map4"; }
        else if (currentGym == "Gym4") { nextMap = "Map5"; }

        using (UnityWebRequest attemptRequest = UnityWebRequest.Get(userAttemptsURL))
        {
            yield return attemptRequest.SendWebRequest();

            if (attemptRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(attemptRequest.error);
            }
            else
            {
                // yield return new WaitForSecondsRealtime(1);
                AttemptGetResult attemptResult = JsonUtility.FromJson<AttemptGetResult>(attemptRequest.downloadHandler.text);              
                AttemptModel[] attempts = attemptResult.data;
                Debug.Log("number of attempts:" + attempts.Length.ToString());
                AttemptModel currAttempt = attempts[attempts.Length-1]; //change how to retrieve score
                
                int score = currAttempt.score;
                string text;
                // if (score >= passingScore)
                if (Array.IndexOf(StateManager.user.unlocked_map, nextMap) > 0)
                {
                    // text = "Good job! You passed the gym battle with a score of "+ score.ToString() +" out of "+ maxScore.ToString() +".";
                    text = "Good job! You passed the gym battle.";
                    if (currentGym == "Gym4") {text += "You have successfully completed all gym battles in this game.";}
                    else {text += "You may now move on to the next map!"; }
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
        string userURL = URL + "account/email/" + userEmail;

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
                // StateManager.user = userResult.data;
                User user = userResult.data;
                Debug.Log("user.unlocked_map[0]: " + user.unlocked_map[0]);
                Debug.Log("StateManager.user.unlocked_map[0]: " + StateManager.user.unlocked_map[0]);
                // string[] StateManager.user.unlocked_map = new string[user.unlocked_map.Length];
                // string[] temp = new string[user.unlocked_map.Length];
                // items.CopyTo(temp, 0);
                // items = temp;
                // Array.Copy(user.unlocked_map, StateManager.user.unlocked_map, user.unlocked_map.Length);
                // StateManager.user.unlocked_map = user.unlocked_map;
                StateManager.user = user;
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
