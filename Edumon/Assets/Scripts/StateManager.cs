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

    public static bool challengeStatusTag = false;
    public static string challengeStatusMessage = "";

    public static int questionIndex = 0;
    public static int assignmentQuestionSize = 0;
    public static Question[] assignmentQuestions;
    public static string assignmentId;

    public static string[] gymIdList = {"DeDnjKAqlzb5cKOJrIzk", "9vF4uGW4b7oop1EUK1Sm", "IFBMLqL6Mb16fagAChaO", "bcO5PTXYNsHaREccZovg"};
    public static int gymQustionIndex = 0;
    public static int gymQuestionsSize = 0;
    public static Question[] gymQuestions;
    public static string gymId;
    public static string[] gymQuestionList;
    public static string currentGym = "";
    public static string[] challengeQuestionList;

    public static int challengeQuestionIndex = 0;
    public static int challengeQuestionSize = 0;
    public static Question[] challengeQuestions;
    public static string challengeId;
    public static string mcqQuestionGroup;
}