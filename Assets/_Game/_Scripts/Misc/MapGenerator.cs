using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _portal;
    [SerializeField] private GameObject _key;
    [SerializeField] private GameObject _player;
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
        Vector3 playerPos = new Vector3(0f, 0f, 0f);

        var portal = Instantiate(_portal, portalPos, transform.rotation);
        var key = Instantiate(_key, keyPos, transform.rotation);

        var player = Instantiate(_player, playerPos, transform.rotation);
        _camera.GetComponent<FollowingCamera>().SetPlayerTransform(player.transform);
    }

    private Vector3 GetPortalPos()
    {
        return new Vector3(Random.Range(-_mapWidth*.9f, -_mapWidth*.8f), Random.Range(-_mapHight*.9f, _mapHight*.9f), 0f);
    }

    private Vector3 GetKeyPos()
    {
        return new Vector3(Random.Range(_mapWidth*.9f, _mapWidth*.8f), Random.Range(-_mapHight*.9f, _mapHight*.9f), 0f);
    }
}