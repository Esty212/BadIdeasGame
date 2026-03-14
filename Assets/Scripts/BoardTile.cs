using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public int x, y;
    public bool isWalkable = true;

    private GameObject occupant;

    public bool IsOccupied => occupant != null;

    public void SetOccupant(GameObject obj)
    {
        occupant = obj;
    }

    public void ClearOccupant()
    {
        occupant = null;
    }

    public GameObject GetOccupant()
    {
        return occupant;
    }

    public void SetHighLight(bool active)
    {
        GetComponent<Renderer>().material.color = active ? Color.yellow : Color.white;
    }
}
