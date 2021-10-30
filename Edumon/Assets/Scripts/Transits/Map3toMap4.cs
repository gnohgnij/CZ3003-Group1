using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map3toMap4 : MonoBehaviour
{
    private float xPos;
    private float yPos;
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // player will only move to Map4 if it's unlocked
            //if (Array.IndexOf(StateManager.user.unlocked_map, "Map4") > 0) {
                xPos = GameObject.FindGameObjectWithTag("Player").transform.position.x; // get player current position
                PlayerPrefs.SetFloat("Saved3XPosition", xPos + 1); 
                yPos = GameObject.FindGameObjectWithTag("Player").transform.position.y; // get player current position
                PlayerPrefs.SetFloat("Saved3YPosition", yPos); 
                PlayerPrefs.SetFloat("Saved4XPosition", (float)18);
                PlayerPrefs.SetFloat("Saved4YPosition", (float)-7);
                Debug.Log(PlayerPrefs.GetFloat("Saved3XPosition"));
                Debug.Log(PlayerPrefs.GetFloat("Saved3YPosition"));
                SceneManager.LoadScene("Map4");
            //}
        }
    }
}
