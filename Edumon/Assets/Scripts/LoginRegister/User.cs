﻿using System;

[Serializable]
public class User
{
    public Error error;
    public string localId;
}

[Serializable]
public class Error
{
    public int code;
}
