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
        PlayerInputManager.Instance.ControlsChanged += OnControlsChanged;
    
    }
    
    private void OnDisable()
    {
        PlayerInputManager.Instance.ControlsChanged -= OnControlsChanged;
    }

    public void OnControlsChanged()
    {
        Cursor.visible = PlayerInputManager.Instance.currentScheme == PlayerInputManager.ControllerEnum.PC;

        if (PlayerInputManager.Instance.currentScheme == PlayerInputManager.ControllerEnum.PC)
            EventSystem.current.SetSelectedGameObject(null);
        else 
            SelectDefaultMenuElement();
    }
    protected abstract void SelectDefaultMenuElement();
}
