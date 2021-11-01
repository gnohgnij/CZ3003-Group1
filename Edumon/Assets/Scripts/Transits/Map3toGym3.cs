using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map3toGym3 : MonoBehaviour
{
    private float xPos;
    private float yPos;
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            xPos = GameObject.FindGameObjectWithTag("Player").transform.position.x; // get player current position
            PlayerPrefs.SetFloat("Saved3XPosition", xPos); 
            yPos = GameObject.FindGameObjectWithTag("Player").transform.position.y; // get player current position
            PlayerPrefs.SetFloat("Saved3YPosition", yPos-1); 
            Debug.Log(PlayerPrefs.GetFloat("Saved3XPosition"));
            Debug.Log(PlayerPrefs.GetFloat("Saved3YPosition"));

            StateManager.currentGym = "Gym3";

            SceneManager.LoadScene("Gym3"); 
        }
    }
}
