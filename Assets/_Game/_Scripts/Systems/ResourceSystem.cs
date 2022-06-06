using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem>
{
    public List<ScriptableEnemy> Enemies { get; private set; }
    private Dictionary<EnemyType, ScriptableEnemy> _enemiesDict;

    protected override void Awake()
    {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources()
    {
        Enemies = Resources.LoadAll<ScriptableEnemy>("Enemies").ToList();
        _enemiesDict = Enemies.ToDictionary(e => e.EnemyType, e => e);
    }

    public ScriptableEnemy GetEnemy(EnemyType t) => _enemiesDict[t];

    public ScriptableEnemy GetRandomEnemy(EnemyType t) => Enemies[Random.Range(0, Enemies.Count)];
}