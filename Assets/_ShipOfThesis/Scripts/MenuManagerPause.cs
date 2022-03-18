using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MenuManagerPause : MenuEssentials
{
    public Sailboat boat;
    private bool practice;

    // Start is called before the first frame update
    void Start()
    {
        boat.freeplay = StudyManager._instance.freePlay;
        boat.controlsActive = false;
        boat.SimpleControls = !boat.freeplay && // Default controls for free play are complex
                              StudyManager._instance.stage == (StudyManager._instance.simpleFirst ?  1 : 2);
        practice = StudyManager._instance.subStage == 1;
    }

    private IEnumerator FadeIn()
    {
        
    }

    private IEnumerator Countdown()
    {
        
    }

    private void Begin()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
