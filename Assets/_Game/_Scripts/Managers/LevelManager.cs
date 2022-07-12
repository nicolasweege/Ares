using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private const int _mapHight = 20;
    [SerializeField] private const int _mapWidth = 20;

    public int MapHight { get => _mapHight; }
    public int MapWidth { get => _mapWidth; }
}