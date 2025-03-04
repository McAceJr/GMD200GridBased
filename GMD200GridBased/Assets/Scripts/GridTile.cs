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

    private void Update()
    {

        bool assigned = false;

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

        }

    }

}
