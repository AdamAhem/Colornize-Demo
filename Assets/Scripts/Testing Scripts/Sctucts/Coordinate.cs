using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct Coordinate
    {
        public int X;
        public int Y;

        public Coordinate(int x, int y)
        {
            X = x; 
            Y = y;
        }

        public static Coordinate Convert(Vector2 v)
        {
            return new Coordinate((int)v.x, (int)v.y);
        }

        /// <summary>
        /// <paramref name="min"/> inclusive, <paramref name="max"/> exclusive.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Coordinate GetRandomWithinBBOX(Coordinate min, Coordinate max)
        {
            int x = UnityEngine.Random.Range(min.X, max.X);
            int y = UnityEngine.Random.Range(min.Y, max.Y);
            return new Coordinate(x, y);
        }

        public static Coordinate Zero => new Coordinate(0, 0);
    }
}