using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    public void SpawnEnemy(EnemyType enemyType, Vector3 pos)
    {
        var scriptableEnemy = ResourceSystem.Instance.GetEnemy(enemyType);
        // var spawned = Instantiate();
    }
}