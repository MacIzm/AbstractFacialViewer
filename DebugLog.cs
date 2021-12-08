using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugLog : MonoBehaviour
{

    TextMesh textMesh;

    void Start()
    {
        textMesh = gameObject.GetComponentInChildren<TextMesh>();
    }


    void OnEnable()
    {
        Application.logMessageReceived += LogMessage;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogMessage;
    }

    public void LogMessage(string message, string stackTrace, LogType type)
    {
        try
        {
            if (textMesh.text.Length > 300)
            {
                textMesh.text = message + "\n";
            }
            else
            {
                textMesh.text += message + "\n";
            }
        }
        catch(Exception e)
        {
            //ignore
        }
        
        // Start();
    }


}