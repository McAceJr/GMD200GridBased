using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    /*public float transitionSpeed;*/
    public int undos;

    public Vector2Int gridPos;

    private Vector2Int nextPos;
    /*static int MAX = 4;*/
    /*public float coolTime;
    private float[] downTime = new float[MAX];*/ // Old stuff that is no longer used
    /*[SerializeField] private bool[] cooldown = new bool[MAX]; // bools for the 4 directions of movement*/

    [SerializeField] private GridManager gM;
    private GoalManager playerGoal;

    public SpriteRenderer sR;
    public TextMeshPro tMPro;

    public Ease ease;
    public float moveDuration;
    public Tween moveTween;

    public List<Vector2Int> _playerPositions = new List<Vector2Int>();

    private void Awake()
    {

        gM = FindObjectOfType<GridManager>();

        /*for (int i = 0; i < MAX; i++)
            cooldown[i] = true;

        for (int i = 0; i < MAX; i++)
            downTime[i] = 0;*/

        _playerPositions.Capacity = 1;

        _playerPositions.Add(gridPos);

    }

    public void MoveTo(Vector2Int newpos, Vector2Int dir, int movemult, bool undoing)
    {

        bool moved = false;

        Vector2Int oldpos;

        oldpos = gridPos;

        if (moveTween != null && moveTween.IsActive())
            return;

        Vector3 targetpos = gM.GetTile(newpos.x, newpos.y).transform.position;

        Vector3 stillpos = gM.GetTile(oldpos.x, oldpos.y).transform.position;

        if (gM.GetTile(newpos.x, newpos.y).data[1].isType != true && gM.GetTile(newpos.x, newpos.y).data[3].isType != true)
        {

            //gM.GetTile(oldpos.x, oldpos.y).data[0].isType = false;

            moveTween = transform.DOMove(targetpos, moveDuration * movemult).SetEase(ease);

            

            moved = true;

        }
        else if (gM.GetTile(newpos.x, newpos.y).data[3].isType)
        {

            bool canMove = gM.GetBox(newpos.x, newpos.y).PushCheck(dir);

            if (canMove)
            {
                moveTween = transform.DOMove(targetpos, moveDuration * movemult).SetEase(ease);

                moved = true;

            }
            else
            {
                moveTween = transform.DOMove(stillpos, moveDuration * movemult).SetEase(ease);

                moved = false;
            }
                

        }
        else if (gM.GetTile(newpos.x, newpos.y).data[1].isType)
        {

            moveTween = transform.DOMove(stillpos, moveDuration * movemult).SetEase(ease);

            moved = false;

        }

        if (moved)
        {

            gridPos = newpos;

            if (gM.GetTile(newpos.x, newpos.y).data[5].isType)
            {

                playerGoal = gM.GetTile(newpos.x, newpos.y).GetComponentInChildren<GoalManager>();

                tMPro.color = sR.color;

                playerGoal.Activate();

            }

            if (gM.GetTile(oldpos.x, oldpos.y).data[5].isType)
            {

                playerGoal = gM.GetTile(oldpos.x, oldpos.y).GetComponentInChildren<GoalManager>();

                tMPro.color = Color.white;

                playerGoal.Deactivate();

            }

            gM.GetTile(oldpos.x, oldpos.y).data[2].isType = false;
            gM.GetTile(newpos.x, newpos.y).data[2].isType = true;

        }

        if (!undoing)
        {
            _playerPositions.Capacity++;

            _playerPositions.Add(gridPos);

            for (int i = 0; i < gM._boxes.Capacity; i++)
            {

                gM._boxes[i]._boxPositions.Capacity++;

                gM._boxes[i]._boxPositions.Add(gM._boxes[i].gridPos);

            }

            for (int i = 0; i < gM._goals.Capacity; i++)
            {
                gM._goals[i]._states.Capacity++;

                gM._goals[i]._states.Add(gM._goals[i].Active);
            }
        }
    }

    private void Update()
    {

        undos = _playerPositions.Capacity;

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

            MoveTo(gridPos + nextPos, nextPos, 1, false);

        }
        if (Input.GetKey(KeyCode.LeftArrow) && gridPos.x > 0)
        {

            nextPos.x--;

            MoveTo(gridPos + nextPos, nextPos, 1, false);

        }
        if (Input.GetKey(KeyCode.DownArrow) && gridPos.y > 0)
        {

            nextPos.y--;

            MoveTo(gridPos + nextPos, nextPos, 1, false);

        }
        if (Input.GetKey(KeyCode.UpArrow) && gridPos.y < gM.numRows - 1)
        {

            nextPos.y++;

            MoveTo(gridPos + nextPos, nextPos, 1, false);

        }

    }

}
