using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _portal;
    [SerializeField] private GameObject _key;
    [SerializeField] private GameObject _camera;
    [SerializeField] private const int _mapHight = 64;
    [SerializeField] private const int _mapWidth = 64;

    private void Start()
    {
        CreateElements();
    }

    public int GetMapHight()
    {
        return _mapHight;
    }

    public int GetMapWidth()
    {
        return _mapWidth;
    }

    private void CreateElements()
    {
        Vector3 portalPos = GetPortalPos();
        Vector3 keyPos = GetKeyPos();

        var portal = Instantiate(_portal, portalPos, Quaternion.identity);
        var key = Instantiate(_key, keyPos, Quaternion.identity);
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