using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public abstract class BaseEnemy: MonoBehaviour
{
    public abstract Point PickNextMove();
}

public class Enemy : BaseEnemy
{

    // TODO: Could define some common MoveableGridObject that has positon functions
    private Point _currentPosition;
    public int MoveCount;

    [SerializeField] private TMP_Text moveText;

    private bool _isFacingLeft = true;

    private Animator _animator;

    public delegate void OnDeath(Enemy e);
    public OnDeath onDeath;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
    }

    public void Init(Point p, int moves)
    {
        _currentPosition = p;
        MoveCount = moves;
        UpdatePosition();
        UpdateUI();
    }

    // TODO: Check if next picked cell is available to move to
    public override Point PickNextMove()
    {
        Point playerPoint = Player.Instance.currentPositon;
        int xDif = playerPoint.x - _currentPosition.x;
        int yDif = playerPoint.y - _currentPosition.y;

        if (xDif > 0 && !_isFacingLeft) Flip();

        if(Mathf.Abs(xDif) + Mathf.Abs(yDif) <= 1)
        {
            _animator.SetTrigger("Attack");
            GamePlayManager.Instance.OnNotifyPlayerHit();
            return playerPoint;
        }

        int yMove = yDif > 0 ? 1 : -1;
        Point tryPointy = new Point(_currentPosition.x, _currentPosition.y + yMove);
        

        if (Mathf.Abs(xDif) >= Mathf.Abs(yDif) || !GridManager.Instance.IsCellAvailable(tryPointy))
        {
            int xMove = xDif > 0 ? 1 : -1;
            Point tryPoint = new Point(_currentPosition.x + xMove, _currentPosition.y);
            if (GridManager.Instance.IsCellAvailable(tryPoint))
            {
                return tryPoint;
            }
        }
        

        return new Point(_currentPosition.x, _currentPosition.y + yMove);
    }

    public void Move()
    {

        if(MoveCount == 0)
        {
            return;
        }
        Point p = PickNextMove();
        _currentPosition = p;
        MoveCount -= 1;
        UpdateUI();

        if (MoveCount == 0)
        {
            StartCoroutine(DestoryAfter());
        }
    }

    public void UpdatePosition()
    {
        this.transform.position = GridManager.Instance.PositionFromPoint(_currentPosition);
    }

    private void HandleMove()
    {
        if (Mathf.Abs(Vector2.Distance(this.transform.position, GridManager.Instance.PositionFromPoint(_currentPosition))) < 0.005f) return;
        this.transform.position = Vector2.MoveTowards(transform.position, GridManager.Instance.PositionFromPoint(_currentPosition), 2.5f * Time.deltaTime);
    }

    private void UpdateUI()
    {
        this.moveText.text = "" + MoveCount;
    }

    IEnumerator DestoryAfter()
    {
        yield return new WaitForSeconds(0.5f);
        _animator.SetTrigger("Death");  

        yield return new WaitForSeconds(1.0f);
        onDeath?.Invoke(this);
        Destroy(gameObject);
    }

    private void Flip()
    {
        _isFacingLeft = !_isFacingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
