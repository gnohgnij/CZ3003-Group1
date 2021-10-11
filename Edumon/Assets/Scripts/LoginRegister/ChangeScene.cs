using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Btn_ChangeScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}
