using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//TODO: Challenge manager to handle 

public class MCQ : MonoBehaviour
{
    private const string URL = "https://cz3003-edumon.herokuapp.com/";
    private EventSystem EventSystem;
    private int AnswerSelection = 0;
    public TextMeshProUGUI QuestionText;
    public Button ButtonA;
    public TextMeshProUGUI ButtonAText;
	public Button ButtonB;
    public TextMeshProUGUI ButtonBText;
	public Button ButtonC;
    public TextMeshProUGUI ButtonCText;
	public Button ButtonD;
    public TextMeshProUGUI ButtonDText;
	public Button ButtonE;
    public TextMeshProUGUI ButtonEText;
	public Button SubmitButton;

    // Start is called before the first frame update
    void Start()
    {
        //Add listeners for answer buttons
        ButtonA.onClick.AddListener(() => AnswerButtonClicked(1));
        ButtonB.onClick.AddListener(() => AnswerButtonClicked(2));
        ButtonC.onClick.AddListener(() => AnswerButtonClicked(3));
        ButtonD.onClick.AddListener(() => AnswerButtonClicked(4));
        ButtonE.onClick.AddListener(() => AnswerButtonClicked(5));
        SubmitButton.onClick.AddListener(SubmitButtonClicked);
        
        //Disable fifth button as challenges only have 4 options
        ButtonE.gameObject.SetActive(false);

        //Hardcoded question ID here
        StartCoroutine(DisplayQuestionFromId("SM3AlYWKEDIrOo02UJtl"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SubmitButtonClicked() {
        //Insert logic here
        Debug.Log("Submitting option: " + AnswerSelection);
    }

    public void RenameTMPLabel(string text, TextMeshProUGUI label) => label.text = text;

    private IEnumerator DisplayQuestionFromId(string questionId) {
        string questionUrl = URL + "question/" + questionId;
        Debug.Log("Retrieving question from: " + questionUrl);
        using (UnityWebRequest questionRequest = UnityWebRequest.Get(questionUrl)) {
            yield return questionRequest.SendWebRequest();

            if (questionRequest.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log(questionRequest.error);
            } else {
                QuestionGetResult questionResult = JsonUtility.FromJson<QuestionGetResult>(questionRequest.downloadHandler.text);
                Question question = new Question();
                question = questionResult.data;
                
                RenameTMPLabel(question.question_text, QuestionText);
                RenameTMPLabel(question.option_one, ButtonAText);
                RenameTMPLabel(question.option_two, ButtonBText);
                RenameTMPLabel(question.option_three, ButtonCText);
                RenameTMPLabel(question.option_four, ButtonDText);
                //RenameTMPLabel(question.option_five, ButtonEText);
            }
        }
    }

    void AnswerButtonClicked(int option) {
        EventSystem.current.SetSelectedGameObject(null);
        AnswerSelection = option;
        if (option == 1) {
            ButtonA.Select();
        }
        if (option == 2) {
            ButtonB.Select();
        }
        if (option == 3) {
            ButtonC.Select();
        }
        if (option == 4) {
            ButtonD.Select();
        }
        if (option == 5) {
            ButtonE.Select();
        }
        Debug.Log("Answer Button Clicked: " + option);
    }

    private void EndChallenge() => SceneManager.LoadScene("Gym5");
}
