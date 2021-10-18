using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Text RegisteredText;

    void Awake()
    {
        RegisteredText.gameObject.SetActive(StateManager.showRegistered);
    }

    public void Btn_Login_Clicked()
    {
        StateManager.showRegistered = false;
        SceneManager.LoadScene("Login");
    }

    public void Btn_Register_Clicked()
    {
        StateManager.showRegistered = false;
        SceneManager.LoadScene("Register");
    }
}
