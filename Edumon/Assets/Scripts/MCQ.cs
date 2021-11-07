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
    private int[] SelectedAnswers;
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
    private int NumberOfQuestions;
    private int QuestionIndex = 0;
    private string[] QuestionList;
    private string QuestionSet;
    private string QuestionGroup;
    private string UserEmail = StateManager.user.email;
    private string CurrentGym;

    // Start is called before the first frame update
    void Start()
    {
        AnswerButtons = new Button[] {ButtonA, ButtonB, ButtonC, ButtonD, ButtonE};
        AnswerButtonsText = new TextMeshProUGUI[] {ButtonAText, ButtonBText, ButtonCText, ButtonDText, ButtonEText};
        
        //Add listeners for answer buttons
        for (int i = 0; i < AnswerButtons.Length; i++) {
            int buttonIndex = i;
            AnswerButtons[i].onClick.AddListener(() => AnswerButtonClicked(buttonIndex));
        }
        SubmitButton.onClick.AddListener(SubmitButtonClicked);
        
        //Disable submit button until user selects an answer
        SubmitButton.gameObject.SetActive(false);

        QuestionGroup = StateManager.mcqQuestionGroup;

        if (String.Equals(QuestionGroup, "Challenge")) {
            NumberOfQuestions = StateManager.challengeQuestionSize;
            QuestionList = StateManager.challengeQuestionList;
            QuestionSet = StateManager.challengeId;
        } else if (String.Equals(QuestionGroup, "Gym")) {
            NumberOfQuestions = StateManager.gymQuestionsSize;
            QuestionList = StateManager.gymQuestionList;
            QuestionSet = StateManager.gymId;
            CurrentGym = StateManager.currentGym;
        }

        

        //Store user's answers in an int array
        SelectedAnswers = new int[NumberOfQuestions];

        LoadQuestionsFromQuestionListCurrentIndex();

        //Hardcoded question ID here
        //StartCoroutine(DisplayQuestionFromId("SM3AlYWKEDIrOo02UJtl"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadQuestionsFromQuestionListCurrentIndex() {
        StartCoroutine(DisplayQuestionFromId(QuestionList[QuestionIndex]));
        
        //Disable submit button after every new question displayed
        SubmitButton.gameObject.SetActive(false);
    }
    
    void SubmitButtonClicked() {
        //Prevent multiple clicks
        SubmitButton.gameObject.SetActive(false);
        //Save question answer
        SelectedAnswers[QuestionIndex] = AnswerSelection;
        //If no questions remain, submit challenge
        if ((QuestionIndex + 1) == NumberOfQuestions) {
            StartCoroutine(SubmitMCQ());
        //Otherwise, load next question
        } else {
            QuestionIndex++;
            LoadQuestionsFromQuestionListCurrentIndex();
        }
    }

    private IEnumerator SubmitMCQ() {
        //Needed: question_set, question_group, user_email, user_answers
        string userAnswers = "{\"" + QuestionList[0] + "\": " + SelectedAnswers[0];
        for (int i = 1; i < NumberOfQuestions; i++) {
            userAnswers += ", \"" + QuestionList[i] + "\": " + SelectedAnswers[i];
        }
        userAnswers += "}";
        Debug.Log("User Answers: " + userAnswers);
        
        string attemptJsonBody = "{ \"question_set\": \"" + QuestionSet + "\",";
        attemptJsonBody += " \"question_group\": \"" + QuestionGroup + "\",";
        attemptJsonBody += " \"user_email\": \"" + UserEmail + "\",";
        attemptJsonBody += "\"user_answers\": " + userAnswers;
        attemptJsonBody += "}";
        Debug.Log("Attempt JSON Body: " + attemptJsonBody);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(attemptJsonBody);

        string attemptUrl = URL + "attempt";
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
                if (String.Equals(QuestionGroup, "Challenge")) {
                StateManager.challengeStatusTag = true;
                StateManager.challengeStatusMessage = "Challenge attempt submitted";
                }
                EndMCQ();
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
                string[] questionOptionsText = {question.option_one, question.option_two, question.option_three, question.option_four, question.option_five};

                for (int i = 0; i < AnswerButtons.Length; i++) {
                    RenameTMPLabel(questionOptionsText[i], AnswerButtonsText[i]);
                    if (String.IsNullOrEmpty((AnswerButtonsText[i].text))) {
                        AnswerButtons[i].gameObject.SetActive(false);
                    } 
                }
            }
        }
    }

    void AnswerButtonClicked(int index) {
        EventSystem.current.SetSelectedGameObject(null);
        int option = index + 1;
        AnswerButtons[index].Select();
        AnswerSelection = option;
        Debug.Log("Answer Button Clicked: " + option);
        SubmitButton.gameObject.SetActive(true);
    }

    private void EndMCQ() {
        if (String.Equals(QuestionGroup, "Challenge")) {
            SceneManager.LoadScene("Challenge");
        } else if (String.Equals(QuestionGroup, "Gym")) {
            SceneManager.LoadScene("GymOutcome");
        }
    }
}
