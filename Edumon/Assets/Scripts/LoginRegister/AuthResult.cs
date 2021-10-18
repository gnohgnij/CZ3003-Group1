using System;

[Serializable]
public class AuthResult
{
    public Error error;
    public string localId;
}

[Serializable]
public class Error
{
    public int code;
}