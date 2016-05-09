using UnityEngine;
using Assets.Scripts.Model;
using System.Collections.Generic;

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
        
        public List<GameCell> GetFreeGameCells() 
        {
            List<GameCell> freeCells = new List<GameCell>();
            for (int i = 0; i < size.y; i++) 
            {
                for (int j = 0; j < size.x; j++) 
                {
                    GameCell gameCell = GetGameCell(j, i);
                    if (gameCell.IsEmpty()) {
                        freeCells.Add(gameCell);
                    }    
                }
            }
            return freeCells;
        }
        
        public List<GameCell> GetFreeCellsAtMinDistance(Vector2 currentPos, int minDistance) 
        {
            List<GameCell> freeCells = GetFreeGameCells();
            List<GameCell> minDistanceCells = new List<GameCell>();
            foreach (GameCell gameCell in freeCells) {
                if (gameCell.ManhattanDistanceTo(currentPos) >= minDistance) {
                    minDistanceCells.Add(gameCell);
                }
            }
            return minDistanceCells;
        }

        public GameCell GetGameCell(Vector2 position)
        {
            return GetGameCell((int)position.x, (int)position.y);
        }

        public GameCell GetGameCell(int x, int y)
        {
            if (x >= 0 && x < size.x && y >= 0 && y < size.y)
            {
                return cells[x, y];
            }
            else
            {
                return null;
            }
        }
        
        public List<GameCell> GetAdjacentCells(Vector2 position) 
        {
            return GetAdjacentCells((int)position.x, (int)position.y);    
        }
        
        public List<GameCell> GetAdjacentCells(int x, int y) 
        {
            List<GameCell> adjacentCells = new List<GameCell>();
            adjacentCells.Add(GetGameCell(x, y + 1));
            adjacentCells.Add(GetGameCell(x, y - 1));
            adjacentCells.Add(GetGameCell(x + 1, y));
            adjacentCells.Add(GetGameCell(x - 1, y));
            
            return adjacentCells.FindAll(gameCell => gameCell != null && gameCell.IsEmpty());
        }
    }
}
