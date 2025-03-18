using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Vector2Int levelPos;

    public int worldNumber;
    public int levelNumber;
    private string levelName;

    public PlayerMovement pM;

    private void Awake()
    {
        
        pM = FindObjectOfType<PlayerMovement>();

        levelName = worldNumber + "-" + levelNumber;

    }

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
