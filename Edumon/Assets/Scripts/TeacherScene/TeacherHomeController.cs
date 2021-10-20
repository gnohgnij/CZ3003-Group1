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
        if (StateManager.teacherHomeStatusTag)
        {
            StatusMessage.text = StateManager.teacherHomeStatusMessage;
            StatusMessage.gameObject.SetActive(true);
        }
    }

    public void Btn_Edit_Gym_Questions_Clicked()
    {
        DisableTag();
        SceneManager.LoadScene("EditGymQuestions");
    }

    public void Btn_Set_Assignment_Clicked()
    {
        DisableTag();
        SceneManager.LoadScene("SetAssignment");
    }
    
    public void Btn_View_Summary_Clicked()
    {
        DisableTag();
        SceneManager.LoadScene("ViewSummary");
    }

    public void Btn_Profile_Page_Clicked()
    {
        DisableTag();
        SceneManager.LoadScene("TeacherProfile");
    }

    public void Btn_Logout_Clicked()
    {
        DisableTag();
        StateManager.user = null;
        SceneManager.LoadScene("MainMenu");
    }

    private void DisableTag()
    {
        StateManager.teacherProfileStatusTag = false;
        StatusMessage.gameObject.SetActive(false);
    }
}
