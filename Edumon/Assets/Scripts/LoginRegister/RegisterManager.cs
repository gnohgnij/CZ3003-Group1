using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;

public class RegisterManager : MonoBehaviour
{
    public InputField UsernameInputField;
    public InputField EmailInputField;
    public InputField PasswordInputField;
    public InputField RePasswordInputField;

    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
    }

    //Function for the login button
    public void RegisterButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Register(EmailInputField.text, PasswordInputField.text));
    }

    private IEnumerator Register(string _email, string _password)
    {
        if (PasswordInputField.text != RePasswordInputField.text)
        {
            //If the password does not match show a warning
            Debug.LogWarning(message: "Password Does Not Match!");
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        Debug.LogWarning(message: "Missing Email");
                        break;
                    case AuthError.MissingPassword:
                        Debug.LogWarning(message: "Missing Password");
                        break;
                    case AuthError.WeakPassword:
                        Debug.LogWarning(message: "Weak Password");
                        break;
                    case AuthError.EmailAlreadyInUse:
                        Debug.LogWarning(message: "Email Already In Use");
                        break;
                }
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;
                Debug.LogFormat("User created successfully: {0} ({1})", User.DisplayName, User.Email);
            }
        }
    }
}
