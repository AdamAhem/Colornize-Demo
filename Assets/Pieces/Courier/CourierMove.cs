using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Courier Move", menuName = "SO/Piece Moves/Courier")]
    public class CourierMove : PieceMove
    {
        public override List<Coordinate> Get(int x, int y, Coordinate size)
        {
            List<Coordinate> allMoves = new(Defaults.MAX_POSSIBLE_MOVES);

            int xMax = size.X - 1;
            int yMax = size.Y - 1;

            // top right
            int dx = 1;
            int dy = 1;
            while (x + dx <= xMax && y + dy <= yMax)
            {
                allMoves.Add(new Coordinate(x + dx, y + dy));
                dx++;
                dy++;
            }

            // top left
            dx = 1;
            dy = 1;
            while (x - dx >= 0 && y + dy <= yMax)
            {
                allMoves.Add(new Coordinate(x - dx, y + dy));
                dx++;
                dy++;
            }

            // btm left
            dx = 1;
            dy = 1;
            while (x - dx >= 0 && y - dy >= 0)
            {
                allMoves.Add(new Coordinate(x - dx, y - dy));
                dx++;
                dy++;
            }

            // btm right
            dx = 1;
            dy = 1;
            while (x + dx <= xMax && y - dy >= 0)
            {
                allMoves.Add(new Coordinate(x + dx, y - dy));
                dx++;
                dy++;
            }

            return allMoves;
        }
    }
}