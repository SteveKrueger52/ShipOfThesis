using System;
using UnityEngine;

public class MenuManagerIntro : MenuEssentials
{
    public GameObject FillOutSurvey, TaskIntro, Debrief, Preview, Simple, Complex;

    private void Start()
    {
        bool simpleFirst = StudyManager._instance.simpleFirst;
        int stage = StudyManager._instance.stage;
        FillOutSurvey.SetActive(stage > 1);
        TaskIntro.SetActive(stage < 3);
        Preview.SetActive(stage < 3);
        Debrief.SetActive(stage == 3);
        Simple .SetActive(stage == 1 ?  simpleFirst : !simpleFirst);
        Complex.SetActive(stage == 1 ? !simpleFirst :  simpleFirst);
    }

    private void Update()
    {
        Cursor.visible = StudyManager._instance.current == StudyManager.ControllerEnum.PC;
    }
}
