using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ViewChallengeController : MonoBehaviour
{
    public Text StatusMessage;

    // Start is called before the first frame update
    void Start()
    {
        if (StateManager.challengeStatusTag)
        {
            StatusMessage.text = StateManager.challengeStatusMessage;
            StatusMessage.gameObject.SetActive(true);
        }
    }

    private void Disable_Tag()
    {
        StateManager.challengeStatusTag = false;
        StatusMessage.gameObject.SetActive(false);
    }

    public void Btn_View_Assignment_Clicked()
    {
        Disable_Tag();
        SceneManager.LoadScene("CreateChallenge");
    }

    public void Btn_View_Challenge_Clicked()
    {
        Disable_Tag();
        SceneManager.LoadScene("Challenge");
    }

    public void Back_Btn_Clicked()
    {
        Disable_Tag();
        StateManager.currentGym = "Gym5";
        SceneManager.LoadScene("Gym5");
    }
}
