using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct Coordinate : IEquatable<Coordinate>
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

        public bool Equals(Coordinate other) => this == other;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Coordinate Zero => new Coordinate(0, 0);

        public static implicit operator Vector2(Coordinate c)
        {
            return new Vector2(c.X, c.Y);
        }

        public static bool operator ==(Coordinate a, Coordinate b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Coordinate a, Coordinate b) => !(a == b);
    }
}