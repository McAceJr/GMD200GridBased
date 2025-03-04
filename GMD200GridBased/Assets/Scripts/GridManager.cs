using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GridManager : MonoBehaviour
{

    public int gridLevel;

    public int numRows = 5;
    public int numCols = 6;

    public GameObject holderPrefab;
    [SerializeField] private GridTile tilePrefab;
    [SerializeField] private PlayerMovement playerPrefab;

    private List<GridTile> _tiles = new List<GridTile>();

    private void Awake()
    {

        ClearGrid();

        _tiles.Capacity = numRows * numCols;

    }

    private void Start()
    {

        InitGrid();

        CenterGrid();

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
                tile.name = $"{gridLevel}Tile_{x}_{y}";
                _tiles.Add(tile);
            }
        }

    }

    public void CenterGrid()
    {

        float totalX = 0;
        float totalY = 0;

        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numCols; x++)
            {
                totalX += GetTile(x, y).transform.position.x;
                totalY += GetTile(x, y).transform.position.y;
            }
        }

        float averageX, averageY;

        averageX = totalX / _tiles.Capacity;
        averageY = totalY / _tiles.Capacity;

        transform.position = new Vector3(transform.position.x - averageX, transform.position.y - averageY, transform.position.z);

    }

    public void ClearGrid()
    {
        for (int i = 0; i < _tiles.Capacity; i++)
        {

            Destroy(this.transform.GetChild(i).gameObject);

        }
    }

    public GridTile GetTile(int col, int row)
    {

        int index = row * numCols + col;
        return _tiles[index];

    }


}
