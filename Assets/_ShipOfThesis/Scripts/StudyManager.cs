using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class StudyManager : MonoBehaviour
{
    public static StudyManager _instance;
    [HideInInspector] public bool simpleFirst;
    
    
    [HideInInspector] public int stage = -1; // 0 - Disclaimer, 1 - Stage 1, 2 - Stage 2, 3 - Results
    [HideInInspector] public int subStage = 0; // 0 - Briefing, 1 - Practice, 2 - Actual
    public bool studyComplete;
    [HideInInspector] public bool freePlay;
    public enum ControllerEnum { PC, XBOX, PS4 }
    [HideInInspector] public ControllerEnum current = ControllerEnum.PC;
    
    public delegate void ControlsChangedHandler();
    public event ControlsChangedHandler ControlsChanged;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        InputUser.onChange += OnInputDeviceChange;
    }
    
    private void OnDisable()
    {
        InputUser.onChange -= OnInputDeviceChange;
    }
    
    void OnInputDeviceChange(InputUser user, InputUserChange change, InputDevice device) {
        if (change == InputUserChange.ControlSchemeChanged) {
            switch (user.controlScheme.Value.name)
            {
                case "Xbox":
                    current = ControllerEnum.XBOX;
                    break;
                case "Playstation":
                    current = ControllerEnum.PS4;
                    break;
                default:
                    current = ControllerEnum.PC;
                    break;
            }
            ControlsChanged?.Invoke();
        }
    }
    
    public void BeginStudy()
    {
        stage = 0;
        subStage = 0;
        simpleFirst = Random.value > 0.5f;
        freePlay = false;
        Next();
    }

    public void FreePlay()
    {
        
    }

    // Proceed to next stage
    public void Next()
    {
        subStage = stage == 0 ? 0 : (subStage + 1) % 3; // 0 -> 1 -> 2 -> 0
        stage = subStage == 0 ? subStage + 1 : subStage; // Increments when subStage ticks over
        switch (stage)
        {
            case 0: // Disclaimer
                ChangeScene(1);
                break;
            case 1: // Stage 1
            case 2: // Stage 2
                ChangeScene(subStage == 0 ? 2 : 3, subStage != 0);
                break;
            case 3:
                // Stage 2 Debrief or Results
                stage += subStage == 0 ? 0 : 1;
                ChangeScene(subStage == 0 ? 2 : 4);
                break;
            default:
                // Unknown State, return to Menu
                stage = -1;
                subStage = -1;
                ChangeScene(0);
                break;
        }
    }
    
    public static void ChangeScene(int index, bool gameplay = false)
    {
        Cursor.lockState = gameplay ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !gameplay;
        Time.timeScale = gameplay ? 1f : 0f;

        SceneManager.LoadScene(index);
    }

}
