using System;

[Serializable]
public class AuthResult
{
    public Error error;
    public string localId;
    public string idToken;
}

[Serializable]
public class Error
{
    public int code;
    public string message;
}