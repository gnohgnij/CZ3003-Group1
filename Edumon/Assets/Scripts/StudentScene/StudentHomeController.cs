using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StudentHomeController : MonoBehaviour
{
    public Text Message;

    // Start is called before the first frame update
    void Start()
    {
        if (StateManager.studentHomeStatusTag)
        {
            Message.text = StateManager.studentHomeStatusMessage;
            Message.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Disable_Tag()
    {
        StateManager.studentHomeStatusTag = false;
        Message.gameObject.SetActive(false);
    }

    public void Btn_Profile_Page_Clicked()
    {
        Disable_Tag();
        SceneManager.LoadScene("StudentProfile");
    }

    public void Btn_Logout_Clicked()
    {
        Disable_Tag();
        StateManager.user = null;
        SceneManager.LoadScene("MainMenu");
    }

    public void Btn_Enter_World_Clicked()
    {
        Disable_Tag();
        StartCoroutine(Get_Unlocked_Map());
    }

    public void Btn_View_Leaderboard_Clicked()
    {
        Disable_Tag();
        SceneManager.LoadScene("Leaderboard");
    }

    public void Btn_Attempt_Assignment_Clicked()
    {
        Disable_Tag();
        StateManager.questionIndex = 0;
        StateManager.assignmentQuestionSize = 0;
        StateManager.assignmentQuestions = null;
        SceneManager.LoadScene("AttemptAssignment");
    }

    public IEnumerator Get_Unlocked_Map()
    {
        string accountUrl = StateManager.apiUrl + "account/" + StateManager.user.uid;

        UnityWebRequest accUwr = UnityWebRequest.Get(accountUrl);
        yield return accUwr.SendWebRequest();

        if (accUwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Message.text = "Network Error";
            Message.gameObject.SetActive(true);
        }
        else
        {
            UserResult userResult = JsonUtility.FromJson<UserResult>(accUwr.downloadHandler.text);
            if (userResult.status == "fail")
            {
                Message.text = "Account Error. Please contact the admin";
                Message.gameObject.SetActive(true);
            }
            else
            {
                string password = StateManager.user.password;
                StateManager.user = userResult.data;
                StateManager.user.password = password;
                SceneManager.LoadScene("WorldSelection");
            }
        }
    }
}
