using UnityEngine.EventSystems;

public class MenuManagerDisclaimer : MenuEssentials
{
    protected override void SelectDefaultMenuElement()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    private void Awake()
    {
        OnControlsChanged();
    }
}
