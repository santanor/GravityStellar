using UnityEngine;
using UnityEngine.Assertions;

public class GravitySource : MonoBehaviour
{
    public delegate void GravityPulseEvent( GravitySource source, Vector3 sourcePoint, float pullForce );

    public GravityPulseEvent OnGravityPulse;
    public float PullForce = 1f;
    public GravitySystem.GravitySystem GravitySystem;
    public float InfluenceAreaRadius = 3;

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
        OnGravityPulse?.Invoke(this, transform.position, PullForce);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, InfluenceAreaRadius);
    }
}
