using System;

[Serializable]
public class Assignment
{
    public string assignment_id;
    public int question_count;
    public string deadline;
    public System.DateTime deadlineDateTime;
    public string[] question_list;
}
