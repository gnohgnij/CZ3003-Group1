using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeacherHomeController : MonoBehaviour
{
    public Text StatusMessage;

    void Awake()
    {
        if (StateManager.teacherProfileStatusTag)
        {
            StatusMessage.text = StateManager.teacherProfileStatusMessage;
            StatusMessage.gameObject.SetActive(true);
        }
    }

    public void Btn_Create_New_Question_Clicked()
    {
        StateManager.teacherProfileStatusTag = false;
        StatusMessage.gameObject.SetActive(false);
        SceneManager.LoadScene("CreateNewQuestion");
    }

    public void Btn_Profile_Page_Clicked()
    {
        StateManager.teacherProfileStatusTag = false;
        StatusMessage.gameObject.SetActive(false);
        SceneManager.LoadScene("TeacherProfile");
    }

    public void Btn_Logout_Clicked()
    {
        StateManager.teacherProfileStatusTag = false;
        StatusMessage.gameObject.SetActive(false);
        StateManager.user = null;
        SceneManager.LoadScene("MainMenu");
    }
}
