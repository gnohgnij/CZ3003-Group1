using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeacherProfileController : MonoBehaviour
{
    public Text username;
    public Text email;
    public Text Message;

    // Start is called before the first frame update
    void Start()
    {
        username.text = StateManager.user.username;
        email.text = StateManager.user.email;
        if (StateManager.teacherProfileStatusTag)
        {
            Message.gameObject.SetActive(true);
            Message.text = StateManager.teacherProfileStatusMessage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Back_Clicked()
    {
        SceneManager.LoadScene("TeacherHome");
    }

    public void Btn_Edit_Profile_Clicked()
    {
        SceneManager.LoadScene("TeacherEditProfile");
    }
}
