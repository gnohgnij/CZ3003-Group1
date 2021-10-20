using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map1toMap3 : MonoBehaviour
{
    private float xPos;
    private float yPos;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            xPos = GameObject.FindGameObjectWithTag("Player").transform.position.x; // get player current position
            PlayerPrefs.SetFloat("Saved1XPosition", xPos); 
            yPos = GameObject.FindGameObjectWithTag("Player").transform.position.y; // get player current position
            PlayerPrefs.SetFloat("Saved1YPosition", yPos + 1); 
            Debug.Log(PlayerPrefs.GetFloat("Saved1XPosition"));
            Debug.Log(PlayerPrefs.GetFloat("Saved1YPosition"));
            SceneManager.LoadScene("Map3"); 
        }
    }
}
