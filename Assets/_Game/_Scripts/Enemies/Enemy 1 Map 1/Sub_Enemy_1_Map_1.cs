using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Enemy_1_Map_1 : EnemyBase
{
    [SerializeField] private GameObject _shotPrefab;
    [SerializeField] private Transform _shotStartPosTop1;
    [SerializeField] private Transform _shotStartPosTop2;
    [SerializeField] private Transform _shotStartPosBottom1;
    [SerializeField] private Transform _shotStartPosBottom2;
    [SerializeField] private Transform _shotStartPosRight;
    [SerializeField] private Transform _shotStartPosLeft;

    private void Update()
    {
        
    }

    private void GenerateShot(Transform shotStartPos)
    {

    }
}