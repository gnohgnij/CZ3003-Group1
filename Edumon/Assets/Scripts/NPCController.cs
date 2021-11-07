using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    private bool ManageDialog = false;
    private static string URL = StateManager.apiUrl;
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
            if (StateManager.currentGym == "Gym1") {
                StartCoroutine(LaunchGymFromGymId(StateManager.gymIdList[0]));
            } else if (StateManager.currentGym == "Gym2") {
                StartCoroutine(LaunchGymFromGymId(StateManager.gymIdList[1]));
            } else if (StateManager.currentGym == "Gym3") {
                StartCoroutine(LaunchGymFromGymId(StateManager.gymIdList[2]));
            } else if (StateManager.currentGym == "Gym4") {
                StartCoroutine(LaunchGymFromGymId(StateManager.gymIdList[3]));
            }
        }
    }

    private void LaunchGymBattle(Gym g) {
        StateManager.gymQuestionList = g.question_list;
        StateManager.gymQuestionsSize = g.question_list.Length;
        StateManager.gymId = g.gym_id;
        StateManager.mcqQuestionGroup = "Gym";
        Debug.Log("Loading Gym Battle with " + StateManager.gymQuestionsSize + " questions");
        SceneManager.LoadScene("MCQSingle");
    }

    private IEnumerator LaunchGymFromGymId(string gymId) {
        string gymUrl = URL + "gym/" + gymId;
        Debug.Log("Retrieving gym from: " + gymUrl);
        using (UnityWebRequest gymRequest = UnityWebRequest.Get(gymUrl)) {
            yield return gymRequest.SendWebRequest();

            if (gymRequest.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log(gymRequest.error);
            } else {
                GymResult gymResult = JsonUtility.FromJson<GymResult>(gymRequest.downloadHandler.text);
                Gym gym = new Gym();
                gym = gymResult.data;
                LaunchGymBattle(gym);
            }
        }
    }
}