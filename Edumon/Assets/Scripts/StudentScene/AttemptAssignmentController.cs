using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class AttemptAssignmentController : MonoBehaviour
{
    public InputField AssignmentIdInputField;
    public Text WarningText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Back_Clicked()
    {
        SceneManager.LoadScene("StudentHome");
    }

    public void Btn_Attempt_Clicked()
    {
        StartCoroutine(Retrieve_Assignment_Data(AssignmentIdInputField.text));
    }

    private IEnumerator Retrieve_Assignment_Data(string _assignmentId)
    {
        WarningText.gameObject.SetActive(false);
        if (string.IsNullOrWhiteSpace(_assignmentId))
        {
            WarningText.text = "Assigment Id cannot be empty";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            bool stop = false;
            AssignmentGetResult assignmentResult = null;

            // Retrive Assignment Data
            string assignmentUrl = StateManager.localhostUrl + "assignment/" + _assignmentId;

            UnityWebRequest assignmentUwr = UnityWebRequest.Get(assignmentUrl);
            yield return assignmentUwr.SendWebRequest();

            if (assignmentUwr.result == UnityWebRequest.Result.ConnectionError)
            {
                WarningText.text = "Network Error";
                WarningText.gameObject.SetActive(true);
                stop = true;
            }
            else
            {
                assignmentResult = JsonUtility.FromJson<AssignmentGetResult>(assignmentUwr.downloadHandler.text);
                if (assignmentResult.status == "fail")
                {
                    WarningText.text = "Unable to get assignment data. Please contact the admin\nError: " + assignmentResult.message;
                    WarningText.gameObject.SetActive(true);
                    stop = true;
                }
                else
                {
                    if (assignmentResult.data.assignment_id == null)
                    {
                        WarningText.text = "Invalid Assignment Id";
                        WarningText.gameObject.SetActive(true);
                        stop = true;
                    }
                    else
                    {
                        assignmentResult.data.deadlineDateTime = System.DateTime.Parse(assignmentResult.data.deadline);
                        if (assignmentResult.data.deadlineDateTime < System.DateTime.UtcNow.ToLocalTime())
                        {
                            WarningText.text = "Expired Assignment";
                            WarningText.gameObject.SetActive(true);
                            stop = true;
                        }
                        else
                        {
                            StateManager.assignmentId = assignmentResult.data.assignment_id;
                            StateManager.assignmentQuestionSize = assignmentResult.data.question_count;
                            StateManager.assignmentQuestions = new Question[StateManager.assignmentQuestionSize];
                        }
                    }
                }
            }


            // Retrieve Assignment Questions
            if (!stop)
            {
                for (int i = 0; i < StateManager.assignmentQuestionSize; i++)
                {
                    string questionUrl = StateManager.localhostUrl + "question/" + assignmentResult.data.question_list[i];

                    UnityWebRequest questionUwr = UnityWebRequest.Get(questionUrl);
                    yield return questionUwr.SendWebRequest();

                    if (questionUwr.result == UnityWebRequest.Result.ConnectionError)
                    {
                        WarningText.text = "Network Error";
                        WarningText.gameObject.SetActive(true);
                        stop = true;
                    }
                    else
                    {
                        QuestionGetResult questionResult = JsonUtility.FromJson<QuestionGetResult>(questionUwr.downloadHandler.text);
                        if (questionResult.status == "fail")
                        {
                            WarningText.text = "Unable to get question data. Please contact the admin\nError: " + questionResult.message;
                            WarningText.gameObject.SetActive(true);
                            stop = true;
                        }
                        else
                        {
                            StateManager.assignmentQuestions[i] = questionResult.data;
                        }
                    }
                }
                if (!stop) SceneManager.LoadScene("AttemptAssignmentQuestions");
            }
        }
    }
}
