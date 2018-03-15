using UnityEngine;
using UnityEngine.Assertions;

public class GravitySource : MonoBehaviour
{
    public delegate void GravityPulseEvent( Vector3 sourcePoint, float pullForce );

    public GravityPulseEvent OnGravityPulse;
    public float PullForce = 1f;
    public GravitySystem.GravitySystem GravitySystem;

    void Awake()
    {
        Assert.IsNotNull(GravitySystem);
        GravitySystem.AddGravitySource(this);
    }

    void OnDestroy()
    {
        GravitySystem.DestroyGravitySource(this);
    }

    void FixedUpdate()
    {
        OnGravityPulse?.Invoke(transform.position, PullForce);
    }
}
