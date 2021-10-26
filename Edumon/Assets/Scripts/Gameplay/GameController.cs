using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {FreeRoam, Dialog, RandomEncounter}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    GameState state;

    private void Start()
    {
        DialogManager.Instance.OnShowDialog += () => 
        {
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnCloseDialog += () =>
        {
            if(state == GameState.Dialog)
                state = GameState.FreeRoam;
        };

        RandomEncountersManager.Instance.OnShowEncounter += () =>
        {
            state = GameState.RandomEncounter;
        };

        RandomEncountersManager.Instance.OnCloseEncouter += () =>
        {
            if(state == GameState.RandomEncounter)
                state = GameState.FreeRoam;
        };
    }

    private void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }

        else if(state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }

        else if(state == GameState.RandomEncounter)
        {
            RandomEncountersManager.Instance.HandleUpdate();
        }
    }
}