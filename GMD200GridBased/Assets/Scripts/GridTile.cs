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
    6 Level Goal Tile
    7 Level Button Tile
    */

    /*
     is called when assigning the type which is color based but after it only keeps the necessary
     colors removing the player start pos and box start pos colored tiles making them white.
     */
    public void AssignType()
    {

        bool found = false;

        for (int i =0; i < data.Length; i++)
        {

            if (spr.color == data[i].typeColor)
            {

                data[i].isType = true;

                found = true;

                if (data[i].typeColor == data[(int)TileType.Player].typeColor)
                    spr.color = Color.white;
                else if (data[i].typeColor == data[(int)TileType.Box].typeColor)
                    spr.color = Color.white;
                else if (data[i].typeColor == data[(int)TileType.LevelGoal].typeColor)
                    data[1].isType = true;
                

            }

        }

        if (!found)
        {

            /*
             the index of 0 for data is the defualt / empty square so if the color isnt found then make the square empty
             if its not found it will also make the tile black to make invisible paths
             */

            data[(int)TileType.Empty].isType = true;

            spr.color = Color.black;

        }

    }

}
