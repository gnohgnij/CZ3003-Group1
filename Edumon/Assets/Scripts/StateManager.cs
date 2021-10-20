using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static User user;
    public static bool showRegistered = false;
    public static string apiUrl = "https://cz3003-edumon.herokuapp.com/";
    public static string localhostUrl = "http://127.0.0.1:3000/";

    public static bool teacherProfileStatusTag = false;
    public static string teacherProfileStatusMessage = "";
    public static bool teacherHomeStatusTag = false;
    public static string teacherHomeStatusMessage = "";
    public static bool studentProfileStatusTag = false;
    public static string studentProfileStatusMessage = "";

    public static int questionIndex = 0;
    public static int assignmentQuestionSize = 0;
    public static Question[] assignmentQuestions;
}
