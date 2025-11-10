using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Game Event", menuName = "SO/Events/Game Event/Normal")]
    public class GameEvent : ScriptableObject
    {
        [TextArea(1, 10)]
        public string Description;

        private event Action _eventAction;

        public void AddEvent(Action e) => _eventAction += e;

        public void RemoveEvent(Action e) => _eventAction -= e;

        public void Raise() => _eventAction?.Invoke();
    }
}