using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Knight Move", menuName = "SO/Piece Moves/Knight")]
    public class KnightMove : PieceMove
    {
        private readonly int[][] Moves = new int[8][]
{
            new int[2] {-1, 2 },
            new int[2] {1, 2 },
            new int[2] {2, 1 },
            new int[2] {2, -1 },
            new int[2] {-1, -2 },
            new int[2] {1, -2 },
            new int[2] {-2, 1 },
            new int[2] {-2, -1 }
};

        public override List<Vector2> Get(int x, int y, Vector2 size)
        {
            List<Vector2> possibleMoves = new List<Vector2>(Moves.Length);

            for (int i = 0; i < Moves.Length; i++)
            {
                int[] move = Moves[i];

                int xPos = x + move[0];
                int yPos = y + move[1];

                if (IsValid(xPos, yPos, size)) possibleMoves.Add(new Vector2(xPos, yPos));
            }

            return possibleMoves;
        }
    }
}