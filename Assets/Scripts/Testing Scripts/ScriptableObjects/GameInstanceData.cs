using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameInstanceData", menuName = "SO/GameInstanceData")]
    public class GameInstanceData : ScriptableObject
    {
        [SerializeField][ReadOnly] private int _numberOfPlayers;

        public void ResetData()
        {
            _numberOfPlayers = 0;
        }

        public int NumberOfPlayers => _numberOfPlayers;

        public void SetNumberOfPlayers(int number) => _numberOfPlayers = number;
    }
}
