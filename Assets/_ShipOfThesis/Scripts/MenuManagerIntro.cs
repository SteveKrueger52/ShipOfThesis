using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManagerIntro : MenuEssentials
{
    public GameObject FillOutSurvey, TaskIntro, Debrief, Preview, Simple, Complex;

    private void Start()
    {
        bool simpleFirst = StudyManager.Instance.simpleFirst;
        int stage = StudyManager.Instance.stage;
        FillOutSurvey.SetActive(stage > 1);
        TaskIntro.SetActive(stage < 3);
        Preview.SetActive(stage < 3);
        //Debrief.SetActive(stage == 3);
        Simple .SetActive(stage == 1 ?  simpleFirst : !simpleFirst);
        Complex.SetActive(stage == 1 ? !simpleFirst :  simpleFirst);
        OnControlsChanged();
    }

    protected override void SelectDefaultMenuElement()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
