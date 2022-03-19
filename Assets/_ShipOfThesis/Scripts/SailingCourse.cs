using UnityEngine;

public class SailingCourse : MonoBehaviour
{
    public Vector3[] targets;
    public Transform checkpointPrefab;
    private int _currentIdx = 0;
    private float[] _legTimes;
    private float _compareTime;

    private void Awake()
    {
        _legTimes = new float[targets.Length];
    }

    public delegate void CourseFinished(float[] times);
    public event CourseFinished CourseFinishedEvent;
    
    public delegate void CheckpointReached(float time);
    public event CheckpointReached CheckpointReachedEvent;

    public void NextCheckpoint()
    {
        if (_currentIdx > 0)
        {
            _legTimes[_currentIdx - 1] = Time.time - _compareTime;
            CheckpointReachedEvent?.Invoke(_legTimes[_currentIdx-1]);
        }
        _compareTime = Time.time;
        
        if (_currentIdx > targets.Length)
            CourseFinishedEvent?.Invoke(_legTimes);
        else
        {
            Transform nextCp = Instantiate(checkpointPrefab, targets[_currentIdx++], Quaternion.identity);
            nextCp.GetComponent<SailTarget>().CheckpointReached += NextCheckpoint;
        }
    }
}
