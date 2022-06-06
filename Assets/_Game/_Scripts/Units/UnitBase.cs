using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public UnitStats Stats { get; private set; }

    public virtual void SetStats(UnitStats stats) => Stats = stats;

    public virtual void TakeDamage(int damage)
    {

    }
}