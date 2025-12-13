
using UnityEngine;

public interface IHurtable
{
    public Vector3 Position { get; }
    public bool IsAlive { get; }
    public void TakeDamage(float value, Entity enemy);
}
