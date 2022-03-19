using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MenuEssentials : MonoBehaviour
{

    public GameObject firstSelected;
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
    
    public void StudyNext()
    {
        StudyManager._instance.Next();
    }
    
    // private void OnEnable()
    // {
    //     StudyManager._instance.ControlsChanged += OnControlsChanged;
    //
    // }
    //
    // private void OnDisable()
    // {
    //     StudyManager._instance.ControlsChanged -= OnControlsChanged;
    // }

    public void OnControlsChanged()
    {
        if (StudyManager._instance.current == StudyManager.ControllerEnum.PC)
            EventSystem.current.SetSelectedGameObject(null);
        else 
            SelectDefaultMenuElement();
    }
    protected abstract void SelectDefaultMenuElement();
}
