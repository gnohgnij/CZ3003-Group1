using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class AttemptAssignmentQuestionsController : MonoBehaviour
{
    public Text Question;
    public Text Option1;
    public Text Option2;
    public Text Option3;
    public Text Option4;
    public Text Option5;
    public InputField AnswerInputField;

    public Text Option3TextBox;
    public Text Option4TextBox;
    public Text Option5TextBox;
    public Text WarningText;

    public Button NextBtn;
    public Button SubmitBtn;

    private int option_size;
    private int[] assignmentAnswers;

    // Start is called before the first frame update
    void Start()
    {
        assignmentAnswers = new int[StateManager.assignmentQuestionSize];
        if ((StateManager.questionIndex + 1) == StateManager.assignmentQuestions.Length)
        {
            NextBtn.gameObject.SetActive(false);
            SubmitBtn.gameObject.SetActive(true);
        }
        Display_Question();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Display_Question()
    {
        Option3TextBox.gameObject.SetActive(false);
        Option4TextBox.gameObject.SetActive(false);
        Option5TextBox.gameObject.SetActive(false);
        option_size = 2;

        Question.text = StateManager.assignmentQuestions[StateManager.questionIndex].question_text;
        Option1.text = StateManager.assignmentQuestions[StateManager.questionIndex].option_one;
        Option2.text = StateManager.assignmentQuestions[StateManager.questionIndex].option_two;
        if (!string.IsNullOrWhiteSpace(StateManager.assignmentQuestions[StateManager.questionIndex].option_three))
        {
            option_size++;
            Option3TextBox.gameObject.SetActive(true);
            Option3.text = StateManager.assignmentQuestions[StateManager.questionIndex].option_three;
        }
        if (!string.IsNullOrWhiteSpace(StateManager.assignmentQuestions[StateManager.questionIndex].option_four))
        {
            option_size++;
            Option4TextBox.gameObject.SetActive(true);
            Option4.text = StateManager.assignmentQuestions[StateManager.questionIndex].option_four;
        }
        if (!string.IsNullOrWhiteSpace(StateManager.assignmentQuestions[StateManager.questionIndex].option_five))
        {
            option_size++;
            Option5TextBox.gameObject.SetActive(true);
            Option5.text = StateManager.assignmentQuestions[StateManager.questionIndex].option_five;
        }
        if (assignmentAnswers[StateManager.questionIndex] != 0)
            AnswerInputField.text = assignmentAnswers[StateManager.questionIndex].ToString();
        else AnswerInputField.text = "";
    }

    public void Btn_Back_Clicked()
    {
        WarningText.gameObject.SetActive(false);
        if (StateManager.questionIndex == 0)
        {
            SceneManager.LoadScene("StudentHome");
        }
        else
        {
            StateManager.questionIndex--;
            NextBtn.gameObject.SetActive(true);
            Display_Question();
        }
    }

    public void Btn_Next_Clicked()
    {
        WarningText.gameObject.SetActive(false);
        if (string.IsNullOrWhiteSpace(AnswerInputField.text))
        {
            WarningText.text = "Invalid Answer";
            WarningText.gameObject.SetActive(true);
        }
        else if (int.Parse(AnswerInputField.text) < 1 || int.Parse(AnswerInputField.text) > option_size)
        {
            WarningText.text = "Invalid Answer";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            assignmentAnswers[StateManager.questionIndex] = int.Parse(AnswerInputField.text);
            StateManager.questionIndex++;
            if ((StateManager.questionIndex + 1) == StateManager.assignmentQuestions.Length)
            {
                NextBtn.gameObject.SetActive(false);
                SubmitBtn.gameObject.SetActive(true);
            }
            Display_Question();
        }
    }

    public void Btn_Submit_Clicked()
    {
        WarningText.gameObject.SetActive(false);
        if (string.IsNullOrWhiteSpace(AnswerInputField.text))
        {
            WarningText.text = "Invalid Answer";
            WarningText.gameObject.SetActive(true);
        }
        else if (int.Parse(AnswerInputField.text) < 1 || int.Parse(AnswerInputField.text) > option_size)
        {
            WarningText.text = "Invalid Answer";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            assignmentAnswers[StateManager.questionIndex] = int.Parse(AnswerInputField.text);
            StartCoroutine(Submit_Assignment());
        }
    }

    private IEnumerator Submit_Assignment()
    {
        string user_answers = "{\"" + StateManager.assignmentQuestions[0].question_id + "\": " + assignmentAnswers[0];
        for (int i = 1; i < StateManager.assignmentQuestionSize; i++)
        {
            user_answers += ", \"" + StateManager.assignmentQuestions[i].question_id + "\": " + assignmentAnswers[i];
        }
        user_answers += "}";


        string attemptJsonBody = "{ \"question_set\": \"" + StateManager.assignmentId + "\",";
        attemptJsonBody += " \"question_group\": \"Assignment\",";
        attemptJsonBody += " \"user_email\": \"" + StateManager.user.email + "\",";
        attemptJsonBody += "\"user_answers\": " + user_answers;
        attemptJsonBody += "}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(attemptJsonBody);


        string attemptUrl = StateManager.apiUrl + "attempt";
        var attemptUwr = new UnityWebRequest(attemptUrl, "POST");

        attemptUwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        attemptUwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        attemptUwr.SetRequestHeader("Content-Type", "application/json");
        yield return attemptUwr.SendWebRequest();

        if (attemptUwr.result == UnityWebRequest.Result.ConnectionError)
        {
            WarningText.text = "Network Error";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            AttemptPostResult attempPostResult = JsonUtility.FromJson<AttemptPostResult>(attemptUwr.downloadHandler.text);
            if (attempPostResult.status == "fail")
            {
                WarningText.text = "Unable to submit assignment. Please contact the admin\nError: " + attempPostResult.message;
                WarningText.gameObject.SetActive(true);
            }
            else
            {
                StateManager.studentHomeStatusTag = true;
                StateManager.studentHomeStatusMessage = "Assignment submitted";
                SceneManager.LoadScene("StudentHome");
            }
        }
    }
}
