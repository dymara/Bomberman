using UnityEngine;
using Assets.Scripts.Model;
using Assets.Scripts.Util;
using Assets.Scripts.Board;
using Assets.Scripts.Position;

namespace Assets.Scripts.Position
{
    public class BoardPostionListener : IPositionListener
    {
        private AbstractPlayer player;

        private Board.Board board;

        private PositionConverter positonConverter;

        private GameCell currentCell;

        public BoardPostionListener(AbstractPlayer player, Board.Board board, PositionConverter positonConverter)
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

        public void RemovePlayerFromCurrentCell()
        {
            currentCell.RemovePlayer(player);
        }
    }
}
