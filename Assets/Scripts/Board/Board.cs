using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Model;

namespace Assets.Scripts.Board
{
    public class Board
    {
        private GameCell[,] cells;
        private Vector2 size;
        private List<Player> players;
        private List<Monster> monsters;

        public Board(GameCell[,] cells, Vector2 size)
        {
            this.cells = cells;
            this.size = size;
            this.players = new List<Player>();
            this.monsters = new List<Monster>();
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        public List<Monster> GetMonsters()
        {
            return monsters;
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
        }

        public void AddMonster(Monster monster)
        {
            monsters.Add(monster);
        }

        public void RemoveMonster(Monster monster)
        {
            monsters.Remove(monster);
        }

        public Vector2 GetSize()
        {
            return size;
        }

        public GameCell[,] GetGameCells()
        {
            return cells;
        }

        public GameCell GetGameCell(int x, int z)
        {
            return cells[x, z];
        }
    }
}
