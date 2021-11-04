using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CreateChallengeController : MonoBehaviour
{
    public InputField QuestionNumberInputField;
    public InputField opponentEmailField;
    public Text WarningText;

    // Start is called before the first frame update
    void Start()
    {
        if (StateManager.challengeQuestionSize != 0)
        {
            QuestionNumberInputField.text = StateManager.challengeQuestionSize.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Create_Clicked()
    {
        WarningText.gameObject.SetActive(false);
        string noOfQuestions = QuestionNumberInputField.text;
        if (string.IsNullOrWhiteSpace(noOfQuestions))
        {
            WarningText.text = "Number of questions cannot be empty";
            WarningText.gameObject.SetActive(true);
        }
        else if (int.Parse(noOfQuestions) < 1)
        {
            WarningText.text = "Invalid number of questions";
            WarningText.gameObject.SetActive(true);
        }
        else if (int.Parse(noOfQuestions) != 3)
        {
            WarningText.text = "Only 3 Questions is required for challenge";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            StateManager.challengeQuestionSize = int.Parse(noOfQuestions);
            StateManager.challengeQuestions = new Question[int.Parse(noOfQuestions)];
            StateManager.challengeQuestionIndex = 0;
            SceneManager.LoadScene("CreateChallengeQuestion");
        }
    }

    public void Btn_Back_Clicked()
    {
        WarningText.gameObject.SetActive(false);
        StateManager.challengeQuestionSize = 0;
        SceneManager.LoadScene("ChallengeView");
    }

    public void Btn_Submit_Clicked()
    {
        StartCoroutine(Submit_Challenge(opponentEmailField.text));
    }

    private IEnumerator Submit_Challenge(string opponentEmail)
    {
        WarningText.gameObject.SetActive(false);
        if (StateManager.challengeQuestionSize == 0)
        {
            WarningText.text = "There are no questions in challenge.";
            WarningText.gameObject.SetActive(true);
        }
        else if (string.IsNullOrWhiteSpace(opponentEmail))
        {
            WarningText.text = "Opponent Email cannot be empty";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            bool stop = false;

            string[] question_list_array = new string[StateManager.challengeQuestions.Length];
            if (!stop)
            {
                // Post Questions
                for (int i = 0; i < StateManager.challengeQuestions.Length; i++)
                {
                    string questionUrl = StateManager.apiUrl + "question";
                    WWWForm questionForm = new WWWForm();
                    questionForm.AddField("question_text", StateManager.challengeQuestions[i].question_text);
                    questionForm.AddField("option_one", StateManager.challengeQuestions[i].option_one);
                    questionForm.AddField("option_two", StateManager.challengeQuestions[i].option_two);
                    if (!string.IsNullOrWhiteSpace(StateManager.challengeQuestions[i].option_three)) questionForm.AddField("option_three", StateManager.challengeQuestions[i].option_three);
                    if (!string.IsNullOrWhiteSpace(StateManager.challengeQuestions[i].option_four)) questionForm.AddField("option_four", StateManager.challengeQuestions[i].option_four);
                    if (!string.IsNullOrWhiteSpace(StateManager.challengeQuestions[i].option_five)) questionForm.AddField("option_five", StateManager.challengeQuestions[i].option_five);
                    questionForm.AddField("answer", StateManager.challengeQuestions[i].answer);

                    UnityWebRequest questionUwr = UnityWebRequest.Post(questionUrl, questionForm);
                    yield return questionUwr.SendWebRequest();

                    if (questionUwr.result == UnityWebRequest.Result.ConnectionError)
                    {
                        WarningText.text = "Network Error";
                        WarningText.gameObject.SetActive(true);
                    }
                    else
                    {
                        QuestionPostResult questionResult = JsonUtility.FromJson<QuestionPostResult>(questionUwr.downloadHandler.text);
                        if (questionResult.status == "success")
                        {
                            question_list_array[i] = questionResult.data;
                        }
                        else
                        {
                            Debug.Log("CreateChallengeController.cs\nUnable to create question. Please contact the admin");
                            stop = true;
                        }
                    }
                }
            }
                    

            if (!stop)
            {
                // Post Challenge
                string question_list = "[\"" + question_list_array[0] + "\"";
                for (int j = 1; j < question_list_array.Length; j++)
                {
                    question_list += ", \"" + question_list_array[j] + "\"";
                }
                question_list += "]";

                string challengeJsonBody = "{ \"question_list\": " + question_list + ", ";
                challengeJsonBody += "\"from_email\": \"" + StateManager.user.email + "\", ";
                challengeJsonBody += "\"to_email\": \"" + opponentEmail + "\" }";
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(challengeJsonBody);

                Debug.Log(challengeJsonBody);

                string challengeUrl = StateManager.apiUrl + "challenge";
                var challengeUwr = new UnityWebRequest(challengeUrl, "POST");

                Debug.Log(challengeUrl);

                challengeUwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                challengeUwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                challengeUwr.SetRequestHeader("Content-Type", "application/json");

                yield return challengeUwr.SendWebRequest();

                if (challengeUwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    WarningText.text = "Network Error";
                    WarningText.gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log(challengeUwr.downloadHandler.text);
                    ChallengePostResult challengeResult = JsonUtility.FromJson<ChallengePostResult>(challengeUwr.downloadHandler.text);
                    if (challengeResult.status == "success")
                    {
                        StateManager.challengeQuestionIndex = 0;
                        StateManager.challengeQuestionSize = 0;
                        StateManager.challengeQuestions = null;

                        StateManager.challengeStatusTag = true;
                        StateManager.challengeStatusMessage = "Successfully created challenge";
                        SceneManager.LoadScene("ChallengeView");
                    }
                    else
                    {
                        WarningText.text = "Unable to post challenge.\nPlease contact the admin";
                        WarningText.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
