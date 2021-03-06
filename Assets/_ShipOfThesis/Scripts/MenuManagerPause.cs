
using System;
using System.Collections;
using Cinemachine.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManagerPause : MenuEssentials
{
    public Sailboat boat;
    public Image blackout;
    public SailingCourse course;
    public TextMeshProUGUI CountdownTimer;
    public TextMeshProUGUI StandardTimer;
    public TextMeshProUGUI LapTimer;

    public GameObject SimpleControlDisplay;
    public GameObject ComplexControlDisplay;
    
    public float fadeTime;
    public float practiceTime;
    private bool practice;
    private bool interactible;
    private bool simple;
    private int pauseState; // 0 - unpaused, 1 - paused, 2 - controls screen

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
    private Vector3 target;
    public RectTransform targetPointer;

    private int pauses;
    private int controlChecks;
    
    // Start is called before the first frame update
    void Start()
    {
        targetPointer.gameObject.SetActive(false);
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
        
        SimpleControlDisplay.SetActive(simple);
        ComplexControlDisplay.SetActive(!simple);
        
        foreach(GameObject go in SimpleTips)
            go.SetActive(simple);
        foreach(GameObject go in ComplexTips)
            go.SetActive(!simple);
        foreach(GameObject go in FreePlayTips)
            go.SetActive(StudyManager.Instance.freePlay);
        
        practice = StudyManager.Instance.subStage == 1;
        SkipButton.gameObject.SetActive(practice);
        ResetButton.gameObject.SetActive(!practice);
        boat.practice = practice;
            
        StartCoroutine(FadeInOut(true, fadeTime));
    }

    private IEnumerator FadeInOut(bool fadeIn, float seconds)
    {
        interactible = false;
        Time.timeScale = 1f;
        Debug.Log("Start Fade " + (fadeIn ? "In" : "Out"));
        blackout.color = new Color(0f,0f,0f,fadeIn ? 1f : 0f);
        blackout.gameObject.SetActive(true);
        LeanTween.alpha((RectTransform) blackout.transform, fadeIn ? 0f : 1f, seconds);
        yield return new WaitForSeconds(seconds);
        blackout.gameObject.SetActive(false);
        
        if (fadeIn)
            StartCoroutine(Countdown());
        else
        {
            Debug.Log("Moving On");
            StudyManager.Instance.Next();
        }
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
        
        if (!practice && !StudyManager.Instance.freePlay)
        {
            course.CheckpointCreatedEvent += UpdateTarget;
            course.CheckpointReachedEvent += x => StartCoroutine(DisplayLapTime(x));
            course.CourseFinishedEvent += SendResults;
            course.NextCheckpoint();
        }
    }

    private void UpdateTarget(Transform checkpoint)
    {
        targetPointer.gameObject.SetActive(true);
        target = checkpoint.position;
    }

    private IEnumerator DisplayLapTime(float time)
    {
        LapTimer.gameObject.SetActive(true);
        LapTimer.text = Timestamp(time);
        yield return new WaitForSecondsRealtime(3f);
        LapTimer.gameObject.SetActive(false);
    }

    private void SendPracticeResults()
    {
        boat.SendPracticeResults();
        StudyManager.Instance.ReceivePracticeResults(pauses,controlChecks,practiceTime);
       
        // Return to Base Pause State - Unpaused
        Pause(false);
        Pause(false);

        StartCoroutine(FadeInOut(false, fadeTime));
    }
    
    private void SendResults(float[] legTimes)
    {
        boat.SendResults();
        StudyManager.Instance.ReceiveResults(pauses,controlChecks,legTimes);
        
        // Return to Base Pause State - Unpaused
        Pause(false);
        Pause(false);

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
                StandardTimer.text = "Time Up!";
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

        if (target != Vector3.zero && Camera.main != null)
        {
            Vector3 flatCamFacing = Camera.main.transform.forward.ProjectOntoPlane(Vector3.up);
            Vector3 flatTargetDir = (target - boat.transform.position).ProjectOntoPlane(Vector3.up);
            targetPointer.rotation = Quaternion.Euler(0f,0f,Vector3.SignedAngle(flatTargetDir,flatCamFacing, Vector3.up));
        }
        
    }

    public void PauseToggle()
    {
        Pause(pauseState == 0);
    }

    public void Pause(bool pauseFurther)
    {
        pauseState += pauseFurther ? 1 : -1;
        pauseState = pauseState < 0 ? 0 : pauseState;
        
        if (interactible)
        {
            MainHUD.SetActive(pauseState == 0);
            Time.timeScale = pauseState == 0 ? 1f : 0f;
            
            PauseMenu.SetActive(pauseState == 1); 
            pauses += pauseFurther ? 1 : 0;

            ControlsScreen.SetActive(pauseState == 2);
            controlChecks += pauseState == 2 ? 1 : 0;

            
            Cursor.lockState = pauseState > 0 ? CursorLockMode.None : CursorLockMode.Locked;
            if (PlayerInputWrapper.Instance.currentScheme == PlayerInputWrapper.ControllerEnum.PC)
                Cursor.visible = pauseState != 0;
            else
                Cursor.visible = false;
        }
        OnControlsChanged();
    }

    public void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) return;
        pauseState = -1;
        PauseToggle();
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
        if (pauseState == 1)
            EventSystem.current.SetSelectedGameObject(firstSelected);
        else
            EventSystem.current.SetSelectedGameObject(ControlsButton.gameObject);
    }
}
