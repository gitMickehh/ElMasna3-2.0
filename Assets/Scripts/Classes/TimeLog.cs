using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeLog {


    public static void MyLog(string message)
    {
        Debug.Log(message + "\n" + System.DateTime.Now.ToString());
    }

}
