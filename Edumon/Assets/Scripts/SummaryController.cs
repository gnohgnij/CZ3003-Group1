using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SummaryController : MonoBehaviour
{
    public TextMeshProUGUI ChallengeHistory;
    public TextMeshProUGUI AssignmentHistory;
    public TextMeshProUGUI GymHistory;
    private string usernameInput;
    private string email;
    private List<Assignment> listOfAssignments = new List<Assignment>();
    private List<Challenge> listOfChallenges = new List<Challenge>();

    private string attemptURL = "https://cz3003-edumon.herokuapp.com/attempt/email/";
    private string accountURL = "https://cz3003-edumon.herokuapp.com/account";

    private void Start()
    {

    }

    public void ReadUsernameInput(string s) //teacher inputs student's username
    {
        usernameInput = s;
        Debug.Log(usernameInput);
    }

    public void OnGenerateReportButton() //will run GetEmail when the generate report button clicked
    {
        //reset the report
        AssignmentHistory.text = "";
        GymHistory.text = "";
        ChallengeHistory.text = "";
        listOfAssignments.Clear();
        listOfChallenges.Clear();

        StartCoroutine(GetEmail(usernameInput));
    }

    IEnumerator GetEmail(string username) //get student email via student username
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(accountURL))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                string jsonString = webRequest.downloadHandler.text;
                AccountResponse accountResponse = JsonUtility.FromJson<AccountResponse>(jsonString);
                Account[] accounts = accountResponse.data;
                for (int i = 0; i < (accounts.Count()); i++)
                {
                    if ((accounts[i].username) == usernameInput)
                    {
                        email = accounts[i].email; //get student's email
                        yield return StartCoroutine(GetReport(attemptURL + email.ToString()));
                    }
                }
            }
        }
    }

    IEnumerator GetReport(string username)
    //get student report via link: https://cz3003-edumon.herokuapp.com/attempt/email/ + student email
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(attemptURL + email))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                string jsonString = webRequest.downloadHandler.text;
                AttemptEmailResponse attemptEmailResponse = JsonUtility.FromJson<AttemptEmailResponse>(jsonString);
                Attempt[] attempt = attemptEmailResponse.data;

                yield return StartCoroutine(GetAllAssignments());
                yield return StartCoroutine(GetAllChallenges());

                //key - question_set, value - score
                Dictionary<string, int> map = new Dictionary<string, int>();
                foreach (Attempt a in attempt)
                {
                    //if student has multiple attempts on the same qn set, 
                    //take the attempt with the highest score
                    if (!map.ContainsKey(a.question_set))
                        map.Add(a.question_set, a.score);
                    else
                    {
                        int initialScore = map[a.question_set];
                        if (initialScore < a.score)
                        {
                            map.Remove(a.question_set);
                            map.Add(a.question_set, a.score);
                        }
                    }
                }

                string AssignmentHist = "";
                string GymHist = "";
                string ChallengeHist = "";

                //match question set with gym_id, challenge id and assignment id
                for (int i = 0; i < listOfAssignments.Count; i++)
                {
                    if (map.ContainsKey(listOfAssignments[i].assignment_id))
                    {
                        AssignmentHist += "Assignment (" + listOfAssignments[i].assignment_id + ") Score - " + map[listOfAssignments[i].assignment_id] + "\n";
                    }
                }

                for (int i = 0; i < listOfChallenges.Count; i++)
                {
                    if (map.ContainsKey(listOfChallenges[i].challenge_id))
                    {
                        ChallengeHist += "Challenge (From: " + listOfChallenges[i].from_email + ") Score - " + map[listOfChallenges[i].challenge_id] + "\n";
                    }
                }

                for (int i = 0; i < StateManager.gymIdList.Length; i++)
                {
                    if (map.ContainsKey(StateManager.gymIdList[i]))
                    {
                        GymHist += "Gym " + (i+1) + " Score - " + map[StateManager.gymIdList[i]] + "\n";
                    }
                }

                

                // for (int i = 0; i < (attempt.Count()); i++) //loops through all attempts 
                // {
                //     if ((attempt[i].question_group) == "ASSIGNMENT") //sort into assignment, gym and challenge scores
                //     {
                //         if (!string.IsNullOrEmpty(AssignmentHist))
                //         {
                //             AssignmentHist += ", ";                            
                //         }
                //         AssignmentHist += attempt[i].score;
                //     }
                //     if ((attempt[i].question_group) == "GYM")
                //     {
                //         if (!string.IsNullOrEmpty(GymHist))
                //         {
                //             GymHist += ", ";
                //         }
                //         GymHist += attempt[i].score;
                //     }
                //     if ((attempt[i].question_group) == "CHALLENGE")
                //     {
                //         if (!string.IsNullOrEmpty(ChallengeHist))
                //         {
                //             ChallengeHist += ", ";
                //         }
                //         ChallengeHist+=attempt[i].score;
                //     }
                // }
                
                //udpate text
                if (string.IsNullOrEmpty(AssignmentHist)){
                    AssignmentHistory.text = "No records.";
                }
                else{
                    AssignmentHistory.text = AssignmentHist;
                }
                if (string.IsNullOrEmpty(GymHist)){
                    GymHistory.text = "No records.";
                }
                else{
                    GymHistory.text = GymHist;
                }
                if (string.IsNullOrEmpty(ChallengeHist))
                {
                    ChallengeHistory.text = "No records.";
                }
                else
                {
                    ChallengeHistory.text = ChallengeHist;
                }
            }
        }
    }

    IEnumerator GetAllAssignments()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://cz3003-edumon.herokuapp.com/assignment"))
        {
            //web request for email
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError) //checks for internet error?
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log("Success");
                string jsonString = webRequest.downloadHandler.text; //creates jsonnode with full parsed data?
                Debug.Log(jsonString);

                ListOfAssignments assignmentResponse = JsonUtility.FromJson<ListOfAssignments>(jsonString);
                foreach (Assignment a in assignmentResponse.data)
                {
                    listOfAssignments.Add(a);
                }
                Debug.Log(listOfAssignments.Count);
                // AssignmentText.text = string.Join(", ", listOfAssignments[1]);
            }

        }
    }


    IEnumerator GetAllChallenges()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://cz3003-edumon.herokuapp.com/challenge"))
        {
            //web request for email
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError) //checks for internet error?
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log("Success");
                string jsonString = webRequest.downloadHandler.text; //creates jsonnode with full parsed data?
                Debug.Log(jsonString);

                ListOfChallenges challengeResponse = JsonUtility.FromJson<ListOfChallenges>(jsonString);
                foreach (Challenge c in challengeResponse.data)
                {
                    listOfChallenges.Add(c);
                }
                Debug.Log(listOfChallenges.Count);
            }
        }
    }
}




    /*IEnumerator GetReport(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            //web request for email
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.result == UnityWebRequest.Result.ConnectionError) //checks for internet error?
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                string attemptResponse = webRequest.downloadHandler.text; //creates jsonnode with full parsed data?
                Debug.Log(attemptResponse);

                AttemptEmailResponse attemptEmailResponse = JsonUtility.FromJson<AttemptEmailResponse>(attemptResponse);
                Attempt[] attempt = attemptEmailResponse.data;

                //Calculation of total points
                int totalPoints = 0;


                //key - question_set, value - score
                Dictionary<string, int> map = new Dictionary<string, int>();
                foreach (Attempt a in attempt)
                {
                    //if student has multiple attempts on the same qn set, 
                    //take the attempt with the highest score
                    if (!map.ContainsKey(a.question_set))
                        map.Add(a.question_set, a.score);
                    else
                    {
                        int initialScore = map[a.question_set];
                        if (initialScore < a.score)
                        {
                            map.Remove(a.question_set);
                            map.Add(a.question_set, a.score);
                        }
                    }
                }

                yield return StartCoroutine(GetAllAssignments());
                yield return StartCoroutine(GetAllChallenges());

                //match question set with challenge id and assignment id
                for (int i = 0; i < listOfAssignments.Count; i++)
                {
                    if (map.ContainsKey(listOfAssignments[i].assignment_id))
                    {
                        Debug.Log("Assignment: " + listOfAssignments[i].assignment_id + " " + listOfAssignments[i].deadline);
                    }
                }

                for (int i = 0; i < listOfChallenges.Count; i++)
                {
                    if (map.ContainsKey(listOfChallenges[i].challenge_id))
                    {
                        Debug.Log("Challenge: " + listOfChallenges[i].challenge_id + " " + listOfChallenges[i].from_email);
                    }
                }

                for (int i = 0; i < StateManager.gymIdList.Length; i++)
                {
                    if (map.ContainsKey(StateManager.gymIdList[i]))
                    {
                        Debug.Log("Gym" + (i + 1));
                    }
                }
            }
        }

    }*/







    





[System.Serializable]
public class ListOfAssignments
{
    public bool success;
    public Assignment[] data;
}


[System.Serializable]
    public class ListOfChallenges
    {
        public bool success;
        public Challenge[] data;
    }
