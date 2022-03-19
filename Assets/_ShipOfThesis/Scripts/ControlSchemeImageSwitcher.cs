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
        StudyManager._instance.ControlsChanged += ChangeImage;
        //Debug.Log("PrintOnDisable: script was enabled");
        ChangeImage();
    }
    
    private void OnDisable()
    {
        StudyManager._instance.ControlsChanged += ChangeImage;
        //Debug.Log("PrintOnDisable: script was disabled");
    }

    private void ChangeImage()
    {
        Keyboard.gameObject.SetActive(false);
        Xbox.gameObject.SetActive(false);
        Playstation.gameObject.SetActive(false);
        
        switch (StudyManager._instance.current)
        {
            case StudyManager.ControllerEnum.PC:
                Keyboard.gameObject.SetActive(true);
                break;
            case StudyManager.ControllerEnum.XBOX:
                Xbox.gameObject.SetActive(true);
                break;
            case StudyManager.ControllerEnum.PS4:
                Playstation.gameObject.SetActive(true);
                break;
        }
    }
}