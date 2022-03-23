
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SailboatCameraControls : MonoBehaviour
{
    private float deltaX, deltaY;
    public CinemachineFreeLook cm_FreeLook;
    public CinemachineVirtualCamera cm_FocusLeft;
    public CinemachineVirtualCamera cm_FocusRight;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cm_FreeLook.m_XAxis.Value += Time.deltaTime * deltaX * cm_FreeLook.m_XAxis.m_MaxSpeed * 
                                     (cm_FreeLook.m_XAxis.m_InvertInput ? -1 : 1);
        cm_FreeLook.m_YAxis.Value += Time.deltaTime * deltaY * cm_FreeLook.m_YAxis.m_MaxSpeed * 
                                     (cm_FreeLook.m_YAxis.m_InvertInput ? -1 : 1);
    }
    
    public void OnGetCameraX(InputValue value)
    {
        deltaX = value.Get<float>();
    }
    
    public void OnGetCameraY(InputValue value)
    {
        deltaY = value.Get<float>();
    }
}
