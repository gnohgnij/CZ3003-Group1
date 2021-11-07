using System;
using System.Collections.Generic;

[Serializable]
public class AttemptModel
{
    public string attempt_id;
    public string question_set;
    public string completion_status;
    public string user_email;
    public string question_group;
    public int score;
    public Dictionary<string, int> user_answers;
}
