using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Siege Move", menuName = "SO/Piece Moves/Siege")]
    public class SiegeMove : PieceMove
    {
        public override List<Vector2> Get(int x, int y, Vector2 size)
        {
            // go as far as possible in the horizontal and vertical direction.
            int xMax = (int)size.x - 1;
            int yMax = (int)size.y - 1;

            List<Vector2> allMoves = new List<Vector2>(xMax + yMax);

            for (int xi = 0; xi <= xMax; xi++)
            {
                if (xi == x) continue;
                allMoves.Add(new Vector2(xi, y));
            }

            for (int yi = 0; yi <= yMax; yi++)
            {
                if (yi == y) continue;
                allMoves.Add(new Vector2(x, yi));
            }

            return allMoves;
        }
    }
}