using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Siege Move", menuName = "SO/Piece Moves/Siege")]
    public class SiegeMove : PieceMove
    {
        public override List<Coordinate> Get(int x, int y, Coordinate size)
        {
            // go as far as possible in the horizontal and vertical direction.
            int xMax = size.X - 1;
            int yMax = size.Y - 1;

            List<Coordinate> allMoves = new List<Coordinate>(xMax + yMax);

            for (int xi = 0; xi <= xMax; xi++)
            {
                if (xi == x) continue;
                allMoves.Add(new Coordinate(xi, y));
            }

            for (int yi = 0; yi <= yMax; yi++)
            {
                if (yi == y) continue;
                allMoves.Add(new Coordinate(x, yi));
            }

            return allMoves;
        }
    }
}