using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float transitionSpeed;

    public Vector2Int gridPos = Vector2Int.zero;

    static int MAX = 4;
    public float coolTime;
    private float[] downTime = new float[MAX];
    [SerializeField] private bool[] cooldown = new bool[MAX]; // bools for the 4 directions of movement

    [SerializeField] private GridManager gM;

    private void Awake()
    {

        for (int i = 0; i < MAX; i++)
            cooldown[i] = true;

        for (int i = 0; i < MAX; i++)
            downTime[i] = 0;

    }

    private void Update()
    {

        Vector3 targetpos = gM.GetTile(gridPos.x, gridPos.y).transform.position;

        gM.GetTile(gridPos.x, gridPos.y).player = true;

        transform.position = Vector3.MoveTowards(transform.position, targetpos, transitionSpeed * Time.deltaTime);

        for (int i = 0; i < MAX; i++)
        {
            if (!cooldown[i])
                downTime[i]++;

            if (downTime[i] >= coolTime && !cooldown[i])
            {
                downTime[i] = 0;
                cooldown[i] = true;
            }
        }

        Debug.Log(downTime);

        if (Vector3.Distance(transform.position, targetpos) < 0.001f)
        {
            if (Input.GetKey(KeyCode.RightArrow) && gridPos.x < gM.numRows && cooldown[0])
            {
                gridPos.x++;
                cooldown[0] = false;
            }
            if (Input.GetKey(KeyCode.LeftArrow) && gridPos.x > 0 && cooldown[1])
            {
                gridPos.x--;
                cooldown[1] = false;
            }
            if (Input.GetKey(KeyCode.DownArrow) && gridPos.y > 0 && cooldown[2])
            {
                gridPos.y--;
                cooldown[2] = false;
            }
            if (Input.GetKey(KeyCode.UpArrow) && gridPos.y < gM.numCols - 2 && cooldown[3])
            {
                gridPos.y++;
                cooldown[3] = false;
            }
        }

    }

}
