using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using static User;

public class RegisterManager : MonoBehaviour
{
    public InputField UsernameInputField;
    public InputField EmailInputField;
    public InputField PasswordInputField;
    public InputField RePasswordInputField;
    public Text WarningText;

    //Function for the login button
    public void RegisterButton()
    {
        string url = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=AIzaSyA0R4_B-i562Bv6kX9vDwZdBuVgk0RmcAA";
        //Call the login coroutine passing the email and password
        StartCoroutine(Register(url, EmailInputField.text, PasswordInputField.text));
    }

    private IEnumerator Register(string _url, string _email, string _password)
    {
        WarningText.gameObject.SetActive(false);
        if (PasswordInputField.text != RePasswordInputField.text)
        {
            WarningText.text = "Password does not match";
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

            if (uwr.isNetworkError)
            {
                WarningText.text = "Network Error";
                WarningText.gameObject.SetActive(true);
            }
            else
            {
                User user = JsonUtility.FromJson<User>(uwr.downloadHandler.text);
                if (user.localId == null)
                {
                    if (user.error.code == 400 )
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
                    WarningText.text = "Register Successful";
                    WarningText.gameObject.SetActive(true);
                }
            }
        }
    }
}