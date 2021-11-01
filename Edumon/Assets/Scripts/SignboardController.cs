using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignboardController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    public void Interact()
    {
        Debug.Log("Interacting with signboard");
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }
}