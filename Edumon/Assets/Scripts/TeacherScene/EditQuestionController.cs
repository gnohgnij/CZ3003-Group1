using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class EditQuestionController : MonoBehaviour
{
    public InputField QuestionInputField;
    public InputField Option1InputField;
    public InputField Option2InputField;
    public InputField Option3InputField;
    public InputField Option4InputField;
    public InputField Option5InputField;
    public InputField AnswerInputField;
    public Text WarningText;
    public Button NextBtn;

    // Start is called before the first frame update
    void Start()
    {
        if ((StateManager.gymQustionIndex + 1) == StateManager.gymQuestions.Length)
        {
            NextBtn.gameObject.SetActive(false);
        }
        Display_Question();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Display_Question()
    {
        QuestionInputField.text = StateManager.gymQuestions[StateManager.gymQustionIndex].question_text;
        Option1InputField.text = StateManager.gymQuestions[StateManager.gymQustionIndex].option_one;
        Option2InputField.text = StateManager.gymQuestions[StateManager.gymQustionIndex].option_two;
        if (!string.IsNullOrWhiteSpace(StateManager.gymQuestions[StateManager.gymQustionIndex].option_three))
            Option3InputField.text = StateManager.gymQuestions[StateManager.gymQustionIndex].option_three;
        else Option3InputField.text = "";
        if (!string.IsNullOrWhiteSpace(StateManager.gymQuestions[StateManager.gymQustionIndex].option_four))
            Option4InputField.text = StateManager.gymQuestions[StateManager.gymQustionIndex].option_four;
        else Option4InputField.text = "";
        if (!string.IsNullOrWhiteSpace(StateManager.gymQuestions[StateManager.gymQustionIndex].option_five))
            Option5InputField.text = StateManager.gymQuestions[StateManager.gymQustionIndex].option_five;
        else Option5InputField.text = "";
        AnswerInputField.text = StateManager.gymQuestions[StateManager.gymQustionIndex].answer.ToString();
    }

    public void Btn_Back_Clicked()
    {
        if (StateManager.gymQustionIndex == 0)
        {
            SceneManager.LoadScene("EditGymQuestions");
        }
        else
        {
            StateManager.gymQustionIndex--;
            NextBtn.gameObject.SetActive(true);
            Display_Question();
        }
    }

    public void Btn_Next_Clicked()
    {
        StateManager.gymQustionIndex++;
        if ((StateManager.gymQustionIndex + 1) == StateManager.gymQuestions.Length)
        {
            NextBtn.gameObject.SetActive(false);
        }
        Display_Question();
    }

    public void Btn_Edit_Clicked()
    {
        string _question = QuestionInputField.text;
        string _option1 = Option1InputField.text;
        string _option2 = Option2InputField.text;
        string _option3 = Option3InputField.text;
        string _option4 = Option4InputField.text;
        string _option5 = Option5InputField.text;
        string _answer = AnswerInputField.text;

        // Question Error Checking
        WarningText.gameObject.SetActive(false);
        if (string.IsNullOrWhiteSpace(_question) || string.IsNullOrWhiteSpace(_option1) || string.IsNullOrWhiteSpace(_option2) || string.IsNullOrWhiteSpace(_answer))
        {
            WarningText.text = "Question, Option1, Option2 and Answer\ncannot be empty";
            WarningText.gameObject.SetActive(true);
        }
        else if (!string.IsNullOrWhiteSpace(_option4) && string.IsNullOrWhiteSpace(_option3))
        {
            WarningText.text = "Please put your Option 4 answer into Option 3";
            WarningText.gameObject.SetActive(true);
        }
        else if (!string.IsNullOrWhiteSpace(_option5) && string.IsNullOrWhiteSpace(_option3))
        {
            WarningText.text = "Please put your Option 5 answer into Option 3";
            WarningText.gameObject.SetActive(true);
        }
        else if (!string.IsNullOrWhiteSpace(_option5) && string.IsNullOrWhiteSpace(_option4))
        {
            WarningText.text = "Please put your Option 5 answer into Option 4";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            int questionCount = 2;
            if (!string.IsNullOrWhiteSpace(_option3)) questionCount++;
            if (!string.IsNullOrWhiteSpace(_option4)) questionCount++;
            if (!string.IsNullOrWhiteSpace(_option5)) questionCount++;
            if (int.Parse(_answer) < 1 || int.Parse(_answer) > questionCount)
            {
                WarningText.text = "Invalid Answer";
                WarningText.gameObject.SetActive(true);
            }
            else
            {
                // Update Question
                StartCoroutine(Update_Question(_question, _option1, _option2, _option3, _option4, _option5, _answer));
            }
        }
    }

    private IEnumerator Update_Question(string _question, string _option1, string _option2, string _option3, string _option4, string _option5, string _answer)
    {
        // Update the question on the database
        string questionJsonBody = "{ \"question_text\": \"" + _question + "\", ";
        questionJsonBody += " \"option_one\": \"" + _option1 + "\", ";
        questionJsonBody += " \"option_two\": \"" + _option2 + "\", ";
        if (!string.IsNullOrWhiteSpace(_option3)) questionJsonBody += " \"option_three\": \"" + _option3 + "\", ";
        if (!string.IsNullOrWhiteSpace(_option4)) questionJsonBody += " \"option_four\": \"" + _option4 + "\", ";
        if (!string.IsNullOrWhiteSpace(_option5)) questionJsonBody += " \"option_five\": \"" + _option5 + "\", ";
        questionJsonBody += "\"answer\": " + _answer + "}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(questionJsonBody);

        string questionUrl = StateManager.apiUrl + "question/" + StateManager.gymQuestions[StateManager.gymQustionIndex].question_id;
        var questionUwr = new UnityWebRequest(questionUrl, "PUT");

        questionUwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        questionUwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        questionUwr.SetRequestHeader("Content-Type", "application/json");

        yield return questionUwr.SendWebRequest();

        if (questionUwr.result == UnityWebRequest.Result.ConnectionError)
        {
            WarningText.text = "Network Error";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            QuestionPostResult questionPostResult = JsonUtility.FromJson<QuestionPostResult>(questionUwr.downloadHandler.text);
            if (questionPostResult.status == "fail")
            {
                WarningText.text = "Unable to update question. Please contact the admin.\nError: " + questionPostResult.message;
                WarningText.gameObject.SetActive(true);
            }
            else
            {
                // Update question locally
                StateManager.gymQuestions[StateManager.gymQustionIndex].question_text = _question;
                StateManager.gymQuestions[StateManager.gymQustionIndex].option_one = _option1;
                StateManager.gymQuestions[StateManager.gymQustionIndex].option_two = _option2;
                if (!string.IsNullOrWhiteSpace(_option3)) StateManager.gymQuestions[StateManager.gymQustionIndex].option_three = _option3;
                else StateManager.gymQuestions[StateManager.gymQustionIndex].option_three = null;
                if (!string.IsNullOrWhiteSpace(_option4)) StateManager.gymQuestions[StateManager.gymQustionIndex].option_four = _option4;
                else StateManager.gymQuestions[StateManager.gymQustionIndex].option_four = null;
                if (!string.IsNullOrWhiteSpace(_option5)) StateManager.gymQuestions[StateManager.gymQustionIndex].option_five = _option5;
                else StateManager.gymQuestions[StateManager.gymQustionIndex].option_five = null;
                StateManager.gymQuestions[StateManager.gymQustionIndex].answer = int.Parse(_answer);

                WarningText.text = "Updated question successfully";
                WarningText.gameObject.SetActive(true);
            }
        }
    }
}
