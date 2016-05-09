using UnityEngine;
using Assets.Scripts.Model;
using Assets.Scripts.Util;
using Assets.Scripts.Board;

namespace Assets.Scripts.Postion
{
    public class PostionListener
    {
        private AbstractPlayer player;

        private Board.Board board;

        private PositionConverter positonConverter;

        private GameCell currentCell;

        public PostionListener(AbstractPlayer player, Board.Board board, PositionConverter positonConverter)
        {
            this.player = player;
            this.board = board;
            this.positonConverter = positonConverter;
        }

        public void OnPostionChanged(Vector3 newPostion)
        {
            Vector2 boardPostion = positonConverter.ConvertScenePositionToBoard(newPostion);
            GameCell gameCell = board.GetGameCell(boardPostion);
            if (gameCell.Equals(currentCell))
            {
                return;
            }
            if (currentCell != null)
            {
                currentCell.RemovePlayer(player);
            }
            currentCell = gameCell;
            gameCell.AddPlayer(player);
        }
    }
}
