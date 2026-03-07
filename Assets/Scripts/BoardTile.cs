using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public int x, y;
    private int isOccupied;


    public void SetHighLight(bool active)
    {
               GetComponent<Renderer>().material.color = active ? Color.yellow : Color.white;
    }

}
