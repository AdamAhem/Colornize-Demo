using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        // this script will be the ONLY script that uses Awake/OnEnable/OnDisable/Start/OnDestroy.
        // all other manager scripts will be called through this script.

        [Header("Managers")]
        [SerializeField] private PlayerSelectionManager _playerSelectionManager;

        [Header("Scriptable Objects")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private SelectionStateData _selectionData;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            Enable();
        }

        private void OnDisable()
        {
            Disable();
        }

        private void OnDestroy()
        {
            Reset();
        }

        private void Initialize()
        {
            _gameData.ResetData();
            _selectionData.ResetData();

            _playerSelectionManager.Initialize();
        }

        private void Enable()
        {
            _playerSelectionManager.Enable();
        }

        private void Disable()
        {
            _playerSelectionManager.Disable();
        }

        private void Reset()
        {
            _gameData.ResetData();
            _selectionData.ResetData();
        }
    }
}