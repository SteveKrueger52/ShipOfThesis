using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlSchemeImageSwitcher : MonoBehaviour
{
    public Image Keyboard;
    public Image Playstation;
    public Image Xbox;

    private void OnEnable()
    {
        PlayerInputManager.Instance.ControlsChanged += ChangeImage;
        //Debug.Log("PrintOnDisable: script was enabled");
        ChangeImage();
    }
    
    private void OnDisable()
    {
        PlayerInputManager.Instance.ControlsChanged += ChangeImage;
        //Debug.Log("PrintOnDisable: script was disabled");
    }

    private void ChangeImage()
    {
        Keyboard.gameObject.SetActive(false);
        Xbox.gameObject.SetActive(false);
        Playstation.gameObject.SetActive(false);
        
        switch (PlayerInputManager.Instance.currentScheme)
        {
            case PlayerInputManager.ControllerEnum.PC:
                Keyboard.gameObject.SetActive(true);
                break;
            case PlayerInputManager.ControllerEnum.XBOX:
                Xbox.gameObject.SetActive(true);
                break;
            case PlayerInputManager.ControllerEnum.PS4:
                Playstation.gameObject.SetActive(true);
                break;
        }
    }
}