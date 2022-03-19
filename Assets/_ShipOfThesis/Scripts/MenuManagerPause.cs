﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerPause : MenuEssentials
{
    public Sailboat boat;
    public Image blackout;
    public SailingCourse course;
    public TextMeshPro CountdownTimer;
    public TextMeshPro StandardTimer;
    public TextMeshPro LapTimer;
    public float fadeTime;
    public float practiceTime;
    private bool practice;
    private bool interactible;
    private bool simple;

    private float timerFrom;

    public GameObject ControlTips;
    public GameObject PauseMenu;
    public GameObject ControlsScreen;

    public GameObject[] SimpleTips;
    public GameObject[] ComplexTips;
    public GameObject[] FreePlayTips;

    private int pauses;
    private int controlChecks;
    
    // Start is called before the first frame update
    void Start()
    {
        CountdownTimer.gameObject.SetActive(false);
        StandardTimer.gameObject.SetActive(false);
        LapTimer.gameObject.SetActive(false);
        
        boat.freeplay = StudyManager._instance.freePlay;
        boat.controlsActive = false;
        simple = !boat.freeplay && // Default controls for free play are complex
                              StudyManager._instance.stage == (StudyManager._instance.simpleFirst ?  1 : 2);
        boat.SimpleControls = simple;
        
        foreach(GameObject go in SimpleTips)
            go.SetActive(simple);
        foreach(GameObject go in ComplexTips)
            go.SetActive(!simple);
        
        practice = StudyManager._instance.subStage == 1;
        StartCoroutine(FadeInOut(true, fadeTime));
    }

    private IEnumerator FadeInOut(bool fadeIn, float seconds)
    {
        interactible = false;
        blackout.color = new Color(0f,0f,0f,fadeIn ? 1f : 0f);
        LeanTween.alpha((RectTransform) blackout.transform, fadeIn ? 0f : 1f, seconds);
        yield return new WaitForSeconds(seconds);

        if (fadeIn)
            StartCoroutine(Countdown());
        else
            StudyManager._instance.Next();
    }

    private IEnumerator Countdown()
    {
        CountdownTimer.gameObject.SetActive(true);
        timerFrom = Time.time;
        for (int i = 1; i <= 3; i++)
        {
            CountdownTimer.text = (4 - i).ToString();
            yield return new WaitUntil(() => timerFrom + i < Time.time);
        }
        CountdownTimer.gameObject.SetActive(false);
        interactible = true;
        boat.controlsActive = interactible;

        StandardTimer.gameObject.SetActive(!StudyManager._instance.freePlay);
        timerFrom = Time.time;
        
        if (!practice)
        {
            course.CheckpointReachedEvent += x => StartCoroutine(DisplayLapTime(x));
            course.CourseFinishedEvent += SendResults;
            course.NextCheckpoint();
        }
    }

    private IEnumerator DisplayLapTime(float time)
    {
        LapTimer.gameObject.SetActive(true);
        LapTimer.text = Timestamp(time);
        yield return new WaitUntil(() => Time.time > time + 3f);
        LapTimer.gameObject.SetActive(false);
    }

    private string Timestamp(float seconds)
    {
        int m = Mathf.FloorToInt(seconds / 60f);
        float s = seconds - (60f * m);
        return String.Format(m == 0 ? "" : "D:" + "F3", m,s);
    }
    
    private void SendPracticeResults()
    {
        boat.SendPracticeResults();
        StudyManager._instance.ReceivePracticeResults(pauses,controlChecks,practiceTime);
        FadeInOut(false, 1f);
    }
    
    private void SendResults(float[] legTimes)
    {
        boat.SendResults();
        StudyManager._instance.ReceiveResults(pauses,controlChecks,legTimes);
        FadeInOut(false, 1f);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (interactible && !StudyManager._instance.freePlay)
        {
            StandardTimer.text = Timestamp(practice ? (timerFrom + practiceTime) - Time.time : Time.time - timerFrom);
            if (practice && Time.time > timerFrom + practiceTime)
            {
                SendPracticeResults();
            }
        }

        if (boat.SimpleControls != simple)
        {
            simple = boat.SimpleControls;
            foreach(GameObject go in SimpleTips)
                go.SetActive(simple);
            foreach(GameObject go in ComplexTips)
                go.SetActive(!simple);
        }

        if (Mathf.Approximately(0f, Time.timeScale))
            Cursor.visible = StudyManager._instance.current == StudyManager.ControllerEnum.PC;
    }

    public void Pause(bool paused)
    {
        if (interactible)
        {
            ControlTips.SetActive(!paused);
            Time.timeScale = paused ? 0f : 1f;
            PauseMenu.SetActive(paused);
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
            pauses += paused ? 1 : 0;
        }
    }

    public void ShowControls(bool shown)
    {
        PauseMenu.SetActive(!shown);
        ControlsScreen.SetActive(shown);
        controlChecks += shown ? 1 : 0;
    }

    public void EndPracticeEarly()
    {
        practiceTime = Time.time - timerFrom;
        SendPracticeResults();
    }

    public void AbortStudy()
    {
        StudyManager._instance.AbortToMenu();
    }

    public void Reset()
    {       
        //TODO 
    }
}
