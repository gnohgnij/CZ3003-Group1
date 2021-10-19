using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField EmailInputField;
    public InputField PasswordInputField;
    public Text WarningText;

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        string url = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyA0R4_B-i562Bv6kX9vDwZdBuVgk0RmcAA";
        StartCoroutine(Login(url, EmailInputField.text, PasswordInputField.text));
    }

    private IEnumerator Login(string _url, string _email, string _password)
    {
        WarningText.gameObject.SetActive(true);

        WWWForm form = new WWWForm();
        form.AddField("email", _email);
        form.AddField("password", _password);
        form.AddField("returnSecureToken", "true");

        UnityWebRequest uwr = UnityWebRequest.Post(_url, form);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            WarningText.text = "Network Error";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            AuthResult authResult = JsonUtility.FromJson<AuthResult>(uwr.downloadHandler.text);
            if (authResult.localId == null)
            {
                WarningText.text = "Wrong email/password";
                WarningText.gameObject.SetActive(true);
            }
            else
            {
                //string accountUrl = StateManager.apiUrl + "account/" + authResult.localId;

                string accountUrl = StateManager.localhostUrl + "account/" + authResult.localId;
                
                UnityWebRequest accUwr = UnityWebRequest.Get(accountUrl);
                yield return accUwr.SendWebRequest();

                if (accUwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    WarningText.text = "Network Error";
                    WarningText.gameObject.SetActive(true);
                }
                else
                {
                    UserResult userResult = JsonUtility.FromJson<UserResult>(accUwr.downloadHandler.text);
                    if (userResult.status == "fail")
                    {
                        WarningText.text = "Account Error. Please contact the admin";
                        WarningText.gameObject.SetActive(true);
                    }
                    else
                    {
                        StateManager.user = userResult.data;
                        StateManager.user.email = _email;
                        if (StateManager.user.type == "teacher")
                        {
                            SceneManager.LoadScene("TeacherHome");
                        }
                        else
                        {
                            SceneManager.LoadScene("StudentHome");
                        }
                    }
                }
            }
        }
    }
}