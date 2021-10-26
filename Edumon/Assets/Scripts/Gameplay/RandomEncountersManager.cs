using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RandomEncountersManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowEncounter;
    public event Action OnCloseEncouter;

    public static RandomEncountersManager Instance {get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    Dialog dialog;
    bool isTyping;

    public IEnumerator ShowEncounter(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();

        OnShowEncounter?.Invoke();

        this.dialog = dialog;
        dialogBox.SetActive(true);

        int count = dialog.Lines.Count;
        int num = UnityEngine.Random.Range(0, count);

        StartCoroutine(TypeEncounter(dialog.Lines[num]));
    }

    public IEnumerator TypeEncounter(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach(var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }

    public void HandleUpdate()
    {
        if(Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            dialogBox.SetActive(false);
            OnCloseEncouter?.Invoke();
        }
    }
}