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
    public float moveDuration;
    private Tween moveTween;

    public List<Vector2Int> _boxPositions = new List<Vector2Int>();

    private void Awake()
    {
        
        gM = FindObjectOfType<GridManager>();

        gM.GetTile(gridPos.x, gridPos.y).data[BoxData].isType = true;

        _boxPositions.Capacity = 1;

    }

    private void Update()
    {

        undos = _boxPositions.Capacity;

    }

    public bool PushCheck(Vector2Int dir)
    {

        Vector2Int nextpos = gridPos + dir;

        if (gM.GetTile(nextpos.x, nextpos.y).data[1].isType)
            return false;   
        else
        {
            if (gM.GetTile(nextpos.x, nextpos.y).data[4].isType)
            {

                boxGoal = gM.GetTile(nextpos.x, nextpos.y).GetComponentInChildren<GoalManager>();

                tMPro.color = sR.color;

                boxGoal.Activate();

            }

            Move(nextpos, 1);

            return true;
        }
            

    }

    public void Move(Vector2Int moveto, int movemult)
    {

        gM.GetTile(gridPos.x, gridPos.y).data[BoxData].isType = false;

        if (gM.GetTile(gridPos.x, gridPos.y).data[4].isType)
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
