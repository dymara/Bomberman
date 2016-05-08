using Assets.Scripts.Model;
using Assets.Scripts.Util;
using System.Collections.Generic;

namespace Assets.Scripts.Postion
{
    public class PlayerPositionManager
    {

        private Dictionary<AbstractPlayer, PostionListener> players;

        private Board.Board board;

        private PositionConverter positonConverter;

        public PlayerPositionManager(Board.Board board, PositionConverter positonConverter)
        {
            players = new Dictionary<AbstractPlayer, PostionListener>();
            this.board = board;
            this.positonConverter = positonConverter;
        }

        public void AddPlayer(AbstractPlayer player)
        {
            PostionListener listener = new PostionListener(player, board, positonConverter);
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
