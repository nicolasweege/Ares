using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _portal;
    [SerializeField] private GameObject _key;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _camera;
    [SerializeField] private const int _mapHight = 32;
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
        Vector3 portalPos = GetElementPos();
        Vector3 keyPos = GetElementPos();
        Vector3 playerPos = GetElementPos();

        var portal = Instantiate(_portal, portalPos, transform.rotation);
        var key = Instantiate(_key, keyPos, transform.rotation);
        var player = Instantiate(_player, playerPos, transform.rotation);
        _camera.GetComponent<FollowingCamera>().SetPlayerTransform(player.transform);
    }

    private Vector3 GetElementPos()
    {
        return new Vector3(Random.Range(-_mapWidth, _mapWidth), Random.Range(-_mapHight, _mapHight), 0f);
    }
}