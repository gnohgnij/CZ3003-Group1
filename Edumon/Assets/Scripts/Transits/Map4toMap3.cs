using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map4toMap3 : MonoBehaviour
{
    private float xPos;
    private float yPos;
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            xPos = GameObject.FindGameObjectWithTag("Player").transform.position.x; // get player current position
            PlayerPrefs.SetFloat("Saved4XPosition", xPos + 1); 
            yPos = GameObject.FindGameObjectWithTag("Player").transform.position.y; // get player current position
            PlayerPrefs.SetFloat("Saved4YPosition", yPos); 
            PlayerPrefs.SetFloat("Saved3XPosition", (float)17);
            PlayerPrefs.SetFloat("Saved3YPosition", (float)-7);
            Debug.Log(PlayerPrefs.GetFloat("Saved4XPosition"));
            Debug.Log(PlayerPrefs.GetFloat("Saved4YPosition"));
            SceneManager.LoadScene("Map3"); 
        }
    }
}
