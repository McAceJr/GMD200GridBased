using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    private GridManager gM;

    private void Awake()
    {

        gM = GetComponent<GridManager>();

    }

    public void Generate()
    {

        gM.InitGrid();

    }

}
