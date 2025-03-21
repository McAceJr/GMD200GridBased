using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalManager : MonoBehaviour
{

    public bool complete;
    private static bool completed;

    public int levelNumber;

    public int undos;

    [SerializeField] private GridManager gM;
    public SpriteRenderer sR;
    public TextMeshPro tMPro;

    public Color on, off;

    public string ontxt, offtxt;

    public Vector2Int goalPos;

    public bool active;

    public List<bool> _states = new List<bool>();

    private void Awake() // resets the states to 1 and adds a false state signaling the starting state
    {

        _states.Capacity = 1;

        _states.Add(false);

    }

    private void Update() // checks if the bool has been triggered and if it has thenn it should be on otherwise off
    {

        completed = complete;

        if (completed)
        {

            Activate();

        }

        undos = _states.Capacity;

        if (active)
        {

            sR.color = on;

            tMPro.text = ontxt;

            tMPro.color = Color.white;

        }
        else if (!active)
        {

            tMPro.text = offtxt;

            sR.color = off;

            tMPro.color = Color.black;

        }

    }

    public void Activate() //called by other functions to active goal
    {

        active = true;

    }

    public void Deactivate() //called by other functions to deactivate goal
    {

        active = false;

    }
}
