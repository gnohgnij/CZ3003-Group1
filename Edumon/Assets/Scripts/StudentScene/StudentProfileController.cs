using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StudentProfileController : MonoBehaviour
{
    public Text Username;
    public Text StudentId;
    public Text Email;

    // Start is called before the first frame update
    void Start()
    {
        Username.text = StateManager.user.username;
        StudentId.text = StateManager.user.studentid;
        Email.text = StateManager.user.email;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Back_Clicked()
    {
        SceneManager.LoadScene("StudentHome");
    }

    public void Btn_Edit_Profile_Clicked()
    {
        SceneManager.LoadScene("StudentEditProfile");
    }
}
