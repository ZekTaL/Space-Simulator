using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that writes the console logs to file
/// </summary>
public class WriteConsoleToFile : MonoBehaviour
{
    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    /// <summary>
    /// Log the console messages to file
    /// </summary>
    /// <param name="_logString">Message content</param>
    /// <param name="_stackTrace">Stack trace of the message</param>
    /// <param name="_type">Type of log message</param>
    public void Log(string _logString, string _stackTrace, LogType _type)
    {
        if (_type == LogType.Log) return;

        string filePath = Application.dataPath + "/Logs"; // Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/GameLogs";
        Directory.CreateDirectory(filePath);

        string fileName = filePath + "/log.txt";

        //string fileName2 = filePath + "/log2.txt";
        //File.AppendAllText(fileName, "[" + DateTime.Now.ToString() + "] ");
        //File.AppendAllText(fileName, _logString + _stackTrace + "\n\n");

        StreamWriter file = new StreamWriter(fileName, true);

        try
        {
            //throw new IOException("IO FAIL!!!");
            file.Write("[" + DateTime.Now.ToString() + "] ");
            file.WriteLine(_logString);
        }
        catch (IOException IOex)
        {
            Debug.LogError(IOex);
            Application.logMessageReceived -= Log;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            Application.logMessageReceived -= Log;
        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }
}
