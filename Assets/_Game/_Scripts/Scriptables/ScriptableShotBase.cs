using UnityEngine;

public abstract class ScriptableShotBase : ScriptableObject
{
    public ShotType ShotType;
    [SerializeField] private ShotStats _stats;
    public ShotStats BaseStats => _stats;
}

[SerializeField]
public struct ShotStats
{
    public float Speed;
    public int DefaultDamage;
}

[SerializeField]
public enum ShotType
{
    PlayerShot = 0,
    EnemyShot = 1
}