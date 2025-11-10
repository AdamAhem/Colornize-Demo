using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Game Event", menuName = "SO/Events/Game Event/Coordinate")]
    public class CoordinateEvent : ScriptableObject
    {
        [TextArea(1, 10)]
        public string Description;

        private event Action<Coordinate> _eventAction;

        public void AddEvent(Action<Coordinate> e) => _eventAction += e;

        public void RemoveEvent(Action<Coordinate> e) => _eventAction -= e;

        public void Raise(Coordinate c) => _eventAction?.Invoke(c);
    }
}