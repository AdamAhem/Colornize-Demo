using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Captain Move", menuName = "SO/Piece Moves/Captain")]
    public class CaptainMove : PieceMove
    {
        private readonly int _rows = 5;
        private readonly int _columns = 5;

        private readonly int _dRow = -2;
        private readonly int _dCol = -2;

        private readonly int _maxSumOfSquares = 8;

        public override List<Coordinate> Get(int x, int y, Coordinate size)
        {
            List<Coordinate> allMoves = new List<Coordinate>(Defaults.MAX_POSSIBLE_MOVES);

            int dx;
            int dy;

            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    dx = i + _dRow;
                    dy = j + _dCol;

                    int xPos = x + dx;
                    int yPos = y + dy;

                    if ((dx * dx) + (dy * dy) == _maxSumOfSquares || !IsValid(xPos, yPos, size)) continue;

                    allMoves.Add(new Coordinate(xPos, yPos));
                }
            }

            return allMoves;
        }
    }
}