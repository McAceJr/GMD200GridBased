using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SHAKEMODE : MonoBehaviour
{

    public bool stayactive;

    public float minMax;

    private Vector3 positionShake;

    private GridManager gM;
    private GridTile[] gridTile;

    private void Start()
    {

        if (stayactive)
        {

            ToggleShakeMode(stayactive);

        }

        gM = FindObjectOfType<GridManager>();

        gridTile = FindObjectsOfType<GridTile>();

    }

    private void Update()
    {

        if (stayactive && !gM.complete)
        {
            for (int i = 0; i < gridTile.Length; i++)
            {

                positionShake = new Vector3(Random.Range(-minMax, minMax), Random.Range(-minMax, minMax), Random.Range(-minMax, minMax));

                gridTile[i].transform.position += positionShake;

            }
        }

    }

    public void ToggleShakeMode(bool ison)
    {

        stayactive = ison;



    }

}
