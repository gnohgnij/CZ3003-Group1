using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
    public InputField UsernameInputField;
    public InputField StudentIdInputField;
    public InputField EmailInputField;
    public InputField PasswordInputField;
    public InputField RePasswordInputField;
    public Text WarningText;
    private string accountType = "student";

    public void On_AccountType_Value_Changed(int value)
    {
        if (value == 0)
        {
            accountType = "student";
        }
        else
        {
            accountType = "teacher";
        }
    }

    //Function for the login button
    public void RegisterButton()
    {
        // actual firebase
        string url = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=AIzaSyA0R4_B-i562Bv6kX9vDwZdBuVgk0RmcAA";
        
        // test firebase
        // string url = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=AIzaSyAjwveFghM1hgay8Vtbi7xegpEMdoyIjpE";
        
        //Call the login coroutine passing the email and password
        StartCoroutine(Register(url, UsernameInputField.text, StudentIdInputField.text, EmailInputField.text, PasswordInputField.text, RePasswordInputField.text));
    }

    private IEnumerator Register(string _url, string _username, string _studentId, string _email, string _password, string _rePassword)
    {
        Debug.Log(accountType == "student");
        Debug.Log(string.IsNullOrWhiteSpace(_studentId));
        WarningText.gameObject.SetActive(false);
        if ((accountType == "teacher" && 
            (string.IsNullOrWhiteSpace(_username) || 
            string.IsNullOrWhiteSpace(_email) || 
            string.IsNullOrWhiteSpace(_password) || 
            string.IsNullOrWhiteSpace(_rePassword))) || 
            (accountType == "student" && 
            (string.IsNullOrWhiteSpace(_username) ||
            string.IsNullOrWhiteSpace(_studentId) ||
            string.IsNullOrWhiteSpace(_email) ||
            string.IsNullOrWhiteSpace(_password) ||
            string.IsNullOrWhiteSpace(_rePassword))))
        {
            WarningText.text = "There are empty fields";
            WarningText.gameObject.SetActive(true);
        }
        else if (PasswordInputField.text != RePasswordInputField.text)
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
                    if (authResult.error.code == 400)
                    {
                        WarningText.text = "Email exists in database";
                        WarningText.gameObject.SetActive(true);
                    }
                    else
                    {
                        WarningText.text = "Unknown Error. Unable to Register";
                        WarningText.gameObject.SetActive(true);
                    }
                }
                else
                {
                    string accUrl = "https://cz3003-edumon.herokuapp.com/account";
                    
                    WWWForm form2 = new WWWForm();
                    form2.AddField("id", authResult.localId);
                    form2.AddField("username", _username);
                    form2.AddField("type", accountType);
                    if (accountType == "student")
                    {
                        form2.AddField("studentId", _studentId);
                    }
                    else
                    {
                        form2.AddField("studentId", "");
                    }
                    

                    UnityWebRequest accUwr = UnityWebRequest.Post(accUrl, form2);
                    yield return accUwr.SendWebRequest();
                    Debug.Log(accUwr.downloadHandler.text);

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
                            StateManager.showRegistered = true;
                            SceneManager.LoadScene("MainMenu");
                        }
                    }
                }
            }
        }
    }
}