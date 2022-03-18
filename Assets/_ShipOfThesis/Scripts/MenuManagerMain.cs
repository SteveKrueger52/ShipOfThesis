using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerMain : MenuEssentials
{
    public Button FreePlayButton;
    public GameObject FreePlayText, Menu, About;

    void Start()
    {
        ToggleAbout(false);
        Time.timeScale = 1f;
        FreePlayButton.interactable = StudyManager._instance.studyComplete;
        FreePlayText.SetActive(!FreePlayButton.interactable);
    }

    public void BeginStudy()
    {
        StudyManager._instance.BeginStudy();
    }

    public void FreePlay()
    {
        StudyManager._instance.FreePlay();
    }

    public void ToggleAbout(bool visible)
    {
        Menu.SetActive(!visible);
        About.SetActive(visible);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
