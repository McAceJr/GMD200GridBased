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

    public float coolTime;
    private float downTime = 0;
    [SerializeField] private bool cooldown = false;

    [SerializeField] private GridManager gM;

    private void Awake()
    {

    }

    private void Update()
    {

        Vector3 targetpos = gM.GetTile(gridPos.x, gridPos.y).transform.position;

        transform.position = Vector3.MoveTowards(transform.position, targetpos, transitionSpeed * Time.deltaTime);

        if (!cooldown)
            downTime++;

        if (downTime >= coolTime)
        {
            downTime = 0;
            cooldown = true;
        }

        Debug.Log(downTime);

        if (Vector3.Distance(transform.position, targetpos) < 0.001f)
        {
            if (Input.GetKey(KeyCode.RightArrow) && gridPos.x < gM.numRows && cooldown)
            {
                gridPos.x++;
                cooldown = false;
            }
            if (Input.GetKey(KeyCode.LeftArrow) && gridPos.x > 0 && cooldown)
            {
                gridPos.x--;
                cooldown = false;
            }
            if (Input.GetKey(KeyCode.DownArrow) && gridPos.y > 0 && cooldown)
            {
                gridPos.y--;
                cooldown = false;
            }
            if (Input.GetKey(KeyCode.UpArrow) && gridPos.y < gM.numCols - 2 && cooldown)
            {
                gridPos.y++;
                cooldown = false;
            }
        }

    }

}
