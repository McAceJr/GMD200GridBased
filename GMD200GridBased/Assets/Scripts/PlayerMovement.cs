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

        /*for (int i = 0; i < MAX; i++)
            cooldown[i] = true;

        for (int i = 0; i < MAX; i++)
            downTime[i] = 0;*/

    }

    private void Update()
    {

        Vector3 targetpos = gM.GetTile(gridPos.x, gridPos.y).transform.position;

        gM.GetTile(gridPos.x, gridPos.y).data[0].isType = true;

        transform.position = Vector3.MoveTowards(transform.position, targetpos, transitionSpeed * Time.deltaTime);

        /*for (int i = 0; i < MAX; i++)
        {
            if (!cooldown[i])
                downTime[i]++;

            if (downTime[i] >= coolTime && !cooldown[i])
            {
                downTime[i] = 0;
                cooldown[i] = true;
            }
        }*/

        if (Vector3.Distance(transform.position, targetpos) < 0.001f)
        {
            if (Input.GetKey(KeyCode.RightArrow) && gridPos.x < gM.numCols - 1)
            {

                if (gM.GetTile(gridPos.x + 1, gridPos.y).data[1].isType != true)
                {

                    gM.GetTile(gridPos.x, gridPos.y).data[0].isType = false;

                    gridPos.x++;

                }

            }
            if (Input.GetKey(KeyCode.LeftArrow) && gridPos.x > 0)
            {

                if (gM.GetTile(gridPos.x - 1, gridPos.y).data[1].isType != true)
                {

                    gM.GetTile(gridPos.x, gridPos.y).data[0].isType = false;

                    gridPos.x--;

                }

            }
            if (Input.GetKey(KeyCode.DownArrow) && gridPos.y > 0)
            {

                if (gM.GetTile(gridPos.x, gridPos.y - 1).data[1].isType != true)
                {

                    gM.GetTile(gridPos.x, gridPos.y).data[0].isType = false;

                    gridPos.y--;

                }

            }
            if (Input.GetKey(KeyCode.UpArrow) && gridPos.y < gM.numRows - 1)
            {

                if (gM.GetTile(gridPos.x, gridPos.y + 1).data[1].isType != true)
                {

                    gM.GetTile(gridPos.x, gridPos.y).data[0].isType = false;

                    gridPos.y++;

                }

            }
        }

    }

}
