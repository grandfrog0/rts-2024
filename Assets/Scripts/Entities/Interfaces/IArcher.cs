public interface IArcher : IUnit
{
    public float MinAttackRange { get; set; }
    public float MaxAttackRange { get; set; }
    public float Cooldown { get; set; }
    public float Damage { get; set; }
}