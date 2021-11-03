using System;

[Serializable]
public class Challenge
{
    public string challenge_id;
    public int question_count;
    public DateTime deadline;
    public string[] question_list;
    public string from_email;
    public string to_email;
}
