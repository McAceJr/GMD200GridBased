using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLoader : MonoBehaviour
{

    public Vector2Int levelPos;

    public int worldNumber;
    public int levelNumber;
    private string levelName;

    public PlayerMovement pM;
    public GridManager gM;

    public TextMeshPro lvlText;

    /*
     After the whole grid is instantiated the level tiles locate the player create their name and set their text to the name
     */
    private void Start() 
    {

        pM = FindObjectOfType<PlayerMovement>();

        gM = FindObjectOfType<GridManager>();

        levelName = worldNumber + "-" + levelNumber;

        lvlText.text = levelName;

    }


    /*
     Constantly checks if the player is on the same tile pos and if the space key is down then tries to load the level
     */
    private void Update()
    {

        if (pM.gridPos == levelPos)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                SceneManager.LoadScene(levelName);

            }
        }

    }

}
