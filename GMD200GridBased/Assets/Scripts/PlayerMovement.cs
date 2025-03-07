using DG.Tweening;
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

    private Vector2Int nextPos;
    static int MAX = 4;
    public float coolTime;
    private float[] downTime = new float[MAX];
    [SerializeField] private bool[] cooldown = new bool[MAX]; // bools for the 4 directions of movement

    [SerializeField] private GridManager gM;

    public Ease ease;
    public float moveDuration;
    private Tween moveTween;
    private void Awake()
    {

        gM = FindObjectOfType<GridManager>();

        /*for (int i = 0; i < MAX; i++)
            cooldown[i] = true;

        for (int i = 0; i < MAX; i++)
            downTime[i] = 0;*/

    }

    void MoveTo(Vector2Int newpos)
    {

        Vector2Int oldpos;

        oldpos = gridPos;

        if (moveTween != null && moveTween.IsActive())
            return;

        gridPos = newpos;
        
        Vector3 targetpos = gM.GetTile(gridPos.x, gridPos.y).transform.position;

        if (gM.GetTile(newpos.x, newpos.y).data[1].isType != true)
        {

            //gM.GetTile(oldpos.x, oldpos.y).data[0].isType = false;

            moveTween = transform.DOMove(targetpos, moveDuration).SetEase(ease);

        }

        
        
    }

    private void Update()
    {

        //Vector3 targetpos = gM.GetTile(gridPos.x, gridPos.y).transform.position;

        //gM.GetTile(gridPos.x, gridPos.y).data[0].isType = true;

        //transform.position = Vector3.MoveTowards(transform.position, targetpos, transitionSpeed * Time.deltaTime);

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

        nextPos = new Vector2Int(0,0);

        if (Input.GetKey(KeyCode.RightArrow) && gridPos.x < gM.numCols - 1)
        {

            nextPos.x++;

            MoveTo(gridPos + nextPos);

        }
        if (Input.GetKey(KeyCode.LeftArrow) && gridPos.x > 0)
        {

            nextPos.x--;

            MoveTo(gridPos + nextPos);

        }
        if (Input.GetKey(KeyCode.DownArrow) && gridPos.y > 0)
        {

            nextPos.y--;

            MoveTo(gridPos + nextPos);

        }
        if (Input.GetKey(KeyCode.UpArrow) && gridPos.y < gM.numRows - 1)
        {

            nextPos.y++;

            MoveTo(gridPos + nextPos);

        }

        

    }

}
