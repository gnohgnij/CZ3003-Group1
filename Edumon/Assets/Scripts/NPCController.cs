using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    public void Interact()
    {
        Debug.Log("Interacting with NPC");
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));

        if(StateManager.currentGym == "Gym5")
            SceneManager.LoadScene("AssignmentChallengeView");
    }
}
