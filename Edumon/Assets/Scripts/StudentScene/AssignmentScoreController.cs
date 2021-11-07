using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class AssignmentScoreController : MonoBehaviour
{
    public Text AssignmentScoreText;
    public Text WarningText;

    // Start is called before the first frame update
    void Start()
    {
        AssignmentScoreText.text = "";
        StartCoroutine(Retrieve_Attempt());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Back_Clicked()
    {
        StateManager.attemptAssignmentId = "";
        SceneManager.LoadScene("StudentHome");
    }

    private IEnumerator Retrieve_Attempt()
    {
        string attemptUrl = StateManager.apiUrl + "attempt/" + StateManager.attemptAssignmentId;

        UnityWebRequest attemptUwr = UnityWebRequest.Get(attemptUrl);
        yield return attemptUwr.SendWebRequest();

        if (attemptUwr.result == UnityWebRequest.Result.ConnectionError)
        {
            WarningText.text = "Network Error";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            AttemptGetResult attemptResult = JsonUtility.FromJson<AttemptGetResult>(attemptUwr.downloadHandler.text);
            if (attemptResult.status == "fail")
            {
                WarningText.text = "Unable to get attempt data. Please contact the admin\nError: " + attemptResult.message;
                WarningText.gameObject.SetActive(true);
            }
            else
            {
                AssignmentScoreText.text = "Your score is " + attemptResult.data.score.ToString() + " out of " + StateManager.assignmentQuestionSize.ToString();
            }
        }
    }
}
