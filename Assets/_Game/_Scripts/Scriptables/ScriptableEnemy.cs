using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy")]
public class ScriptableEnemy : ScriptableUnitBase
{
    public EnemyType EnemyType;
}

[SerializeField]
public enum EnemyType
{
    Enemy = 0,
    Enemy2 = 1
}