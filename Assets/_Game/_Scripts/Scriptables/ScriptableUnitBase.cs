using UnityEngine;

public abstract class ScriptableUnitBase : ScriptableObject
{
    public UnitType UnitType;
    [SerializeField] private UnitStats _stats;
    public UnitStats BaseStats => _stats;

    public PlayerUnitBase Prefab;
}

[SerializeField] public struct UnitStats
{
    public int Health;
    public float Speed;
    public int DefaultDamage;
}

[SerializeField] public enum UnitType
{
    Player = 0,
    Enemy = 1
}