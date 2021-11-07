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
    private string email;

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

        for (var i = 0; i < accountInfo.Count; i++) //loop through all accounts to check if username matches input
        {
            if (accountInfo[i]["username"] == input) 
            {
                email = accountInfo[i]["email"]; //updates email 
            }
        }

        string scoreURL = attemptURL + "email/" + email.ToString(); //https://cz3003-edumon.herokuapp.com/attempt/email/ and add in the persons email

        UnityWebRequest scoreRequest = UnityWebRequest.Get(scoreURL);
        yield return scoreRequest.SendWebRequest();

        if (scoreRequest.isNetworkError || scoreRequest.isHttpError)
        {
            Debug.LogError(scoreRequest.error);
            yield break;
        }

        JSONNode scoreRecords = JSON.Parse(scoreRequest.downloadHandler.text); //should record all attempt data
        int[] allScores = new int[scoreRecords.Count]; //new int array to store each score

        for (int i = 0; i < scoreRecords.Count; i++)
        {
            allScores[i] = scoreRecords[i]["score"]; //update the array
        }

        ReportText.text = allScores.ToString(); //update the text gameobject


    }




}