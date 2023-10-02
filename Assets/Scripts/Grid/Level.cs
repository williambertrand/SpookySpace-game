using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectSpawn
{
    public Point pos;
    public GridObjectType type;
    public Point tpPoint;
}


[System.Serializable]
public struct EnemySpawn
{
    public Point pos;
    public int moves;
}


[System.Serializable]
public class Level
{

    public string id;

    public bool useFocus = false;

    public int gridWidth;
    public int gridHeight;

    public Point playerSpawn;

    public List<EnemySpawn> enemySpawns;

    public List<ObjectSpawn> objectSpawns;
}
