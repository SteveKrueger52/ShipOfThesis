using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEssentials : MonoBehaviour {

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
    
    public void StudyNext()
    {
        StudyManager._instance.Next();
    }
    
}
