using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameInstanceData", menuName = "SO/GameInstanceData")]
    public class GameInstanceData : ScriptableObject
    {
        [Header("FIXED CONSTANTS")]
        [SerializeField] private int _piecesPerPlayer;

        [Header("Instance Data Visualiser")]
        [SerializeField][ReadOnly] private int _numberOfPlayers;

        [Header("<color=#ff0000> --- READONLY --- DO NOT ADD OR REMOVE ELEMENTS MANUALLY FROM THE EDITOR</color>")]
        [SerializeField] private List<PreGamePlayerConfig> _playerConfig;

        public void ResetData()
        {
            _numberOfPlayers = 0;
            _playerConfig = null;
        }

        public void GenerateNewPlayerConfigs(int playerCount)
        {
            _playerConfig = new List<PreGamePlayerConfig>(playerCount);
        }

        public void UpdatePlayerConfig(int playerID, int slotID, int pieceID)
        {
            var config = _playerConfig[playerID];
            config.SetPieceID(slotID, pieceID);
        }

        public int NumberOfPlayers => _numberOfPlayers;

        public void SetNumberOfPlayers(int number)
        {
            _numberOfPlayers = number;

            int configCount = _playerConfig.Count;

            if (_numberOfPlayers < configCount)
            {
                for (int i = configCount; i > _numberOfPlayers; i--)
                {
                    int j = i - 1;
                    Debug.Log($"removing: {j}");
                    _playerConfig.RemoveAt(j);
                }
            }
            else if (_numberOfPlayers > configCount)
            {
                for (int i = 0; i < _numberOfPlayers - configCount; i++)
                {
                    int j = i + configCount;
                    Debug.Log($"adding: {j}");
                    _playerConfig.Add(new PreGamePlayerConfig(j, _piecesPerPlayer));
                }
            }
        }
    }
}
