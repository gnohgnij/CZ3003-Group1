using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map4toGym4 : MonoBehaviour
{
    private float xPos;
    private float yPos;
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            xPos = GameObject.FindGameObjectWithTag("Player").transform.position.x; // get player current position
            PlayerPrefs.SetFloat("Saved4XPosition", xPos); 
            yPos = GameObject.FindGameObjectWithTag("Player").transform.position.y; // get player current position
            PlayerPrefs.SetFloat("Saved4YPosition", yPos-1); 
            Debug.Log(PlayerPrefs.GetFloat("Saved4XPosition"));
            Debug.Log(PlayerPrefs.GetFloat("Saved4YPosition"));

            StateManager.currentGym = "Gym4";

            SceneManager.LoadScene("Gym4"); 
        }
    }
}
