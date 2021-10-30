using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map1toMap4 : MonoBehaviour
{
    private float xPos;
    private float yPos;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // player will only move to Map4 if it's unlocked
            if (Array.IndexOf(StateManager.user.unlocked_map, "Map4") > 0) {
                xPos = GameObject.FindGameObjectWithTag("Player").transform.position.x; // get player current position
                PlayerPrefs.SetFloat("Saved1XPosition", xPos); 
                yPos = GameObject.FindGameObjectWithTag("Player").transform.position.y; // get player current position
                PlayerPrefs.SetFloat("Saved1YPosition", yPos + 1); 
                PlayerPrefs.SetFloat("Saved4XPosition", (float)-8);
                PlayerPrefs.SetFloat("Saved4YPosition", (float)8.450);
                Debug.Log(PlayerPrefs.GetFloat("Saved1XPosition"));
                Debug.Log(PlayerPrefs.GetFloat("Saved1YPosition"));
                SceneManager.LoadScene("Map4");
            }
        }
    }
}
