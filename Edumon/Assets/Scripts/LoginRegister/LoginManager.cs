using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using static User;

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
                WarningText.text = "Wrong email/password";
                WarningText.gameObject.SetActive(true);
            }
            else
            {
                WarningText.text = "Login Successful";
                WarningText.gameObject.SetActive(true);
            }
        }
    }
}