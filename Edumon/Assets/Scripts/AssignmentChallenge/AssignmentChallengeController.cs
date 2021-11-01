using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AssignmentChallengeController : MonoBehaviour
{
    public void Btn_View_Assignment_Clicked()
    {
        SceneManager.LoadScene("SetChallenge");
    }

    public void Btn_View_Challenge_Clicked()
    {
        SceneManager.LoadScene("Challenge");
    }
}
