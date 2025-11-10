using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Spy Move", menuName = "SO/Piece Moves/Spy")]
    public class SpyMove : PieceMove
    {
        private readonly int[][] Moves = new int[8][]
        {
            new int[2] {0, 2 },
            new int[2] {2, 2 },
            new int[2] {2, 0 },
            new int[2] {2, -2 },
            new int[2] {0, -2 },
            new int[2] {-2, -2 },
            new int[2] {-2, 0},
            new int[2] {-2, 2 }
        };

        public override List<Coordinate> Get(int x, int y, Coordinate size)
        {
            List<Coordinate> possibleMoves = new List<Coordinate>(Moves.Length);

            for (int i = 0; i < Moves.Length; i++)
            {
                int[] move = Moves[i];

                int xPos = x + move[0];
                int yPos = y + move[1];

                if (IsValid(xPos, yPos, size)) possibleMoves.Add(new Coordinate(xPos, yPos));
            }

            return possibleMoves;
        }
    }
}