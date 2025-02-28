using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GridManager : MonoBehaviour
{

    public int numRows = 5;
    public int numCols = 6;

    [SerializeField] private GridTile tilePrefab;
    [SerializeField] private PlayerMovement playerPrefab;

    private List<GridTile> _tiles = new List<GridTile>();

    private void Awake()
    {

        _tiles.Capacity = numRows * numCols;

        InitGrid();

    }

    public void InitGrid()
    {

        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numCols; x++)
            {

                GridTile tile = Instantiate(tilePrefab, transform); // creates the grid
                Vector2 tilePos = new Vector2(x, y);
                tile.transform.position = tilePos;
                tile.name = $"Tile_{x}_{y}";
                _tiles.Add(tile);
            }
        }

    }

    public GridTile GetTile(int col, int row)
    {

        int index = row * numCols + col;
        return _tiles[index];

    }
}
