using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Event", menuName = "SO/Events/Game Event")]
public class GameEvent : ScriptableObject
{
    private event Action _eventAction;

    public void Add(Action e) => _eventAction += e;

    public void Remove(Action e) => _eventAction -= e;

    public void Raise() => _eventAction?.Invoke();
}