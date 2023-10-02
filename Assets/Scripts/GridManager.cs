using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GridObjectType
{
    Block,
    Switch,
    TP
}

public class GridManager : MonoBehaviour
{ 
    [SerializeField] private GameObject gridCell;
    [SerializeField] private GameObject blockedGridCell;
    [SerializeField] private GameObject switchGridCell;
    [SerializeField] private GameObject tpGridObject;

    [SerializeField] private int _width;
    [SerializeField] private int _height;

    public float gridSpaceSize;

    public static GridManager Instance;

    [SerializeField] public Point PlayerStartPos;

    private Dictionary<Point, Cell> gridCells;
    private List<GameObject> _cellGameObjects;
    private List<GameObject> _switchesGameObjects;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        _cellGameObjects = new List<GameObject>();
        _switchesGameObjects = new List<GameObject>();
    }

    public void SpawnGrid(int width, int height)
    {
        ClearGrid();
        gridCells = new Dictionary<Point, Cell>();
        _width = width;
        _height = height;

        for (var i = 0; i < width; i++)
        {
            for(var j = 0; j < height; j++)
            {
                Point p = new Point(i, j);
                Vector3 pos = PositionFromPoint(p);
                
                Cell c = new Cell(p);
                c.isBlocked = false;

                // TODO: Blocked cells and obstacles
                GameObject objToSpawn = gridCell;
                GridObjectType? objectAtPos = null;

                if (objectAtPos == GridObjectType.Block)
                {
                    c.isBlocked = true;
                    objToSpawn = blockedGridCell;
                }

                gridCells.Add(p, c);
                GameObject obj = Instantiate(objToSpawn, pos, Quaternion.identity);
                _cellGameObjects.Add(obj);
            }
        }
    }

    public void SpawnObstacles(List<ObjectSpawn> spawns)
    {
        foreach(ObjectSpawn spawn in spawns)
        {
            Cell cell = gridCells.GetValueOrDefault(spawn.pos);
            cell.type = spawn.type;
            switch (spawn.type)
            {
                case GridObjectType.Block:
                    cell.isBlocked = true;
                    GameObject g = Instantiate(blockedGridCell, PositionFromPoint(spawn.pos), Quaternion.identity);
                    _cellGameObjects.Add(g);
                    break;
                case GridObjectType.Switch:
                    cell.isBlocked = true;
                    GameObject switchCell = Instantiate(switchGridCell, PositionFromPoint(spawn.pos), Quaternion.identity);
                    _cellGameObjects.Add(switchCell);
                    _switchesGameObjects.Add(switchCell);
                    break;
                case GridObjectType.TP:
                    cell.type = GridObjectType.TP;
                    cell.linkedTPPoint = spawn.tpPoint;
                    GameObject tPCell = Instantiate(tpGridObject, PositionFromPoint(spawn.pos), Quaternion.identity);
                    _cellGameObjects.Add(tPCell);
                    break;
                default:
                    break;
            }
        }
    }

    private void ClearGrid()
    {
        for(int i = 0; i < _cellGameObjects.Count; i++)
        {
            // TODO: Cell pop/dissolve effect
            Destroy(_cellGameObjects[i]);
        }
        _cellGameObjects.Clear();
        _cellGameObjects = new List<GameObject>();

        for (int i = 0; i < _switchesGameObjects.Count; i++)
        {
            // TODO: Cell pop/dissolve effect
            Destroy(_switchesGameObjects[i]);
        }
        _switchesGameObjects.Clear();
        _switchesGameObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 PositionFromPoint(Point p)
    {
        return new Vector3(gridSpaceSize * p.x, gridSpaceSize * p.y, 0.0f);
    }

    private bool IsInBounds(Point p)
    {
        if (p.x < 0 || p.x >= _width) return false;
        if (p.y < 0 || p.y >= _height) return false;
        return true;
    }

    public bool IsCellAvailable(Point p)
    {
        if (!IsInBounds(p)) return false;
        Cell cell = gridCells.GetValueOrDefault(p);

        if (cell == null) return false;

        if(cell.type == GridObjectType.Switch)
        {
            // Next turn, cell will be un-blocked
            return cell.isBlocked;
        }

        // Go through cell checks
        if (cell.isBlocked) return false;


        return true;
    }

    public void OnPlayerDidMove()
    {
        Point pos = Player.Instance.currentPositon;
        Cell cell = gridCells.GetValueOrDefault(pos);

        if(cell.type == GridObjectType.TP)
        {
            Point toPoint = (Point)cell.linkedTPPoint;
            Player.Instance.Movement.SetPosition(toPoint);
        }

        foreach (Cell c in gridCells.Values)
        {
            if (c.type == GridObjectType.Switch)
            {
                c.isBlocked = !c.isBlocked;
                if(c.isBlocked)
                {
                    // How to tie cell to the in game object?
                }
            }
        }

        foreach(GameObject s in _switchesGameObjects)
        {
            SpriteRenderer sr = s.GetComponent<SpriteRenderer>();
            Color c = sr.color;
            if(c.a == 1)
            {
                c.a = 0.25f;
            } else
            {
                c.a = 1;
            }
            sr.color = c;
        }
    }
}
