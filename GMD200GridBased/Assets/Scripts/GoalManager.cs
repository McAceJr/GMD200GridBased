using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalManager : MonoBehaviour
{

    public int undos;

    [SerializeField] private GridManager gM;
    public SpriteRenderer sR;
    public TextMeshPro tMPro;

    public Color on, off;

    public Vector2Int goalPos;

    public bool Active;

    public List<bool> _states = new List<bool>();

    private void Awake()
    {
        
        _states.Capacity = 1;

        _states.Add(false);

    }

    private void Update()
    {
        
        undos = _states.Capacity;

        if (Active)
        {

            sR.color = on;

            tMPro.color = Color.white;

        }
        else if (!Active)
        {

            sR.color = off;

            tMPro.color = Color.black;

        }


    }

    public void Activate()
    {

        Active = true;

    }

    public void Deactivate() 
    {

        Active = false;

    }
}
