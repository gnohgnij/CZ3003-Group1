using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map3toMap1 : MonoBehaviour
{
    private float xPos;
    private float yPos;
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            xPos = GameObject.FindGameObjectWithTag("Player").transform.position.x; // get player current position
            PlayerPrefs.SetFloat("Saved3XPosition", xPos); 
            yPos = GameObject.FindGameObjectWithTag("Player").transform.position.y; // get player current position
            PlayerPrefs.SetFloat("Saved3YPosition", yPos - 1); 
            PlayerPrefs.SetFloat("Saved1XPosition", (float)0.474);
            PlayerPrefs.SetFloat("Saved1YPosition", (float)-6.162);
            Debug.Log(PlayerPrefs.GetFloat("Saved3XPosition"));
            Debug.Log(PlayerPrefs.GetFloat("Saved3YPosition"));
            SceneManager.LoadScene("Map1"); 
        }
    }
}
