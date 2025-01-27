﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Snake
{
    public class GameState
    {
        public int Rows { get; }//game pad which allows moving by the sanke
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }// game score
        public bool GameOver { get; private set; }// true/fasle for gameover

        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();
        public readonly LinkedList<Position> snakePositions = new LinkedList<Position>();  // this area is occuiped by the snake   
        private readonly Random random = new Random();// it appears the food in randomly. 

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows,cols];
            Dir = Direction.Right;//when the game start, the snake direction is the right
           
            AddSnake();
            AddFood(); 
        }
        // Adds initial snake to the grid.
        private void AddSnake()
        {
            int r = Rows / 2;// even numbers closed to the top. 

            for (int c = 1; c < 3; c++)
            {
                Grid[r, c] = GridValue.Snake;//good add snake 
                snakePositions.AddFirst(new Position(r, c));// Adds position to the snake's body.

            }
        }
        // Returns all empty positions in the grid.
        private IEnumerable<Position> EmptyPositions()
            {
                for (int r = 0; r < Rows; r++)
                {
                    for (int c = 0; c < Cols; c++)
                    {
                        if (Grid[r, c] == GridValue.Empty)
                        {
                            yield return new Position(r, c);

                        }
                    }
                }
            }
         private void AddFood()
            {
                List<Position> emtpy = new List<Position>(EmptyPositions());

                if(emtpy.Count == 0)
                {
                    return;//aviod the game crash but return 
                }
                Position pos = emtpy[random.Next(emtpy.Count)];
                Grid[pos.Row, pos.Col] = GridValue.Food;
            }
                    
        public Position HeadPosition()
        {
            return snakePositions.First.Value;
        }

        public Position TailPosition()
        {
            return snakePositions.Last.Value;
        }

        public IEnumerable<Position> SnakePositions()
        {
            return snakePositions;
        }

        private void AddHead(Position pos)
        {
            snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            Position tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            snakePositions.RemoveLast();//remove the linkedlists. 
        
        }

        private Direction GetLastDirection()
        {
            if(dirChanges.Count == 0)
            {
                return Dir;
            }
            return dirChanges.Last.Value;
        }

        private bool CanChangeDirection(Direction newDir)
        {
            if(dirChanges.Count == 2)
            {
                return false;
            }

            Direction lastDir = GetLastDirection();
            return newDir != lastDir && newDir != lastDir.Opposite();
        }
        public void ChangeDirection(Direction dir)
        {
           //if can change direction 
           if(CanChangeDirection(dir))
            {
                dirChanges.AddLast(dir);
            }
        }

        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;
        }
        private GridValue WillHit(Position newHeadPos)
        {
            //hit outside grid , return call outside. 
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            if(newHeadPos == TailPosition())
            {
                return GridValue.Empty;

            }
            return Grid[newHeadPos.Row, newHeadPos.Col];
        }
        public void Move()
        {
            if (dirChanges.Count > 0)
            {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }
            Position newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);

            if(hit == GridValue.Outside || hit == GridValue.Snake)
            {
                GameOver = true;
            }
            else if(hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            else if(hit == GridValue.Food)
            {
                AddHead(newHeadPos);
                Score++;
                AddFood();
            }
        }
    }
}
