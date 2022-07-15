using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private float _mapHight;
    [SerializeField] private float _mapWidth;

    public float MapHight { get => _mapHight; set => _mapHight = value; }
    public float MapWidth { get => _mapWidth; set => _mapWidth = value; }
}