
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManagerMain : MenuEssentials
{
    public Button AboutButton;
    public Button FreePlayButton;
    public GameObject FreePlayText, Menu, About;

    void Start()
    {
        ToggleAbout(false);
        Time.timeScale = 1f;
        FreePlayButton.interactable = StudyManager.Instance.studyComplete;
        FreePlayText.SetActive(!FreePlayButton.interactable);
    }

    public void BeginStudy()
    {
        StudyManager.Instance.BeginStudy();
    }

    public void FreePlay()
    {
        StudyManager.Instance.FreePlay();
    }

    public void ToggleAbout(bool visible)
    {
        Menu.SetActive(!visible);
        About.SetActive(visible);
        OnControlsChanged();
    }

    public void Exit()
    {
        Application.Quit();
    }

    protected override void SelectDefaultMenuElement()
    {
        if (Menu.activeSelf)
            EventSystem.current.SetSelectedGameObject(firstSelected);
        else           
            EventSystem.current.SetSelectedGameObject(AboutButton.gameObject);
    }
}
