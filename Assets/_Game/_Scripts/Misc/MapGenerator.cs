using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _portalPrefab;
    public int _gridWidth = 16;
    public int _gridHight = 16;
    public int _mapHight = 32;
    public int _mapWidth = 64;

    private void Start()
    {
        CreatePortal();
    }

    private void CreatePortal()
    {
        Vector3 portalPos = GetPortalPos();
        var portal = Instantiate(_portalPrefab, portalPos, transform.rotation);
    }

    private Vector3 GetPortalPos()
    {
        return new Vector3(Random.Range(-_mapWidth, _mapWidth), Random.Range(-_mapHight, _mapHight), 0f);
    }
}