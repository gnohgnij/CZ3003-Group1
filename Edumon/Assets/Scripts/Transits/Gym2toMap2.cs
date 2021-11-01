using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gym2toMap2 : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Here");

            StateManager.currentGym = "";

            SceneManager.LoadScene("Map2"); 
        }
    }
}
