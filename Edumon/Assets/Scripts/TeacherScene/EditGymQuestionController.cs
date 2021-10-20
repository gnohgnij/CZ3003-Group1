using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditGymQuestionController : MonoBehaviour
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
        SceneManager.LoadScene("TeacherHome");
    }
}
