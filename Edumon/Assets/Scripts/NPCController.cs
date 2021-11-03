using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    private bool ManageDialog = false;
    public void Interact()
    {
        Debug.Log("Interacting with NPC");
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        ManageDialog = true;
    }
    
    public void Update() 
    {

        if (ManageDialog && Input.GetKeyDown(KeyCode.E) && StateManager.currentGym == "Gym5") 
        {
            SceneManager.LoadScene("ChallengeView");
        }

        else if (ManageDialog && Input.GetKeyDown(KeyCode.E) && (StateManager.currentGym == "Gym1" 
        || StateManager.currentGym == "Gym2" || StateManager.currentGym == "Gym3" || StateManager.currentGym == "Gym4")) 
        {
            SceneManager.LoadScene("MCQSingle");
        }
    }
}
