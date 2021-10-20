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
    public Text Message;

    // Start is called before the first frame update
    void Start()
    {
        Username.text = StateManager.user.username;
        StudentId.text = StateManager.user.studentid;
        Email.text = StateManager.user.email;
        if (StateManager.studentProfileStatusTag)
        {
            Message.gameObject.SetActive(true);
            Message.text = StateManager.studentProfileStatusMessage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Back_Clicked()
    {
        Disable_Tag();
        SceneManager.LoadScene("StudentHome");
    }

    public void Btn_Edit_Profile_Clicked()
    {
        Disable_Tag();
        SceneManager.LoadScene("StudentEditProfile");
    }

    private void Disable_Tag()
    {
        StateManager.studentProfileStatusTag = false;
        Message.gameObject.SetActive(false);
    }
}
