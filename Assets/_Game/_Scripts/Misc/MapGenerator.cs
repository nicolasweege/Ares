using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _portal;
    [SerializeField] private GameObject _key;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _camera;
    public int _gridWidth = 16;
    public int _gridHight = 16;
    public int _mapHight = 32;
    public int _mapWidth = 64;

    private void Start()
    {
        CreateElements();
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