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
        StudyManager.Instance.Next();
    }
    
    private void OnEnable()
    {
        PlayerInputWrapper.Instance.ControlsChanged += OnControlsChanged;
    
    }
    
    private void OnDisable()
    {
        PlayerInputWrapper.Instance.ControlsChanged -= OnControlsChanged;
    }

    public void OnControlsChanged()
    {
        if (PlayerInputWrapper.Instance.currentScheme == PlayerInputWrapper.ControllerEnum.PC)
            EventSystem.current.SetSelectedGameObject(null);
        else 
            SelectDefaultMenuElement();
    }
    protected abstract void SelectDefaultMenuElement();
}
