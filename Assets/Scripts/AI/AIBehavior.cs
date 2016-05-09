using UnityEngine;
using Assets.Scripts.Board;
using Assets.Scripts.Util;

public abstract class AIBehavior : MonoBehaviour
{

    protected Board board;

    protected PositionConverter positionConverter;

    private bool isMoving;

    // Use this for initialization
    public virtual void Start()
    {
        board = GameObject.Find(Constants.GAME_MANAGER_NAME)
            .GetComponent<GameManager>().GetBoard();
        positionConverter = GameObject.Find(Constants.GAME_MANAGER_NAME)
            .GetComponent<GameManager>().GetPositionConverter();
        TryMove();
    }

    void Update()
    {
        if (!isMoving)
        {
            TryMove();
        }
    }

    protected abstract GameCell GetNextMove();

    private void TryMove()
    {
        GameCell nextCell = GetNextMove();
        if (nextCell != null)
        {
            isMoving = true;
            MakeMove(nextCell);
        }
        else
        {
            isMoving = false;
        }
    }

    void MakeMove(GameCell targetCell)
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("isWalk", true);
        transform.LookAt(positionConverter.ConvertBoardPositionToScene(targetCell.GetCoordinates(), true));
        iTween.MoveTo(this.gameObject, iTween.Hash(
                "position", positionConverter.ConvertBoardPositionToScene(targetCell.GetCoordinates(), true),
                "oncomplete", "TryMove",
                "time", 3,
                "easetype", "linear"));
    }
}
