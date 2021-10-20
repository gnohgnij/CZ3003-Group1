using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gym1toMap1 : MonoBehaviour
{
    private float xPos;
    private float yPos;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // xPos = GameObject.FindGameObjectWithTag("Player").transform.position.x; // get player current position
            // PlayerPrefs.SetFloat("SavedXPosition", xPos); 
            // yPos = GameObject.FindGameObjectWithTag("Player").transform.position.y; // get player current position
            // PlayerPrefs.SetFloat("SavedYPosition", yPos - 1); 
            // Debug.Log(PlayerPrefs.GetFloat("SavedXPosition"));
            // Debug.Log(PlayerPrefs.GetFloat("SavedYPosition"));
            SceneManager.LoadScene("Map1"); 
        }
    }
}
