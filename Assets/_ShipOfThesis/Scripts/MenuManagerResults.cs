using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class MenuManagerResults : MenuEssentials
{ 
    [Serializable]
    class JSON_Out
    {
            public Sailboat.Snapshot[] data;
    }
    
    private String validChars = "ABCDEFGHJKLMNPQRSTUVWXYZ1234567890";
    public TextMeshProUGUI id, simpleText, complexText, copiedText;

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
        copiedText.gameObject.SetActive(false);
        id.text = GetUniqueID();
        simpleText.text = GetResults(true);
        complexText.text = GetResults(false);
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
            Debug.Log("Results Obtained: ");
            Debug.Log(data);

            string acc = " Practice Time: " + String.Format("{0,-10}", data[0]) + "\n" +
                         "K/X/P: " + data[11] + "/" + data[12] + "/" + data[13] + "\n\n" +
                         "         Leg 1: " + String.Format("{0,-10}", data[1]) + "\n" +
                         "         Leg 2: " + String.Format("{0,-10}", data[2]) + "\n" +
                         "         Leg 3: " + String.Format("{0,-10}", data[3]) + "\n" +
                         "         Leg 4: " + String.Format("{0,-10}", data[4]) + "\n" +
                         "K/X/P: " + data[14] + "/" + data[15] + "/" + data[16] + "\n\n" +
                         "        Pauses: " + String.Format("{0,-10}", data[5] + "/" + data[6]) + "\n" +
                         "Control Checks: " + String.Format("{0,-10}", data[7] + "/" + data[8]) + "\n" +
                         "       Crashes: " + String.Format("{0,-10}", data[9] + "/" + data[10]) + "\n" +
                         "        Resets: " + String.Format("{0,-10}", data[17]);
            return acc;
    }

    public void JSONtoClipboard()
    {
            JSON_Out json = new JSON_Out();
            json.data = StudyManager.Instance.GetAccuracy();
            
            GUIUtility.systemCopyBuffer = JsonUtility.ToJson(json);
            StartCoroutine(ShowCopiedText());
    }

    IEnumerator ShowCopiedText()
    {
            LeanTween.cancel(copiedText.gameObject);
            copiedText.alpha = 1f;
            copiedText.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1f);
            LeanTween.textAlpha((RectTransform) copiedText.transform, 0f, 2);
            yield return new WaitForSecondsRealtime(2f);
            copiedText.gameObject.SetActive(false);
    }
            
    protected override void SelectDefaultMenuElement()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
