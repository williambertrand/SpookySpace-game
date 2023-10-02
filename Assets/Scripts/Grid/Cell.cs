using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Point point;
    public bool isBlocked;

    public GridObjectType? type;

    public Point? linkedTPPoint;

    public Cell(Point p)
    {
        point = p;
        type = null;
        linkedTPPoint = null;
    }

    public string ToString()
    {
        return "P: (" + point.x + "," + point.y + "), isBlocked: " + isBlocked;
    }
}
