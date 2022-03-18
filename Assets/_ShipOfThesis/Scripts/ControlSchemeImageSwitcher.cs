using UnityEngine;
using UnityEngine.UI;

public class ControlSchemeImageSwitcher : MonoBehaviour
{
    public Image ToSwitch;

    public Sprite Keyboard;
    public Sprite Playstation;
    public Sprite Xbox;

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
        switch (StudyManager._instance.current)
        {
            case StudyManager.ControllerEnum.PC:
                ToSwitch.sprite = Keyboard;
                break;
            case StudyManager.ControllerEnum.XBOX:
                ToSwitch.sprite = Xbox;
                break;
            case StudyManager.ControllerEnum.PS4:
                ToSwitch.sprite = Playstation;
                break;
        }
    }
}