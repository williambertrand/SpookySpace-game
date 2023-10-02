using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct Point
{
    public int x;
    public int y;

    public Point(int x, int y) : this()
    {
        this.x = x;
        this.y = y;
    }
}

public class PlayerGridMovement : MonoBehaviour
{
    
    public Point currentPosition;

    public delegate void OnPlayerMove();
    public OnPlayerMove onDidMove;

    [SerializeField] private float moveCoolDown;
    private float lastMove;

    private Animator _animator;

    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
        currentPosition = GridManager.Instance.PlayerStartPos;
        UpdatePosition();
        onDidMove += GridManager.Instance.OnPlayerDidMove;
        onDidMove += GamePlayManager.Instance.OnPlayerDidMove;

        _animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        onDidMove -= GridManager.Instance.OnPlayerDidMove;
        onDidMove -= GamePlayManager.Instance.OnPlayerDidMove;
    }

    void Update() {
        HandleMove();
    }


    public void OnPlayerMoveRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        HandlePlayerMove(1, 0);
    }

    public void OnPlayerMoveLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        HandlePlayerMove(-1, 0);
    }

    public void OnPlayerMoveUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        HandlePlayerMove(0, 1);
    }

    public void OnPlayerMoveDown(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        HandlePlayerMove(0, -1);
    }

    private void HandlePlayerMove(int xDir, int yDir)
    {
        if (player.IsDead) return;
        if (Time.time - lastMove <= moveCoolDown) return;

        Point newP = new Point(currentPosition.x + xDir, currentPosition.y + yDir);
        if (!CanMoveTo(newP))
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayOneShot(AudioEvent.Player_NO_Move);
            return;
        }

        int moveDir = 0;
        if (xDir == 1) moveDir = 1;
        else if (xDir == -1) moveDir = 2;
        else if (yDir == 1) moveDir = 3;
        else if (yDir == -1) moveDir = 4;


        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayOneShotForMoveDir(moveDir);

        currentPosition = newP;

        onDidMove?.Invoke();
        lastMove = Time.time;
    }

    private bool CanMoveTo(Point p)
    {
        if (!GridManager.Instance.IsCellAvailable(p)) return false;
        // Check for space open
        return true;
    }

    private void UpdatePosition()
    {
        this.transform.position = GridManager.Instance.PositionFromPoint(currentPosition);
    }

    public void SetPosition(Point p)
    {
        currentPosition = p;
        UpdatePosition();
    }

    private void HandleMove()
    {
        if (Mathf.Abs(Vector2.Distance(this.transform.position, GridManager.Instance.PositionFromPoint(currentPosition))) < 0.005f)
        {
            _animator.SetBool("Moving", false);
            return;
        }
        _animator.SetBool("Moving", true);
        this.transform.position = Vector2.MoveTowards(transform.position, GridManager.Instance.PositionFromPoint(currentPosition), 3.5f * Time.deltaTime);
    }
}
