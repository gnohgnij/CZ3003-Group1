using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map1toMap2 : MonoBehaviour
{
    private float xPos;
    private float yPos;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            xPos = GameObject.FindGameObjectWithTag("Player").transform.position.x; // get player current position
            PlayerPrefs.SetFloat("Saved1XPosition", xPos - 1); 
            yPos = GameObject.FindGameObjectWithTag("Player").transform.position.y; // get player current position
            PlayerPrefs.SetFloat("Saved1YPosition", yPos); 
            PlayerPrefs.SetFloat("Saved2XPosition", (float)-21.24);
            PlayerPrefs.SetFloat("Saved2YPosition", (float)-2.573);
            Debug.Log(PlayerPrefs.GetFloat("Saved1XPosition"));
            Debug.Log(PlayerPrefs.GetFloat("Saved1YPosition"));
            SceneManager.LoadScene("Map2"); 
        }
    }
}
