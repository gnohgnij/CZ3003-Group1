using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ViewChallengeController : MonoBehaviour
{
    public void Btn_View_Assignment_Clicked()
    {
        SceneManager.LoadScene("CreateChallenge");
    }

    public void Btn_View_Challenge_Clicked()
    {
        SceneManager.LoadScene("Challenge");
    }
}
