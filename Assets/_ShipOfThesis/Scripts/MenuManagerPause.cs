using System;
using System.Collections;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManagerPause : MenuEssentials
{
    public Sailboat boat;
    public Image blackout;
    public SailingCourse course;
    public TextMeshProUGUI CountdownTimer;
    public TextMeshProUGUI StandardTimer;
    public TextMeshProUGUI LapTimer;
    public float fadeTime;
    public float practiceTime;
    private bool practice;
    private bool interactible;
    private bool simple;

    private float timerFrom;

    public GameObject MainHUD;
    public GameObject PauseMenu;
    public GameObject ControlsScreen;

    public Button ResetButton;
    public Button SkipButton;
    public Button ControlsButton;

    public GameObject[] SimpleTips;
    public GameObject[] ComplexTips;
    public GameObject[] FreePlayTips;

    private int pauses;
    private int controlChecks;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        CountdownTimer.gameObject.SetActive(false);
        StandardTimer.gameObject.SetActive(false);
        LapTimer.gameObject.SetActive(false);
        
        PauseMenu.SetActive(false);
        MainHUD.SetActive(true);
        ControlsScreen.SetActive(false);
        
        boat.freeplay = StudyManager.Instance.freePlay;
        boat.controlsActive = false;
        simple = !boat.freeplay && // Default controls for free play are complex
                              StudyManager.Instance.stage == (StudyManager.Instance.simpleFirst ?  1 : 2);
        boat.SimpleControls = simple;
        
        foreach(GameObject go in SimpleTips)
            go.SetActive(simple);
        foreach(GameObject go in ComplexTips)
            go.SetActive(!simple);
        foreach(GameObject go in FreePlayTips)
            go.SetActive(StudyManager.Instance.freePlay);
        
        practice = StudyManager.Instance.subStage == 1;
        SkipButton.gameObject.SetActive(practice);
        ResetButton.gameObject.SetActive(!practice);
        
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
            StudyManager.Instance.Next();
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

        StandardTimer.gameObject.SetActive(!StudyManager.Instance.freePlay);
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
        return (m == 0 ? "" : (m + ":")) + $"{s:#0.000}";
    }
    
    private void SendPracticeResults()
    {
        boat.SendPracticeResults();
        StudyManager.Instance.ReceivePracticeResults(pauses,controlChecks,practiceTime);
        StartCoroutine(FadeInOut(false, fadeTime));
    }
    
    private void SendResults(float[] legTimes)
    {
        boat.SendResults();
        StudyManager.Instance.ReceiveResults(pauses,controlChecks,legTimes);
        StartCoroutine(FadeInOut(false, fadeTime));
    }
    
    // Update is called once per frame
    void Update()
    {
        if (interactible && !StudyManager.Instance.freePlay)
        {
            StandardTimer.text = Timestamp(practice ? (timerFrom + practiceTime) - Time.time : Time.time - timerFrom);
            //Debug.Log(StandardTimer.text);
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
    }

    public void PauseToggle()
    {
        Pause(Mathf.Approximately(Time.timeScale, 1f));
    }

    public void Pause(bool paused)
    {
        if (interactible)
        {
            MainHUD.SetActive(!paused);
            Time.timeScale = paused ? 0f : 1f;
            PauseMenu.SetActive(paused);
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = PlayerInputWrapper.Instance.currentScheme == PlayerInputWrapper.ControllerEnum.PC;
            pauses += paused ? 1 : 0;
        }
        if (paused)
            OnControlsChanged();
    }

    public void ShowControls(bool shown)
    {
        PauseMenu.SetActive(!shown);
        ControlsScreen.SetActive(shown);
        controlChecks += shown ? 1 : 0;
        OnControlsChanged();
    }

    public void EndPracticeEarly()
    {
        practiceTime = Time.time - timerFrom;
        SendPracticeResults();
    }

    public void AbortStudy()
    {
        StudyManager.Instance.AbortToMenu();
    }

    public void Reset()
    {       
        if (!StudyManager.Instance.freePlay)
            StudyManager.Instance.IncrementResets();
        StudyManager.ChangeScene(3,true);
    }

    protected override void SelectDefaultMenuElement()
    {
        if (PauseMenu.activeSelf)
            EventSystem.current.SetSelectedGameObject(firstSelected);
        else
            EventSystem.current.SetSelectedGameObject(ControlsButton.gameObject);

            
    }
}
