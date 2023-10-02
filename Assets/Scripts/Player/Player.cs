using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance;
    public PlayerGridMovement Movement;
    public Point currentPositon {
        get
        {
            return Movement.currentPosition;
        }
    }

    public bool IsDead;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        Movement = GetComponent<PlayerGridMovement>();
    }

    private void Start()
    {
        IsDead = false;
    }

    public void Reset()
    {
        IsDead = false;
    }
}
