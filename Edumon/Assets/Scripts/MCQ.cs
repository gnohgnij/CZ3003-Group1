using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MCQ : MonoBehaviour
{
    private static string URL = StateManager.apiUrl;
    private EventSystem EventSystem;
    private int AnswerSelection = 0;
    private int[] ChallengeAnswers;
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
    private Button[] AnswerButtons;
    private TextMeshProUGUI[] AnswerButtonsText;

    // Start is called before the first frame update
    void Start()
    {
        AnswerButtons = new Button[] {ButtonA, ButtonB, ButtonC, ButtonD, ButtonE};
        AnswerButtonsText = new TextMeshProUGUI[] {ButtonAText, ButtonBText, ButtonCText, ButtonDText, ButtonEText};
        
        //Add listeners for answer buttons
        ButtonA.onClick.AddListener(() => AnswerButtonClicked(1));
        ButtonB.onClick.AddListener(() => AnswerButtonClicked(2));
        ButtonC.onClick.AddListener(() => AnswerButtonClicked(3));
        ButtonD.onClick.AddListener(() => AnswerButtonClicked(4));
        ButtonE.onClick.AddListener(() => AnswerButtonClicked(5));
        SubmitButton.onClick.AddListener(SubmitButtonClicked);
        
        //Disable submit button until user selects an answer
        SubmitButton.gameObject.SetActive(false);

        //Store user's answers in an int array
        ChallengeAnswers = new int[StateManager.challengeQuestionSize];

        LoadQuestionsFromQuestionListCurrentIndex();

        //Hardcoded question ID here
        //StartCoroutine(DisplayQuestionFromId("SM3AlYWKEDIrOo02UJtl"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadQuestionsFromQuestionListCurrentIndex() {
        StartCoroutine(DisplayQuestionFromId(StateManager.challengeQuestionList[StateManager.challengeQuestionIndex]));
        
        //Disable submit button after every new question displayed
        SubmitButton.gameObject.SetActive(false);
    }
    
    void SubmitButtonClicked() {
        //Prevent multiple clicks
        SubmitButton.gameObject.SetActive(false);
        //Save question answer
        ChallengeAnswers[StateManager.challengeQuestionIndex] = AnswerSelection;
        //If no questions remain, submit challenge
        if ((StateManager.challengeQuestionIndex + 1) == StateManager.challengeQuestionSize) {
            StartCoroutine(SubmitChallenge());
        //Otherwise, load next question
        } else {
            StateManager.challengeQuestionIndex++;
            LoadQuestionsFromQuestionListCurrentIndex();
        }
    }

    private IEnumerator SubmitChallenge() {
        //Needed: question_set, question_group, user_email, user_answers
        string user_answers = "{\"" + StateManager.challengeQuestionList[0] + "\": " + ChallengeAnswers[0];
        for (int i = 1; i < StateManager.challengeQuestionSize; i++) {
            user_answers += ", \"" + StateManager.challengeQuestionList[0] + "\": " + ChallengeAnswers[i];
        }
        user_answers += "}";

        string attemptJsonBody = "{ \"question_set\": \"" + StateManager.challengeId + "\",";
        attemptJsonBody += " \"question_group\": \"Challenge\",";
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

        if (attemptUwr.result == UnityWebRequest.Result.ConnectionError) {
            QuestionText.text = "Network Error";
        } else {
            AttemptPostResult attempPostResult = JsonUtility.FromJson<AttemptPostResult>(attemptUwr.downloadHandler.text);
            if (attempPostResult.status == "fail") {
                QuestionText.text = "Unable to submit challenge. Please contact the admin\nError: " + attempPostResult.message;
            } else {
                StateManager.challengeStatusTag = true;
                StateManager.challengeStatusMessage = "Challenge attempt submitted";
                EndChallenge();
            }
        }
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
                RenameTMPLabel(question.option_five, ButtonEText);
                
                for (int i = 0; i < AnswerButtons.Length; i++) {
                    if (String.IsNullOrEmpty((AnswerButtonsText[i].text))) {
                        AnswerButtons[i].gameObject.SetActive(false);
                    } 
                }
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
        SubmitButton.gameObject.SetActive(true);
    }

    private void EndChallenge() => SceneManager.LoadScene("Challenge");
}
