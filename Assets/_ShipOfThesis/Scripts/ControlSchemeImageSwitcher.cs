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
        PlayerInputWrapper.Instance.ControlsChanged += ChangeImage;
        //Debug.Log("PrintOnDisable: script was enabled");
        ChangeImage();
    }
    
    private void OnDisable()
    {
        PlayerInputWrapper.Instance.ControlsChanged -= ChangeImage;
        //Debug.Log("PrintOnDisable: script was disabled");
    }

    private void ChangeImage()
    {
        Keyboard.gameObject.SetActive(false);
        Xbox.gameObject.SetActive(false);
        Playstation.gameObject.SetActive(false);
        
        switch (PlayerInputWrapper.Instance.currentScheme)
        {
            case PlayerInputWrapper.ControllerEnum.PC:
                Keyboard.gameObject.SetActive(true);
                break;
            case PlayerInputWrapper.ControllerEnum.XBOX:
                Xbox.gameObject.SetActive(true);
                break;
            case PlayerInputWrapper.ControllerEnum.PS4:
                Playstation.gameObject.SetActive(true);
                break;
        }
    }
}