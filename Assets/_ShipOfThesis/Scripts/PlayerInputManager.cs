using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : Singleton<PlayerInputManager>
{
    private PlayerInput _playerInput;
    private Sailboat _boat;
    
    public enum ControllerEnum { PC, XBOX, PS4 }
    [HideInInspector] public ControllerEnum currentScheme = ControllerEnum.PC;

    public delegate void ControlsChangedHandler();
    public event ControlsChangedHandler ControlsChanged;
    
    protected override void OnAwake()
    {
        base.OnAwake();
        _playerInput = GetComponent<PlayerInput>();
        FindBoat();
    }

    private void OnControlsChanged(PlayerInput playerInput)
    {
        switch (playerInput.currentControlScheme)
        {
            case "Xbox":
                currentScheme = ControllerEnum.XBOX;
                break;
            case "Playstation":
                currentScheme = ControllerEnum.PS4;
                break;
            default:
                currentScheme = ControllerEnum.PC;
                break;
        }
        ControlsChanged?.Invoke();
        Debug.Log("Now Using " + playerInput.currentControlScheme);
    }

    private void OnEnable()
    {        
        _playerInput.onControlsChanged += OnControlsChanged;
        StudyManager.Instance.SceneChanged += FindBoat;
    }
    
    private void OnDisable()
    {
        _playerInput.onControlsChanged -= OnControlsChanged;
        StudyManager.Instance.SceneChanged -= FindBoat;
    }

    private void FindBoat()
    {
        _boat = FindObjectOfType<Sailboat>();
        _playerInput.camera = Camera.main;
    }
    
    public void OnToggleHalyard()
    {
        if (_boat != null)
            _boat.OnToggleHalyard();
    }
    
    public void OnGetHalyard(InputValue value)
    {
        if (_boat != null)
            _boat.OnGetHalyard(value);
    }

    public void OnGetSteering(InputValue value)
    {
        if (_boat != null)
            _boat.OnGetSteering(value);
    }
    
    public void OnGetRudder(InputValue value)
    {
        if (_boat != null)
            _boat.OnGetRudder(value);
    }
    
    public void OnGetMainsheet(InputValue value)
    {
        if (_boat != null)
            _boat.OnGetMainsheet(value);
    }

    public void OnSwapControls()
    {
        if (_boat != null)
            _boat.OnSwapControls();
    }
}
