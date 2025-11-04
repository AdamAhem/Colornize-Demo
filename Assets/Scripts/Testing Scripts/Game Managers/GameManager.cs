using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        // this script will be the ONLY script that uses Awake/OnEnable/OnDisable/Start/OnDestroy.
        // all other manager scripts will be called through this script.

        private enum GamePhase { Selection, Placement, Gameplay }

        [Header("Starting phase  on playmode")]
        [SerializeField] private GamePhase _currentPhase;

        [Header("Managers")]
        [SerializeField] private PlayerSelectionManager _playerSelectionManager;
        [SerializeField] private float _selectionPageFadeDuration;
        [SerializeField] private PiecePlacementManager _placementManager;
        [SerializeField] private GameObject _placementMain;
        [SerializeField] private BoardBuilder _boardBuilder;
        [SerializeField] private GameplayManager _gameplayManager;
        [SerializeField] private GameObject _cellsBoardObject;
        [SerializeField] private GameObject _scoreboardMain;
        [SerializeField] private GameObject _selectionMainObject;

        [Header("Scriptable Objects")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private SelectionStateData _selectionData;
        [SerializeField] private Colors _defaultColors;
        [SerializeField] private Colors _gameplayColors;

        public async void OnPressPlay_UI_BUTTON()
        {
            // disable the player selection manager and initialize (for first time playing) + enable the GAMEPLAY manager (first phase - piece placement)
            DisableSelection();

            await AsyncHelpers.Wait(_selectionPageFadeDuration);

            _selectionMainObject.SetActive(false);

            _currentPhase = GamePhase.Placement;

            InitializePlacement();
            EnablePlacement();
        }

        public void OnPressConfirmToPlay()
        {
            _currentPhase = GamePhase.Gameplay;
            DisablePlacement();

            InitializeGameplay();
            EnableGameplay();
        }

        private void Awake()
        {
            _selectionData.ResetData();
            _gameplayColors.List = _defaultColors.List;

            switch (_currentPhase)
            {
                case GamePhase.Selection: InitializeSelection(); break;
                case GamePhase.Placement: InitializePlacement(); break;
                case GamePhase.Gameplay: InitializeGameplay(); break;
            }
        }

        private void OnEnable()
        {
            switch (_currentPhase)
            {
                case GamePhase.Selection: EnableSelection(); break;
                case GamePhase.Placement: EnablePlacement(); break;
                case GamePhase.Gameplay: EnableGameplay(); break;
            }
        }

        private void OnDisable()
        {
            switch (_currentPhase)
            {
                case GamePhase.Selection: DisableSelection(); break;
                case GamePhase.Placement: DisablePlacement(); break;
                case GamePhase.Gameplay: DisableGameplay(); break;
            }
        }

        private void OnDestroy()
        {
            _selectionData.ResetData();

            switch (_currentPhase)
            {
                case GamePhase.Selection: ResetSelection(); break;
                case GamePhase.Placement: ResetPlacement(); break;
                case GamePhase.Gameplay: ResetGameplay(); break;
            }
        }

        #region selection methods

        private void InitializeSelection()
        {
            _cellsBoardObject.SetActive(false);
            _scoreboardMain.SetActive(false);

            _gameData.ResetData();
            _playerSelectionManager.Initialize();
        }

        private void EnableSelection()
        {
            _selectionMainObject.SetActive(true);

            _playerSelectionManager.Enable();
        }

        private void DisableSelection()
        {
            _playerSelectionManager.Disable();
            _playerSelectionManager.FadeOut(_selectionPageFadeDuration);
        }

        private void ResetSelection()
        {
            _gameData.ResetData();
        }

        #endregion

        #region placement methods

        private void InitializePlacement()
        {
            _playerSelectionManager.FadeOut(0);
            _placementManager.Initialize();
        }

        private void EnablePlacement()
        {
            _placementManager.Enable();
            _boardBuilder.BuildBoard();

            _placementMain.SetActive(true);
            _cellsBoardObject.SetActive(true);
        }

        private void DisablePlacement()
        {
            _placementManager.Disable();
        }

        private void ResetPlacement()
        {

        }

        #endregion

        #region gameplay methods

        private void InitializeGameplay()
        {
            _playerSelectionManager.FadeOut(0);
            _placementMain.SetActive(false);
            _gameplayManager.Initialize();
        }

        private void EnableGameplay()
        {
            _gameplayManager.Enable();
            _scoreboardMain.SetActive(true);
        }

        private void DisableGameplay()
        {
            _gameplayManager.Disable();
        }

        private void ResetGameplay()
        {

        }

        #endregion
    }
}