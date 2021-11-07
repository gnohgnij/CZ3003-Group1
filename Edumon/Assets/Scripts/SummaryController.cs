using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;

public class SummaryController : MonoBehaviour
{
    public TextMeshProUGUI ReportText;
    private string input;
    private string email = "";

    private readonly string attemptURL = "https://cz3003-edumon.herokuapp.com/attempt/";
    private readonly string accountURL = "https://cz3003-edumon.herokuapp.com/account";

    private void Start()
    {
        /* TextMeshProUGUI rptText = ReportText;
        ReportText.text = "";
        */

        ReportText.text = ""; //blank out text game object
    }

    public void ReadUsernameInput(string username) //get the student's username as input from teacher
    {
        input = username;
    }

    //remember to link to the button add component and drag to onclick part
    //add script as a component to main camera -> drag and drop main camera to component of the buttons -> select function
    public void OnGenerateReportButton()
    {
        StartCoroutine(GetReport());
    }

    IEnumerator GetReport()
    {
        UnityWebRequest emailRequest = UnityWebRequest.Get(accountURL); //web request for email
        yield return emailRequest.SendWebRequest();

        if (emailRequest.isNetworkError || emailRequest.isHttpError) //checks for internet error?
        {
            Debug.LogError(emailRequest.error);
            yield break;
        }

        JSONNode accountInfo = JSON.Parse(emailRequest.downloadHandler.text); //creates jsonnode with full parsed data?

        for (var i = 0; i < accountInfo[1].Count; i++) //loop through all accounts to check if username matches input
        {
            Debug.Log(input);
            if (accountInfo[1][i]["username"] == input) 
            {
                email = accountInfo[1][i]["email"]; //updates email 
                Debug.Log(accountInfo[1][i]["username"]);
            }
        }

        string scoreURL = attemptURL + "email/ch0109ng@e.ntu.edu.sg"; //https://cz3003-edumon.herokuapp.com/attempt/email/ and add in the persons email
        

        UnityWebRequest scoreRequest = UnityWebRequest.Get(scoreURL);
        yield return scoreRequest.SendWebRequest();

        if (scoreRequest.isNetworkError || scoreRequest.isHttpError)
        {
            Debug.LogError(scoreRequest.error);
            yield break;
        }

        JSONNode scoreRecords = JSON.Parse(scoreRequest.downloadHandler.text); //should record all attempt data
        int[] allScores = new int[scoreRecords[1].Count]; //new int array to store each score

        for (int i = 0; i < scoreRecords[1].Count; i++)
        {
            allScores[i] = scoreRecords[1][i]["score"]; //update the array
        }

        for (int i = 0; i < allScores.Length; i++) 
        {
            ReportText.text += allScores[i].ToString();
        }

    }




}