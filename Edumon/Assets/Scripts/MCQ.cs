using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

//TODO: Challenge manager to handle 

public class MCQ : MonoBehaviour
{
    private const string URL = "https://cz3003-edumon.herokuapp.com/";
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

        //Disable fifth button as challenges only have 4 options
        ButtonE.gameObject.SetActive(false);

        SubmitButton.onClick.AddListener(TaskOnClick);
        //GenerateRequest();
        StartCoroutine(DisplayQuestionFromId("SM3AlYWKEDIrOo02UJtl"));
    }

    // Update is called once per frame
    void TaskOnClick()
    {
        //Output this to console when SubmitButton is clicked
        GenerateRequest();
    }

    public void RenameTMPLabel(string text, TextMeshProUGUI label) => label.text = text;

    private void EndChallenge() => SceneManager.LoadScene("Gym5");

    private IEnumerator DisplayQuestionFromId(string questionId) {
        string questionUrl = URL + "question/" + questionId;
        Debug.Log(questionUrl);

        UnityWebRequest questionUwr = UnityWebRequest.Get(questionUrl);
        yield return questionUwr.SendWebRequest();

        //Add error handling here
        QuestionGetResult questionResult = JsonUtility.FromJson<QuestionGetResult>(questionUwr.downloadHandler.text);
        Question question = new Question();
        question = questionResult.data;
        
        RenameTMPLabel(question.question_text, QuestionText);
        RenameTMPLabel(question.option_one, ButtonAText);
        RenameTMPLabel(question.option_two, ButtonBText);
        RenameTMPLabel(question.option_three, ButtonCText);
        RenameTMPLabel(question.option_four, ButtonDText);
        //RenameTMPLabel(question.option_five, ButtonEText);
    }

    void AnswerButtonClicked(int option) {
        Debug.Log("Button Clicked: " + option);
    }
    public void GenerateRequest()
    {
        StartCoroutine(ProcessRequest(URL));
    }

    private IEnumerator ProcessRequest(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(UnityWebRequest.Get(url+"question/SM3AlYWKEDIrOo02UJtl"));
            }
        }
    }
}
