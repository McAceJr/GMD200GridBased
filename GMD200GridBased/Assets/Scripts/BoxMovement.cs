using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxMovement : MonoBehaviour
{

    public int undos;

    [SerializeField] private GridManager gM;

    public Vector2Int gridPos;

    private int BoxData = 3;

    private GoalManager boxGoal;

    public SpriteRenderer sR;
    public TextMeshPro tMPro;

    public Ease ease;
    public float moveDurFast, moveDurNorm;
    private float moveDuration;
    private Tween moveTween;

    public List<Vector2Int> _boxPositions = new List<Vector2Int>();

    private void Awake() // very similar to playermovement
    {
        
        gM = FindObjectOfType<GridManager>();

        gM.GetTile(gridPos.x, gridPos.y).data[BoxData].isType = true;

        _boxPositions.Capacity = 1;

    }

    private void Update() // the if statement makes sure that the tile the box is on is a box tile to signify the player that there is a box there, and there is no need to make it remove the is type because the player always double checks if there is actually a box there
    {

        undos = _boxPositions.Capacity;

        if (!gM.GetTile(gridPos.x, gridPos.y).data[3].isType)
        {

            gM.GetTile(gridPos.x, gridPos.y).data[3].isType = true;

        }

        if (Input.GetKey(KeyCode.LeftShift))
        {

            moveDuration = moveDurFast;

        }
        else
        {

            moveDuration = moveDurNorm;

        }

    }

    public bool PushCheck(Vector2Int dir) // this makes sure the box can be pushed which causes loops based on how many boxes are in a row that you are pushing and it will keep going ontill there are no more boxes and it runs into an empty tile (with or without a goal) or a wall returning a bool if the player can push them and move into their spot
    {

        Vector2Int nextpos = gridPos + dir;

        if (gM.GetTile(nextpos.x, nextpos.y).data[(int)TileType.Wall].isType)
            return false;
        else if (gM.GetTile(nextpos.x, nextpos.y).data[(int)TileType.Box].isType)
        {
            bool nextBoxCheck;

            BoxMovement adjbox = gM.GetBox(nextpos.x, nextpos.y);

            nextBoxCheck = adjbox.PushCheck(dir);

            if (nextBoxCheck)
            {

                if (gM.GetTile(nextpos.x, nextpos.y).data[(int)TileType.BoxGoal].isType)
                {

                    boxGoal = gM.GetTile(nextpos.x, nextpos.y).GetComponentInChildren<GoalManager>();

                    tMPro.color = sR.color;

                    boxGoal.Activate();

                }

                Move(nextpos, 1);
            }

            return nextBoxCheck;

            

        }
        else
        {
            if (gM.GetTile(nextpos.x, nextpos.y).data[(int)TileType.BoxGoal].isType)
            {

                boxGoal = gM.GetTile(nextpos.x, nextpos.y).GetComponentInChildren<GoalManager>();

                tMPro.color = sR.color;

                boxGoal.Activate();

            }

            Move(nextpos, 1);

            return true;
        }
            

    }

    public void Move(Vector2Int moveto, int movemult) // actually handles the move and the move mult is just so that undooing can look different from moving
    {

        gM.GetTile(gridPos.x, gridPos.y).data[BoxData].isType = false;

        if (gM.GetTile(gridPos.x, gridPos.y).data[(int)TileType.BoxGoal].isType)
        {

            boxGoal = gM.GetTile(gridPos.x, gridPos.y).GetComponentInChildren<GoalManager>();

            tMPro.color = Color.white;

            boxGoal.Deactivate();

        }
        
        Vector3 updatedPos = gM.GetTile(moveto.x, moveto.y).transform.position;

        moveTween = transform.DOMove(updatedPos, moveDuration*movemult).SetEase(ease);

        gridPos = moveto;

        gM.GetTile(gridPos.x, gridPos.y).data[BoxData].isType = true;

    }

}
