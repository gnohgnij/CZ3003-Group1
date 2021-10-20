using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CreateNewQuestionController : MonoBehaviour
{
    public InputField QuestionInputField;
    public InputField Option1InputField;
    public InputField Option2InputField;
    public InputField Option3InputField;
    public InputField Option4InputField;
    public InputField Option5InputField;
    public InputField AnswerInputField;
    public Text WarningText;
    public Text NextQuestionBtnText;

    void Start()
    {
        if ((StateManager.questionIndex + 1) == StateManager.assignmentQuestionSize)
        {
            NextQuestionBtnText.text = "Create Question";
        }
    }

    public void Btn_Back_Clicked()
    {
        if (StateManager.questionIndex == 0)
        {
            SceneManager.LoadScene("SetAssignment");
        }
        else
        {
            if ((StateManager.questionIndex + 1) == StateManager.assignmentQuestionSize)
            {
                NextQuestionBtnText.text = "Next Question";
            }
            StateManager.questionIndex--;
            Display_Question();
        }
    }

    private void Display_Question()
    {
        if (StateManager.assignmentQuestions[StateManager.questionIndex] != null)
        {
            QuestionInputField.text = StateManager.assignmentQuestions[StateManager.questionIndex].question_text;
            Option1InputField.text = StateManager.assignmentQuestions[StateManager.questionIndex].option_one;
            Option2InputField.text = StateManager.assignmentQuestions[StateManager.questionIndex].option_two;
            if (!string.IsNullOrWhiteSpace(StateManager.assignmentQuestions[StateManager.questionIndex].option_three))
                Option3InputField.text = StateManager.assignmentQuestions[StateManager.questionIndex].option_three;
            if (!string.IsNullOrWhiteSpace(StateManager.assignmentQuestions[StateManager.questionIndex].option_four))
                Option4InputField.text = StateManager.assignmentQuestions[StateManager.questionIndex].option_four;
            if (!string.IsNullOrWhiteSpace(StateManager.assignmentQuestions[StateManager.questionIndex].option_five))
                Option5InputField.text = StateManager.assignmentQuestions[StateManager.questionIndex].option_five;
            AnswerInputField.text = StateManager.assignmentQuestions[StateManager.questionIndex].answer.ToString();
        }
        else
        {
            QuestionInputField.text = null;
            Option1InputField.text = null;
            Option2InputField.text = null;
            Option3InputField.text = null;
            Option4InputField.text = null;
            Option5InputField.text = null;
            AnswerInputField.text = null;
        }
    }

    public void Btn_Create_Question_Clicked()
    {
        string _question = QuestionInputField.text;
        string _option1 = Option1InputField.text;
        string _option2 = Option2InputField.text;
        string _option3 = Option3InputField.text;
        string _option4 = Option4InputField.text;
        string _option5 = Option5InputField.text;
        string _answer = AnswerInputField.text;

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
                if (StateManager.assignmentQuestions[StateManager.questionIndex] == null)
                {
                    StateManager.assignmentQuestions[StateManager.questionIndex] = new Question();
                    StateManager.assignmentQuestions[StateManager.questionIndex].question_text = _question;
                    StateManager.assignmentQuestions[StateManager.questionIndex].option_one = _option1;
                    StateManager.assignmentQuestions[StateManager.questionIndex].option_two = _option2;
                    if (!string.IsNullOrWhiteSpace(_option3)) StateManager.assignmentQuestions[StateManager.questionIndex].option_three = _option3;
                    if (!string.IsNullOrWhiteSpace(_option4)) StateManager.assignmentQuestions[StateManager.questionIndex].option_four = _option4;
                    if (!string.IsNullOrWhiteSpace(_option5)) StateManager.assignmentQuestions[StateManager.questionIndex].option_five = _option5;
                    StateManager.assignmentQuestions[StateManager.questionIndex].answer = int.Parse(_answer);
                }

                StateManager.questionIndex++;
                if ((StateManager.questionIndex + 1) == StateManager.assignmentQuestionSize)
                {
                    NextQuestionBtnText.text = "Create Question";
                }
                else if (StateManager.questionIndex == StateManager.assignmentQuestionSize)
                {
                    SceneManager.LoadScene("SetAssignment");
                    return;
                }
                Display_Question();
            }
        }
    }

    private IEnumerator Create_Question(string _question, string _option1, string _option2, string _option3, string _option4, string _option5, string _answer)
    {
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
                //string questionUrl = StateManager.localhostUrl + "question";
                string questionUrl = StateManager.apiUrl + "question";
                WWWForm questionForm = new WWWForm();
                questionForm.AddField("question_text", _question);
                questionForm.AddField("option_one", _option1);
                questionForm.AddField("option_two", _option2);
                if (!string.IsNullOrWhiteSpace(_option3)) questionForm.AddField("option_three", _option3);
                if (!string.IsNullOrWhiteSpace(_option4)) questionForm.AddField("option_four", _option4);
                if (!string.IsNullOrWhiteSpace(_option5)) questionForm.AddField("option_five", _option5);
                questionForm.AddField("answer", _answer);

                UnityWebRequest questionUwr = UnityWebRequest.Post(questionUrl, questionForm);
                yield return questionUwr.SendWebRequest();

                if (questionUwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    WarningText.text = "Network Error";
                    WarningText.gameObject.SetActive(true);
                }
                else
                {
                    QuestionResult questionResult = JsonUtility.FromJson<QuestionResult>(questionUwr.downloadHandler.text);
                    if (questionResult.status == "success")
                    {
                        StateManager.teacherProfileStatusTag = true;
                        StateManager.teacherProfileStatusMessage = "Question Created";
                        SceneManager.LoadScene("TeacherHome");
                    }
                    else
                    {
                        WarningText.text = "Unable to create question. Please contact the admin.\nError: " + questionResult.message;
                        WarningText.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
