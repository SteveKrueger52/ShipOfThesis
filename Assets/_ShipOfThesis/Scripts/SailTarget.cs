using UnityEngine;

public class SailTarget : MonoBehaviour
{
    public delegate void CheckpointReachedHandler();
    public event CheckpointReachedHandler CheckpointReached;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Sailboat>() != null)
        {
            CheckpointReached?.Invoke();
            Destroy(this);
        }
    }
}
