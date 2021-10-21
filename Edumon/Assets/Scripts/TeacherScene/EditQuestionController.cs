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
}
