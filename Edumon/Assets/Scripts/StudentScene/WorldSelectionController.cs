using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldSelectionController : MonoBehaviour
{
    public Button World2Btn;
    public Button World3Btn;
    public Button World4Btn;
    public Button World5Btn;

    // Start is called before the first frame update
    void Start()
    {
        if (Array.IndexOf(StateManager.user.unlocked_map, "Map2") > 0) World2Btn.gameObject.SetActive(true);
        if (Array.IndexOf(StateManager.user.unlocked_map, "Map3") > 0) World3Btn.gameObject.SetActive(true);
        if (Array.IndexOf(StateManager.user.unlocked_map, "Map4") > 0) World4Btn.gameObject.SetActive(true);
        if (Array.IndexOf(StateManager.user.unlocked_map, "Map5") > 0) World5Btn.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_World1_Clicked()
    {
        SceneManager.LoadScene("Map1");
    }

    public void Btn_World2_Clicked()
    {
        SceneManager.LoadScene("Map2");
    }

    public void Btn_World3_Clicked()
    {
        SceneManager.LoadScene("Map3");
    }

    public void Btn_World4_Clicked()
    {
        SceneManager.LoadScene("Map4");
    }

    public void Btn_World5_Clicked()
    {
        //SceneManager.LoadScene("Map5");
    }

    public void Btn_Challenge_World_Clicked()
    {
        //SceneManager.LoadScene("");
    }

    public void Btn_Back_Clicked()
    {
        SceneManager.LoadScene("StudentHome");
    }
}
