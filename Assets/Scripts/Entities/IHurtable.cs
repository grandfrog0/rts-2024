
using UnityEngine;
using UnityEngine.Events;

public interface IHurtable
{
    public UnityEvent OnDead { get; }
    public Vector3 Position { get; }
    public bool IsAlive { get; }
    public void TakeDamage(float value, Entity enemy);
}
