using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CreateChallengeQuestionController : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        if ((StateManager.challengeQuestionIndex + 1) == StateManager.challengeQuestionSize)
        {
            NextQuestionBtnText.text = "Create Question";
        }
    }

    public void Btn_Back_Clicked()
    {
        if (StateManager.challengeQuestionIndex == 0)
        {
            SceneManager.LoadScene("CreateChallenge");
        }
        else
        {
            if ((StateManager.challengeQuestionIndex + 1) == StateManager.challengeQuestionSize)
            {
                NextQuestionBtnText.text = "Next Question";
            }
            StateManager.challengeQuestionIndex--;
            Display_Question();
        }
    }

    private void Display_Question()
    {
        if (StateManager.challengeQuestions[StateManager.challengeQuestionIndex] != null)
        {
            QuestionInputField.text = StateManager.challengeQuestions[StateManager.challengeQuestionIndex].question_text;
            Option1InputField.text = StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_one;
            Option2InputField.text = StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_two;
            if (!string.IsNullOrWhiteSpace(StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_three))
                Option3InputField.text = StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_three;
            if (!string.IsNullOrWhiteSpace(StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_four))
                Option4InputField.text = StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_four;
            if (!string.IsNullOrWhiteSpace(StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_five))
                Option5InputField.text = StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_five;
            AnswerInputField.text = StateManager.challengeQuestions[StateManager.challengeQuestionIndex].answer.ToString();
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
                if (StateManager.challengeQuestions[StateManager.challengeQuestionIndex] == null)
                {
                    StateManager.challengeQuestions[StateManager.challengeQuestionIndex] = new Question();
                    StateManager.challengeQuestions[StateManager.challengeQuestionIndex].question_text = _question;
                    StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_one = _option1;
                    StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_two = _option2;
                    if (!string.IsNullOrWhiteSpace(_option3)) StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_three = _option3;
                    if (!string.IsNullOrWhiteSpace(_option4)) StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_four = _option4;
                    if (!string.IsNullOrWhiteSpace(_option5)) StateManager.challengeQuestions[StateManager.challengeQuestionIndex].option_five = _option5;
                    StateManager.challengeQuestions[StateManager.challengeQuestionIndex].answer = int.Parse(_answer);
                }

                StateManager.challengeQuestionIndex++;
                if ((StateManager.challengeQuestionIndex + 1) == StateManager.challengeQuestionSize)
                {
                    NextQuestionBtnText.text = "Create Question";
                }
                else if (StateManager.challengeQuestionIndex == StateManager.challengeQuestionSize)
                {
                    SceneManager.LoadScene("CreateChallenge");
                    return;
                }
                Display_Question();
            }
        }
    }
}
