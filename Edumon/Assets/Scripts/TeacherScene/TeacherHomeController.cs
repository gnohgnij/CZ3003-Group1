using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeacherHomeController : MonoBehaviour
{
    public void Btn_Logout_Clicked()
    {
        StateManager.user = null;
        SceneManager.LoadScene("MainMenu");
    }

    public void Btn_Profile_Page_Clicked()
    {
        SceneManager.LoadScene("TeacherProfile");
    }
}
