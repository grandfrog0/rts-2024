
using UnityEngine;
using UnityEngine.Events;

public interface IHurtable
{
    public UnityEvent OnDead { get; }
    public Vector3 Position { get; }
    public bool IsReady { get; }
    public void TakeDamage(float value, Entity enemy);
}
