using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private const float _mapHight = 20f;
    [SerializeField] private const float _mapWidth = 20f;

    public float MapHight { get => _mapHight; }
    public float MapWidth { get => _mapWidth; }
}