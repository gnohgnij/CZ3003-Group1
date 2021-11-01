using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    private bool ManageDialog;
    public void Interact()
    {
        Debug.Log("Interacting with NPC");
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        ManageDialog = true;
    }

    public void Update() 
    {
        // if (ManageDialog && Input.GetKeyDown(KeyCode.E) && SceneManager.GetActiveScene().name == "Gym5") 
        // {
        //     SceneManager.LoadScene("AssignmentChallengeView");
        // }

        if (ManageDialog && Input.GetKeyDown(KeyCode.E) && StateManager.currentGym == "Gym5") 
        {
            SceneManager.LoadScene("AssignmentChallengeView");
        }
    }
}
