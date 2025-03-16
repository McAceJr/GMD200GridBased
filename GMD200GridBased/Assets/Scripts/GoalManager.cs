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

    private void Awake() // resets the states to 1 and adds a false state signaling the starting state
    {
        
        _states.Capacity = 1;

        _states.Add(false);

    }

    private void Update() // checks if the bool has been triggered and if it has thenn it should be on otherwise off
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

    public void Activate() //called by other functions to active goal
    {

        Active = true;

    }

    public void Deactivate() //called by other functions to deactivate goal
    {

        Active = false;

    }
}
