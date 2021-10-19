using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StudentHomeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Profile_Page_Clicked()
    {
        SceneManager.LoadScene("StudentProfile");
    }

    public void Btn_Logout_Clicked()
    {
        StateManager.user = null;
        SceneManager.LoadScene("MainMenu");
    }
}
