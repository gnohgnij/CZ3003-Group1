using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class TeacherEditProfileController : MonoBehaviour
{
    public InputField UsernameInputField;
    public InputField EmailInputField;
    public InputField PasswordInputField;
    public InputField RePasswordInputField;
    public Text WarningText;

    // Start is called before the first frame update
    void Start()
    {
        WarningText.gameObject.SetActive(false);
        UsernameInputField.text = StateManager.user.username;
        EmailInputField.text = StateManager.user.email;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Back_Clicked()
    {
        SceneManager.LoadScene("TeacherProfile");
    }

    public void Btn_Edit_Clicked()
    {
        WarningText.gameObject.SetActive(false);
        StartCoroutine(EditProfile(UsernameInputField.text, EmailInputField.text, PasswordInputField.text, RePasswordInputField.text));
    }

    private IEnumerator EditProfile(string _username, string _email, string _password, string _rePassword)
    { 
        if (string.IsNullOrWhiteSpace(_username) || string.IsNullOrWhiteSpace(_email))
        {
            WarningText.text = "Username/Email cannot be empty";
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            bool stop = false;
            AuthResult authResult = null;

            // Change username
            string changeUsernameUrl = StateManager.localhostUrl + "account/username";
            WWWForm usernameForm = new WWWForm();
            usernameForm.AddField("id", StateManager.user.uid);
            usernameForm.AddField("username", _username);
            usernameForm.AddField("studentId", "");

            UnityWebRequest usernameUwr = UnityWebRequest.Post(changeUsernameUrl, usernameForm);
            yield return usernameUwr.SendWebRequest();

            if (usernameUwr.result == UnityWebRequest.Result.ConnectionError)
            {
                WarningText.text = "Network Error";
                WarningText.gameObject.SetActive(true);
            }
            else
            {
                UserResult userResult = JsonUtility.FromJson<UserResult>(usernameUwr.downloadHandler.text);
                if (userResult.status == "success")
                {
                    StateManager.user.username = _username;
                }
                else
                {
                    WarningText.text = "Unable to change username. Please contact the admin";
                    WarningText.gameObject.SetActive(true);
                    stop = true;
                }
                
            }



            // Login to change email/password
            if (!stop)
            {
                string loginUrl = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyA0R4_B-i562Bv6kX9vDwZdBuVgk0RmcAA";
                WWWForm loginForm = new WWWForm();
                loginForm.AddField("email", StateManager.user.email);
                loginForm.AddField("password", StateManager.user.password);
                loginForm.AddField("returnSecureToken", "true");

                UnityWebRequest loginUwr = UnityWebRequest.Post(loginUrl, loginForm);
                yield return loginUwr.SendWebRequest();

                if (loginUwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    WarningText.text = "Network Error";
                    WarningText.gameObject.SetActive(true);
                }
                else
                {
                    authResult = JsonUtility.FromJson<AuthResult>(loginUwr.downloadHandler.text);
                    if (authResult.localId == null)
                    {
                        WarningText.text = "Unable to login. Please contact the admin";
                        WarningText.gameObject.SetActive(true);
                        stop = true;
                    }

                }
            }
            


            // Change email
            if (!stop)
            {
                string updateEmailUrl = "https://identitytoolkit.googleapis.com/v1/accounts:update?key=AIzaSyA0R4_B-i562Bv6kX9vDwZdBuVgk0RmcAA";
                WWWForm updateEmailForm = new WWWForm();
                updateEmailForm.AddField("idToken", authResult.idToken);
                updateEmailForm.AddField("email", _email);
                updateEmailForm.AddField("returnSecureToken", "true");

                UnityWebRequest updateEmailUwr = UnityWebRequest.Post(updateEmailUrl, updateEmailForm);
                yield return updateEmailUwr.SendWebRequest();

                if (updateEmailUwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    WarningText.text = "Network Error";
                    WarningText.gameObject.SetActive(true);
                }
                else
                {
                    AuthResult tempResult = JsonUtility.FromJson<AuthResult>(updateEmailUwr.downloadHandler.text);
                    if (authResult.error.code == 0)
                    {
                        StateManager.user.email = _email;
                    }
                    else
                    {
                        if (tempResult.idToken != null)
                        {
                            authResult = JsonUtility.FromJson<AuthResult>(updateEmailUwr.downloadHandler.text);
                        }
                        WarningText.text = "Error: " + authResult.error.message;
                        WarningText.gameObject.SetActive(true);
                        stop = true;
                    }
                }
            }
            


            // Change password
            if (!stop)
            {
                if (!(string.IsNullOrWhiteSpace(_password) && string.IsNullOrWhiteSpace(_rePassword)))
                {
                    if (_password != _rePassword)
                    {
                        WarningText.text = "Password does not match";
                        WarningText.gameObject.SetActive(true);
                    }
                    else if (!(_password.Any(char.IsUpper) && _password.Any(char.IsLower) && _password.Any(char.IsDigit) && _password.Any(char.IsSymbol)))
                    {
                        WarningText.text = "Please ensure that your password contains\nuppercase, lowercase, number, and symbols";
                        WarningText.gameObject.SetActive(true);
                    }
                    else if (_password.Length < 12)
                    {
                        WarningText.text = "Password is less than 12 characters long";
                        WarningText.gameObject.SetActive(true);
                    }
                    else
                    {
                        string updatePasswordUrl = "https://identitytoolkit.googleapis.com/v1/accounts:update?key=AIzaSyA0R4_B-i562Bv6kX9vDwZdBuVgk0RmcAA";
                        WWWForm updatePasswordForm = new WWWForm();
                        updatePasswordForm.AddField("idToken", authResult.idToken);
                        updatePasswordForm.AddField("password", _password);
                        updatePasswordForm.AddField("returnSecureToken", "true");

                        UnityWebRequest updatePasswordUwr = UnityWebRequest.Post(updatePasswordUrl, updatePasswordForm);
                        yield return updatePasswordUwr.SendWebRequest();

                        if (updatePasswordUwr.result == UnityWebRequest.Result.ConnectionError)
                        {
                            WarningText.text = "Network Error";
                            WarningText.gameObject.SetActive(true);
                        }
                        else
                        {
                            authResult = JsonUtility.FromJson<AuthResult>(updatePasswordUwr.downloadHandler.text);
                            if (authResult.error.code == 0)
                            {
                                StateManager.user.password = _password;
                                WarningText.text = "Successfully change username, email, and password";
                                WarningText.gameObject.SetActive(true);
                            }
                            else
                            {
                                WarningText.text = authResult.error.message;
                                WarningText.gameObject.SetActive(true);
                                stop = true;
                            }
                        }
                    }
                }
                else
                {
                    WarningText.text = "Successfully change username and email";
                    WarningText.gameObject.SetActive(true);
                }
            }
        }
    }
}