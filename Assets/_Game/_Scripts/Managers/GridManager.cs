using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tile;
    [SerializeField] private Transform _cameraTransform;

    private void Start() => GenerateGrid();

    private void GenerateGrid()
    {
        for (float x = -_width; x <= _width; x++)
        {
            for (float y = -_height; y <= _height; y++)
            {
                var tile = Instantiate(_tile, new Vector3(x, y), Quaternion.identity);
                tile.name = $"Tile {x} {y}";

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                _tile.Init(isOffset);
            }
        }

        _cameraTransform.transform.position = new Vector3(_width/2 - .5f, _height/2 - .5f, -10);
    }
}