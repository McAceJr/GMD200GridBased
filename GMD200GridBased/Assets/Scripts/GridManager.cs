using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{

    public bool playerIsTweening;

    public int gridLevel;

    public int numRows = 5;
    public int numCols = 6;

    public Texture2D level;

    public GameObject holderPrefab;
    [SerializeField] private GridTile tilePrefab;
    public PlayerMovement playerPrefab;
    public BoxMovement boxPrefab;
    public GoalManager boxGoalPrefab;
    public GoalManager playerGoalPrefab;

    public GameObject winUI;

    public PlayerMovement player;
    private List<GridTile> _tiles = new List<GridTile>();
    public List<BoxMovement> _boxes = new List<BoxMovement>();
    public List<GoalManager> _goals = new List<GoalManager>();

    private bool uiActive = false;

    public Ease ease;
    public float scaleDuration;
    private Tween scaleTween;

    private void Awake()
    {

        winUI.SetActive(false);

        _goals.Capacity = 0;

        _boxes.Capacity = 0;

    }

    private void Start()
    {

        InitGrid();

        CenterGrid();

    }

    private void Update()
    {
        bool complete = true;
        
        if (!uiActive)
        {
            for (int i = 0; i < _goals.Capacity; i++)
            {

                if (!_goals[i].Active)
                {
                    complete = false;
                }
            }
        }

        if (complete)
        {

            if (!uiActive)
            {

                player.moveTween.Kill();

                ClearGrid();

                StartCoroutine(InputToLeave());

            }

            winUI.SetActive(true);

            uiActive = true;

        }

        if (Input.GetKeyDown(KeyCode.R))
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {

            if (player._playerPositions.Capacity <= 1)
            {

                return;

            }

            player.MoveTo(player._playerPositions[player.undos - 2], new Vector2Int (0, 0), 0, true);

            player._playerPositions.RemoveAt(player.undos - 1);

            player._playerPositions.Capacity--;

            for (int i = 0; i < _boxes.Capacity; i++)
            {

                _boxes[i].Move(_boxes[i]._boxPositions[_boxes[i].undos - 2], 0);

                _boxes[i]._boxPositions.RemoveAt(_boxes[i].undos - 1);

                _boxes[i]._boxPositions.Capacity--;

            }

            for (int i = 0; i < _goals.Capacity; i++)
            {

                _goals[i].Active = _goals[i]._states[_goals[i].undos - 2];

                _goals[i]._states.RemoveAt(_goals[i].undos - 1);

                _goals[i]._states.Capacity--;

            }

        }
    }

    public void InitGrid()
    {
        Color[] colorData = level.GetPixels();

        numCols = level.width;
        numRows = level.height;

        _tiles.Capacity = numRows * numCols;

        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numCols; x++)
            {
                GridTile tile = Instantiate(tilePrefab, transform); // creates the grid
                Vector2 tilePos = new Vector2(x, y);
                
                tile.transform.position = tilePos;
                tile.name = $"{gridLevel}Tile_{x}_{y}";
                _tiles.Add(tile);

                int index = y * numCols + x;
                tile.spr.color = colorData[index];
                tile.AssignType();
                if (tile.data[2].isType)
                {

                    player = Instantiate(playerPrefab, tile.transform);

                    player.gridPos = new Vector2Int(x, y);
                }
                else if (tile.data[3].isType)
                {

                    BoxMovement box = Instantiate(boxPrefab, tile.transform);

                    box.transform.position = tilePos;

                    box._boxPositions.Add(new Vector2Int(x, y));

                    box.gridPos = new Vector2Int(x,y);

                    _boxes.Capacity++;

                    _boxes.Add(box);

                }
                else if (tile.data[4].isType)
                {

                    GoalManager boxGoal = Instantiate(boxGoalPrefab, tile.transform);

                    boxGoal.transform.position = tilePos;

                    _goals.Capacity++;

                    _goals.Add(boxGoal);

                }
                else if (tile.data[5].isType)
                {

                    GoalManager playerGoal = Instantiate(playerGoalPrefab, tile.transform);

                    playerGoal.transform.position = tilePos;

                    _goals.Capacity++;

                    _goals.Add(playerGoal);

                }
                    

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

    public BoxMovement GetBox(int x, int y)
    {

        Vector2Int gridCheck = new Vector2Int(x, y);

        for (int i = 0; i < _boxes.Capacity; i++)
        {

            if (_boxes[i].gridPos == gridCheck)
            {

                return _boxes[i];

            }

        }

        return null;

    }

    private IEnumerator InputToLeave()
    {

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene("MainMenu");

    }


}
