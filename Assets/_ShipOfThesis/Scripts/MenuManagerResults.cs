using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class MenuManagerResults : MenuEssentials
{
    private String validChars = "ABCDEFGHJKLMNPQRSTUVWXYZ1234567890";
    public TextMeshProUGUI id, simpleText, complexText;
    
    // Captured Data: 
    // System OS
    // Simple or Complex First
    
    // Practice time
    // Lap Times 1-4
    // Total Time
    // Crashes (Practice/Actual)
    // Pauses (Practice/Actual)
    // Control checks (Practice/Actual)
    // Retries (Actual Only)
    // Sail Accuracy (Complex Only)
    // Controller Time Distribution (K - Keyboard / X - Xbox / P = PS4)
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private String GetUniqueID()
    {
        string id;
#if UNITY_STANDALONE_WIN
        id = "W";
#elif UNITY_STANDALONE_OSX
        id = "M";
#else
        id = "U"
#endif
        id += StudyManager.Instance.simpleFirst ? "S" : "C";

        for (int i = 0; i < 4; i++)
                id += validChars.Substring((int) Random.Range(0,validChars.Length), 1);
        return id;
    }

    private String GetResults(bool simple)
    {
            String[] data = StudyManager.Instance.GetResults(simple);

            string acc = " Practice Time: " + String.Format($"{0,-10}", data[0]) + "\n" +
                         "         Leg 1: " + String.Format($"{0,-10}", data[1]) + "\n" +
                         "         Leg 2: " + String.Format($"{0,-10}", data[2]) + "\n" +
                         "         Leg 3: " + String.Format($"{0,-10}", data[3]) + "\n" +
                         "         Leg 4: " + String.Format($"{0,-10}", data[4]) + "\n" +
                         "        Pauses: " + String.Format($"{0,-10}", data[0]) + "\n" +
                         "Control Checks: " + String.Format($"{0,-10}", data[0]) + "\n" +
                         "       Crashes: " + String.Format($"{0,-10}", data[0]) + "\n";

            return "";
    }

    protected override void SelectDefaultMenuElement()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
