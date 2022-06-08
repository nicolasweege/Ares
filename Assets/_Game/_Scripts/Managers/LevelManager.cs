using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject _portalPrefab;
    [SerializeField] private GameObject _keyPrefab;
    [SerializeField] private const int _mapHight = 64;
    [SerializeField] private const int _mapWidth = 64;

    public int MapHight { get => _mapHight; }
    public int MapWidth { get => _mapWidth; }

    private void Start() => GenerateElements();

    private void GenerateElements()
    {
        Vector3 portalPos = GetPortalPos();
        Vector3 keyPos = GetKeyPos();

        var portal = Instantiate(_portalPrefab, portalPos, Quaternion.identity);
        var key = Instantiate(_keyPrefab, keyPos, Quaternion.identity);
    }

    private Vector3 GetPortalPos() => new Vector3(Random.Range(-_mapWidth * .9f, -_mapWidth * .8f), Random.Range(-_mapHight * .9f, _mapHight * .9f));

    private Vector3 GetKeyPos() => new Vector3(Random.Range(_mapWidth * .9f, _mapWidth * .8f), Random.Range(-_mapHight * .9f, _mapHight * .9f));
}