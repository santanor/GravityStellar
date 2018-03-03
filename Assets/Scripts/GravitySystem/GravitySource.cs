using UnityEngine;

public class GravitySource : MonoBehaviour
{
    public delegate void GravityPulseEvent( Vector3 sourcePoint, float pullForce );

    public GravityPulseEvent OnGravityPulse;
    public float PullForce = 1f;

    void FixedUpdate()
    {
        OnGravityPulse?.Invoke(transform.position, PullForce);
    }
}
