using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CreateChallenge : MonoBehaviour
{
    public InputField QuestionNumberInputField;
    public InputField DDInputField;
    public InputField MMInputField;
    public InputField YYYYInputField;
    public InputField hhInputField;
    public InputField mmInputField;
    public InputField ssInputField;
    public InputField emailField;
    public Text WarningText;

    // Start is called before the first frame update
    void Start()
    {
        if (StateManager.assignmentQuestionSize != 0)
        {
            QuestionNumberInputField.text = StateManager.assignmentQuestionSize.ToString();
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
        else
        {
            StateManager.assignmentQuestionSize = int.Parse(noOfQuestions);
            StateManager.assignmentQuestions = new Question[int.Parse(noOfQuestions)];
            StateManager.questionIndex = 0;
            SceneManager.LoadScene("CreateNewQuestion");
        }
    }

    public void Btn_Back_Clicked()
    {
        WarningText.gameObject.SetActive(false);
        StateManager.assignmentQuestionSize = 0;
        SceneManager.LoadScene("TeacherHome");
    }

    public void Btn_Submit_Clicked()
    {
        StartCoroutine(Submit_Assignment(DDInputField.text, MMInputField.text, YYYYInputField.text, hhInputField.text, mmInputField.text, ssInputField.text, emailField.text));
    }

    private IEnumerator Submit_Assignment(string dd, string MM, string yyyy, string HH, string mm, string ss, string email)
    {
        WarningText.gameObject.SetActive(false);
        if (StateManager.assignmentQuestionSize == 0)
        {
            WarningText.text = "There are no questions in assignment.";
            WarningText.gameObject.SetActive(true);
        }
        else if (string.IsNullOrWhiteSpace(dd) || string.IsNullOrWhiteSpace(MM) || string.IsNullOrWhiteSpace(yyyy) ||
            string.IsNullOrWhiteSpace(HH) || string.IsNullOrWhiteSpace(mm) || string.IsNullOrWhiteSpace(ss))
        {
            WarningText.text = "Deadline cannot be empty.";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            bool stop = false;
            System.DateTime deadlineDateTime = System.DateTime.UtcNow.ToLocalTime();
            try
            {
                deadlineDateTime = new System.DateTime(int.Parse(yyyy), int.Parse(MM), int.Parse(dd),
                int.Parse(HH), int.Parse(mm), int.Parse(ss), System.DateTimeKind.Local);
                deadlineDateTime = deadlineDateTime.AddHours(-8);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                stop = true;
                WarningText.text = "Unable to parse deadline to DateTime";
                WarningText.gameObject.SetActive(true);
            }

            if (!stop)
            {
                string deadline = deadlineDateTime.ToString("MM/dd/yyyy HH:mm:ss");
                if (deadlineDateTime < System.DateTime.UtcNow.ToLocalTime())
                {
                    WarningText.text = "Invalid Deadline";
                    WarningText.gameObject.SetActive(true);
                    stop = true;
                }
                else
                {
                    string[] question_list_array = new string[StateManager.assignmentQuestions.Length];
                    if (!stop)
                    {
                        // Post Questions
                        for (int i = 0; i < StateManager.assignmentQuestions.Length; i++)
                        {
                            string questionUrl = StateManager.apiUrl + "question";
                            WWWForm questionForm = new WWWForm();
                            questionForm.AddField("question_text", StateManager.assignmentQuestions[i].question_text);
                            questionForm.AddField("option_one", StateManager.assignmentQuestions[i].option_one);
                            questionForm.AddField("option_two", StateManager.assignmentQuestions[i].option_two);
                            if (!string.IsNullOrWhiteSpace(StateManager.assignmentQuestions[i].option_three)) questionForm.AddField("option_three", StateManager.assignmentQuestions[i].option_three);
                            if (!string.IsNullOrWhiteSpace(StateManager.assignmentQuestions[i].option_four)) questionForm.AddField("option_four", StateManager.assignmentQuestions[i].option_four);
                            if (!string.IsNullOrWhiteSpace(StateManager.assignmentQuestions[i].option_five)) questionForm.AddField("option_five", StateManager.assignmentQuestions[i].option_five);
                            questionForm.AddField("answer", StateManager.assignmentQuestions[i].answer);

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
                                    Debug.Log("CreateAssignmentController.cs\nUnable to create question. Please contact the admin");
                                    stop = true;
                                }
                            }
                        }
                    }
                    

                    if (!stop)
                    {
                        // Post Assignment
                        string question_list = "[\"" + question_list_array[0] + "\"";
                        for (int j = 1; j < question_list_array.Length; j++)
                        {
                            question_list += ", \"" + question_list_array[j] + "\"";
                        }
                        question_list += "]";

                        string assignmentJsonBody = "{ \"deadline\": \"" + deadline + "\", ";
                        assignmentJsonBody += "\"question_list\": " + question_list + "}";
                        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(assignmentJsonBody);

                        string assignmentUrl = StateManager.apiUrl + "assignment";
                        var assignmentUwr = new UnityWebRequest(assignmentUrl, "POST");

                        assignmentUwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                        assignmentUwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                        assignmentUwr.SetRequestHeader("Content-Type", "application/json");

                        yield return assignmentUwr.SendWebRequest();

                        if (assignmentUwr.result == UnityWebRequest.Result.ConnectionError)
                        {
                            WarningText.text = "Network Error";
                            WarningText.gameObject.SetActive(true);
                        }
                        else
                        {
                            AssignmentPostResult assignmentResult = JsonUtility.FromJson<AssignmentPostResult>(assignmentUwr.downloadHandler.text);
                            if (assignmentResult.status == "success")
                            {
                                StateManager.questionIndex = 0;
                                StateManager.assignmentQuestionSize = 0;
                                StateManager.assignmentQuestions = null;

                                StateManager.teacherHomeStatusTag = true;
                                StateManager.teacherHomeStatusMessage = "Successfully created assignment\nAssignment Id: " + assignmentResult.data;
                                SceneManager.LoadScene("TeacherHome");
                            }
                            else
                            {
                                WarningText.text = "Unable to post assignment.\nPlease contact the admin";
                                WarningText.gameObject.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
    }
}
