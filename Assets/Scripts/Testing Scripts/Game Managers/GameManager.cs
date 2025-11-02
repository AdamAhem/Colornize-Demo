using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private PlayerSelectionManager _playerSelectionManager;

        [Header("Scriptable Objects")]
        [SerializeField] private GameInstanceData _gameData;
        // this script will be the ONLY script that uses Awake/OnEnable/OnDisable/Start.
        // all other manager scripts will be called through this script.

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _gameData.ResetData();

            // initialize game config manager (the part where the number of players and their pieces are chosen)
            _playerSelectionManager.Initialize();

        }
    }
}