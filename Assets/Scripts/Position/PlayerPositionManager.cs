using Assets.Scripts.Model;
using Assets.Scripts.Util;
using System.Collections.Generic;

namespace Assets.Scripts.Position
{
    public class PlayerPositionManager
    {
        private Dictionary<AbstractPlayer, BoardPostionListener> players;

        private Board.Board board;

        private PositionConverter positonConverter;

        public PlayerPositionManager(Board.Board board, PositionConverter positonConverter)
        {
            this.players = new Dictionary<AbstractPlayer, BoardPostionListener>();
            this.board = board;
            this.positonConverter = positonConverter;
        }

        public void AddPlayer(AbstractPlayer player)
        {
            BoardPostionListener listener = new BoardPostionListener(player, board, positonConverter);
            player.postionLisener = listener;
            players.Add(player, listener);
        }


        public bool RemovePlayer(AbstractPlayer player)
        {
            player.postionLisener = null;
            return players.Remove(player);
        }

    }
}
