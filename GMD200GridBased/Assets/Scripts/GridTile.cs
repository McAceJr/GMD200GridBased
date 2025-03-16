using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [System.Serializable]
    public struct tileType
    {
        public string name;
        public Color typeColor;
        public bool isType;

    };

    public SpriteRenderer spr;

    public tileType[] data;
    /*
    0 Empty Tile
    1 Wall Tile
    2 Player Tile
    3 Box Tile
    4 Box Goal Tile
    5 Player Goal Tile
    6 Door Tile
    7 Door Switch Tile
    */

    public void AssignType() // is called when assigning the type which is color based but after it only keeps the necessary colors removing the player start pos and box start pos colored tiles making them white
    {

        bool found = false;

        for (int i =0; i < data.Length; i++)
        {

            if (spr.color == data[i].typeColor)
            {

                data[i].isType = true;

                found = true;

                if (data[i].typeColor == data[2].typeColor)
                    spr.color = Color.white;
                else if (data[i].typeColor == data[3].typeColor)
                    spr.color = Color.white;

            }

        }

        if (!found)
        {

            data[0].isType = true; // the index of 0 for data is the defualt / empty square so if the color isnt found then make the square empty

            spr.color = Color.white;

        }

    }

    private void Update() // going to be remove later
    {

        /*bool assigned = false;

        for (int i = 0; i < data.Length; i++)
        {

            if (data[i].isType)
            {
                
                spr.color = data[i].typeColor;

                assigned = true;

            }

        }

        if (!assigned)
        {

            spr.color = Color.white;

        }*/

    }

}
