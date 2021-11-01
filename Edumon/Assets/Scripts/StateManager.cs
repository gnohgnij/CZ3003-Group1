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

    public static bool editGymStatusTag = false;
    public static string editGymStatusMessage = "";

    public static bool studentProfileStatusTag = false;
    public static string studentProfileStatusMessage = "";

    public static bool studentHomeStatusTag = false;
    public static string studentHomeStatusMessage = "";

    public static int questionIndex = 0;
    public static int assignmentQuestionSize = 0;
    public static Question[] assignmentQuestions;
    public static string assignmentId;

    public static string[] gymId = new string[] { "2AcLYXxjcwUH8M6pS4kZ", "kz6hrs41oWb7tRHalQei", "Ma8MvuUvTeEHwLr7cPKd", "CJlAjApLDPzKPDZAYLVk", "O2zaxmG5w0wWnH5UkjlV" };
    public static int gymQustionIndex = 0;
    public static int gymQuestionsSize = 0;
    public static Question[] gymQuestions;

    public static string currentGym = "";
}
