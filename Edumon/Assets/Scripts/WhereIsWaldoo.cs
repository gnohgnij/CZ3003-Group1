using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhereIsWaldoo : MonoBehaviour
{
    public int mapNumber;

    // /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    // /// Start function here invokes don't destroy variable when transiting scenes
    // /// If current scene is "Introduction" scene then destroy game object "Variable"
    // void Start() {
    //     DontDestroyOnLoad(transform.gameObject); 
    // } 

    // /// Awake is called when the script instance is being loaded.
    // /// Awake function is invoked if scene is "Introduction" scene then destroy game object "Variable"
    // void Awake() {
    //     if (SceneManager.GetActiveScene ().name == "Gym1" || SceneManager.GetActiveScene ().name == "Gym2" || 
    //         SceneManager.GetActiveScene ().name == "Gym3" || SceneManager.GetActiveScene ().name == "Gym4") 
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }

    // /// Update is called every frame, if the MonoBehaviour is enabled.
    // /// Update function is invoked if scene is "Introduction" scene then destroy game object "Variable"
    // void Update() {
    //     if (SceneManager.GetActiveScene ().name == "Gym1" || SceneManager.GetActiveScene ().name == "Gym2" || 
    //         SceneManager.GetActiveScene ().name == "Gym3" || SceneManager.GetActiveScene ().name == "Gym4") 
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }
}
