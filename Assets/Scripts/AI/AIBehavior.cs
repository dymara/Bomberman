using UnityEngine;
using Assets.Scripts.Board;
using Assets.Scripts.Util;

public abstract class AIBehavior : MonoBehaviour
{

    protected Board board;

    protected PositionConverter positionConverter;

    // Use this for initialization
    public virtual void Start()
    {
        board = GameObject.Find(Constants.GAME_MANAGER_NAME)
            .GetComponent<GameManager>().GetBoard();
        positionConverter = GameObject.Find(Constants.GAME_MANAGER_NAME)
            .GetComponent<GameManager>().GetPositionConverter();
        Move();
    }

    protected abstract GameCell GetNextMove();

    private void Move()
    {
        GameCell nextCell = GetNextMove();
        MakeMove(nextCell);
    }

    void MakeMove(GameCell targetCell)
    {
        iTween.MoveTo(this.gameObject, iTween.Hash(
            "position", positionConverter.ConvertBoardPositionToScene(targetCell.GetCoordinates(), true),
            "oncomplete", "Move",
            "time", 3,
            "easetype", "linear"));
    }
}
