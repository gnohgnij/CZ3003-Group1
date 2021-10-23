using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class EditGymQuestionsController : MonoBehaviour
{
    public Text Message;

    // Start is called before the first frame update
    void Start()
    {
        if (StateManager.editGymStatusTag)
        {
            Message.text = StateManager.editGymStatusMessage;
            Message.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Disable_Tag()
    {
        StateManager.editGymStatusTag = false;
        Message.gameObject.SetActive(false);
    }

    public void Btn_Back_Clicked()
    {
        Disable_Tag();
        SceneManager.LoadScene("TeacherHome");
    }

    public void Btn_Edit_Gym_1_Questions_Clicked()
    {
        Message.gameObject.SetActive(false);

        StateManager.gymQustionIndex = 0;
        StateManager.gymQuestionsSize = 0;
        StateManager.gymQuestions = null;

        StartCoroutine(Retrieve_Gym_Data(0));
    }

    public void Btn_Edit_Gym_2_Questions_Clicked()
    {
        Message.gameObject.SetActive(false);

        StateManager.gymQustionIndex = 0;
        StateManager.gymQuestionsSize = 0;
        StateManager.gymQuestions = null;

        StartCoroutine(Retrieve_Gym_Data(1));
    }

    public void Btn_Edit_Gym_3_Questions_Clicked()
    {
        Message.gameObject.SetActive(false);

        StateManager.gymQustionIndex = 0;
        StateManager.gymQuestionsSize = 0;
        StateManager.gymQuestions = null;

        StartCoroutine(Retrieve_Gym_Data(2));
    }

    public void Btn_Edit_Gym_4_Questions_Clicked()
    {
        Message.gameObject.SetActive(false);

        StateManager.gymQustionIndex = 0;
        StateManager.gymQuestionsSize = 0;
        StateManager.gymQuestions = null;

        StartCoroutine(Retrieve_Gym_Data(3));
    }

    public void Btn_Edit_Gym_5_Questions_Clicked()
    {
        Message.gameObject.SetActive(false);

        StateManager.gymQustionIndex = 0;
        StateManager.gymQuestionsSize = 0;
        StateManager.gymQuestions = null;

        StartCoroutine(Retrieve_Gym_Data(4));
    }

    private IEnumerator Retrieve_Gym_Data(int _index)
    {
        bool stop = false;
        GymResult gymResult = null;

        // get gym data first
        string gymUrl = StateManager.apiUrl + "gym/" + StateManager.gymId[_index];

        UnityWebRequest gymUwr = UnityWebRequest.Get(gymUrl);
        yield return gymUwr.SendWebRequest();

        if (gymUwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Message.text = "Network Error";
            Message.gameObject.SetActive(true);
            stop = true;
        }
        else
        {
            gymResult = JsonUtility.FromJson<GymResult>(gymUwr.downloadHandler.text);
            if (gymResult.status == "fail")
            {
                Message.text = "Unable to get gym data. Please contact the admin\nError: " + gymResult.message;
                Message.gameObject.SetActive(true);
                stop = true;
            }
            else
            {
                StateManager.gymQuestionsSize = gymResult.data.question_count;
                StateManager.gymQuestions = new Question[StateManager.gymQuestionsSize];
            }
        }


        // get questions
        if (!stop)
        {
            for (int i = 0; i < StateManager.gymQuestions.Length; i++)
            {
                string questionUrl = StateManager.apiUrl + "question/" + gymResult.data.question_list[i];

                UnityWebRequest questionUwr = UnityWebRequest.Get(questionUrl);
                yield return questionUwr.SendWebRequest();

                if (questionUwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    Message.text = "Network Error";
                    Message.gameObject.SetActive(true);
                    stop = true;
                }
                else
                {
                    QuestionGetResult questionResult = JsonUtility.FromJson<QuestionGetResult>(questionUwr.downloadHandler.text);
                    if (questionResult.status == "fail")
                    {
                        Message.text = "Unable to get question data. Please contact the admin\nError: " + questionResult.message;
                        Message.gameObject.SetActive(true);
                        stop = true;
                    }
                    else
                    {
                        StateManager.gymQuestions[i] = questionResult.data;
                    }
                }
            }
            if (!stop) SceneManager.LoadScene("EditQuestion");
        }
    }
}
