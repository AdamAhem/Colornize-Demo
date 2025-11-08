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
    }
}