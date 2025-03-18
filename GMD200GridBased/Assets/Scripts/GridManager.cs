using Cinemachine;
using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{

    private int sceneIndex;
    private int levelNumber = 1;
    public string exitTo;

    public bool playerIsTweening;

    public int gridLevel;

    public int numRows = 0;
    public int numCols = 0;

    public Texture2D level;

    public GameObject holderPrefab;
    [SerializeField] private GridTile tilePrefab;
    public PlayerMovement playerPrefab;
    public BoxMovement boxPrefab;
    public GoalManager boxGoalPrefab;
    public GoalManager playerGoalPrefab;
    public GoalManager levelGoalPrefab;
    public LevelLoader levelLoaderPrefab;

    public GameObject lowbound, highbound;
    public GameObject winUI;

    public PlayerMovement player;
    public List<GridTile> _tiles = new List<GridTile>();
    public List<BoxMovement> _boxes = new List<BoxMovement>();
    public List<GoalManager> _goals = new List<GoalManager>();

    private bool uiActive = false;

    public Ease ease;
    public float scaleDuration;
    private Tween scaleTween;

    private void Awake() // double checks that the winui is off and sets capacities for the goals and boxes to 0 to be updated in the initialization
    {

        winUI.SetActive(false);

        _goals.Capacity = 0;

        _boxes.Capacity = 0;

    }

    private void Start() // initializes the grid and centers it at teh start
    {

        InitGrid();

        CenterGrid();

    }

    private void Update() // in update I check for if all the goals have been completed, and I check if the player wants to press R to restart or Z to Undo
    {

        bool complete = true;

        if (!uiActive) // checks if the ui is already activated meaning its been completed
        {
            for (int i = 0; i < _goals.Capacity; i++) // checks all goals and if a single goal isnt active than complete becomes false
            {

                if (!_goals[i].Active)
                {
                    complete = false;
                }
            }
        }

        if (complete) // if complete is never turned false that menas all the goals are on and you get to see the win screen and go back to the main menu
        {


            if (!uiActive)
            {

                bool cleared = false;

                player.moveTween.Kill();

                while(!cleared)
                {

                    cleared = ClearGrid();

                }

                StartCoroutine(InputToLeave());

            }

            winUI.SetActive(true);

            uiActive = true;

        }

        if (Input.GetKeyDown(KeyCode.R) && !complete) // restarts the scene
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }

        if (Input.GetKeyDown(KeyCode.Z) && !complete) // everytime the player moves it records everything's position / states and then pressing Z goes backwards 1 in the list removing the positions/states they used to be at
        {

            if (player._playerPositions.Capacity <= 1) // checks for the only player as long as there is more than 1 state in there meaning you aren't at the start
            {

                return;

            }

            player.MoveTo(player._playerPositions[player.undos - 2], new Vector2Int(0, 0), 0, true);

            player._playerPositions.RemoveAt(player.undos - 1);

            player._playerPositions.Capacity--;

            for (int i = 0; i < _boxes.Capacity; i++) // checks for multiple boxes
            {

                _boxes[i].Move(_boxes[i]._boxPositions[_boxes[i].undos - 2], 0);

                _boxes[i]._boxPositions.RemoveAt(_boxes[i].undos - 1);

                _boxes[i]._boxPositions.Capacity--;

            }

            for (int i = 0; i < _goals.Capacity; i++) // checks for multiple goals 
            {

                _goals[i].Active = _goals[i]._states[_goals[i].undos - 2];

                _goals[i]._states.RemoveAt(_goals[i].undos - 1);

                _goals[i]._states.Capacity--;

            }

        }
    }

    public void InitGrid() // creates the grid reading from an image file and taking the different colors to assign different tiles
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
                else if (tile.data[6].isType)
                {

                    GoalManager levelGoal = Instantiate(levelGoalPrefab, tile.transform);

                    levelGoal.transform.position = tilePos;

                    _goals.Capacity++;

                    _goals.Add(levelGoal);

                }
                else if (tile.data[7].isType)
                {
                    
                    LevelLoader level = Instantiate(levelLoaderPrefab, tile.transform);

                    level.transform.position = tilePos;

                    level.levelPos = new Vector2Int(x, y);

                    level.levelNumber = levelNumber;

                    levelNumber++;
                }


            }
        }

        lowbound.transform.position = _tiles[0].transform.position;

        highbound.transform.position = _tiles[_tiles.Capacity -1].transform.position;

    }

    public void CenterGrid() // post initiation it centers the grid based on the dimensions
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

    public bool ClearGrid() // removes all tiles from grid one by one
    {

        for (int i = 0; i < _tiles.Capacity+3; i++) // Some error is forcing me to have this +3 inside the for loop otherwise it missess the last three tiles might fix later but not sure how this is happening as the tile.capcity never looses value and it can only gain value.
        {

            Destroy(this.transform.GetChild(i).gameObject);

        }

        return true;

    }

    public GridTile GetTile(int col, int row) // finds a specific tile based on the col and row its in and returns the tile's script
    {

        int index = row * numCols + col;
        return _tiles[index];

    }

    public BoxMovement GetBox(int x, int y) // finds a specific box based on its grid pos and returns it's script
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

    private IEnumerator InputToLeave() // just delays the transition to main menu
    {

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(exitTo);

    }


}
