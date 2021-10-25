using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map2toMap4 : MonoBehaviour
{
    private float xPos;
    private float yPos;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            xPos = GameObject.FindGameObjectWithTag("Player").transform.position.x; // get player current position
            PlayerPrefs.SetFloat("Saved2XPosition", xPos); 
            yPos = GameObject.FindGameObjectWithTag("Player").transform.position.y; // get player current position
            PlayerPrefs.SetFloat("Saved2YPosition", yPos + 1); 
            PlayerPrefs.SetFloat("Saved4XPosition", (float)1.453);
            PlayerPrefs.SetFloat("Saved4YPosition", (float)8.5);
            Debug.Log(PlayerPrefs.GetFloat("Saved2XPosition"));
            Debug.Log(PlayerPrefs.GetFloat("Saved2YPosition"));
            SceneManager.LoadScene("Map4"); 
        }
    }
}
