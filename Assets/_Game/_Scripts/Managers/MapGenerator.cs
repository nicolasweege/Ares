using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _portalPf;
    [SerializeField] private GameObject _keyPf;
    [SerializeField] private const int _mapHight = 64;
    [SerializeField] private const int _mapWidth = 64;

    public int MapHight { get => _mapHight; }
    public int MapWidth { get => _mapWidth; }

    private void Start() => CreateElements();

    private void CreateElements()
    {
        Vector3 portalPos = GetPortalPos();
        Vector3 keyPos = GetKeyPos();

        var portal = Instantiate(_portalPf, portalPos, Quaternion.identity);
        var key = Instantiate(_keyPf, keyPos, Quaternion.identity);
    }

    private Vector3 GetPortalPos()
    {
        return new Vector3(Random.Range(-_mapWidth * .9f, -_mapWidth * .8f), Random.Range(-_mapHight * .9f, _mapHight * .9f));
    }

    private Vector3 GetKeyPos()
    {
        return new Vector3(Random.Range(_mapWidth * .9f, _mapWidth * .8f), Random.Range(-_mapHight * .9f, _mapHight * .9f));
    }
}