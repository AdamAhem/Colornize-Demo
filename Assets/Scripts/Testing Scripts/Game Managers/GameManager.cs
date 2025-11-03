using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        // this script will be the ONLY script that uses Awake/OnEnable/OnDisable/Start/OnDestroy.
        // all other manager scripts will be called through this script.

        private enum GamePhase { Selection, Placement, Gameplay }

        [Header("Starting phase  on playmode")]
        [SerializeField] private GamePhase _startPhase;

        [Header("Managers")]
        [SerializeField] private PlayerSelectionManager _playerSelectionManager;
        [SerializeField] private float _selectionPageFadeDuration;
        [SerializeField] private PiecePlacementManager _placementManager;
        [SerializeField] private BoardBuilder _boardBuilder;

        [Header("Scriptable Objects")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private SelectionStateData _selectionData;
        [SerializeField] private Colors _defaultColors;
        [SerializeField] private Colors _gameplayColors;

        public async void OnPressPlay_UI_BUTTON()
        {
            // disable the player selection manager and initialize (for first time playing) + enable the GAMEPLAY manager (first phase - piece placement)
            _playerSelectionManager.Disable();
            _playerSelectionManager.FadeOut(_selectionPageFadeDuration);

            await AsyncHelpers.Wait(_selectionPageFadeDuration);

            _placementManager.Initialize();
            _placementManager.Enable();
        }

        private void Awake()
        {
            _selectionData.ResetData();
            _gameplayColors.List = _defaultColors.List;

            switch (_startPhase)
            {
                case GamePhase.Selection: InitializeSelection(); break;
                case GamePhase.Placement: InitializePlacement(); break;
                case GamePhase.Gameplay: break;
            }
        }

        private void OnEnable()
        {
            switch (_startPhase)
            {
                case GamePhase.Selection: EnableSelection(); break;
                case GamePhase.Placement: EnablePlacement(); break;
                case GamePhase.Gameplay: break;
            }
        }

        private void OnDisable()
        {
            switch (_startPhase)
            {
                case GamePhase.Selection: DisableSelection(); break;
                case GamePhase.Placement: DisablePlacement(); break;
                case GamePhase.Gameplay: break;
            }
        }

        private void OnDestroy()
        {
            _selectionData.ResetData();

            switch (_startPhase)
            {
                case GamePhase.Selection: ResetSelection(); break;
                case GamePhase.Placement: ResetPlacement(); break;
                case GamePhase.Gameplay: break;
            }
        }

        #region selection methods

        private void InitializeSelection()
        {
            _gameData.ResetData();
            _playerSelectionManager.Initialize();
        }

        private void EnableSelection()
        {
            _playerSelectionManager.Enable();
        }

        private void DisableSelection()
        {
            _playerSelectionManager.Disable();
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

            // display the BOARD and the PLACEMENT PANEL

            // how will the placement panel work?

            // start with player 0. they will select one of their pieces and choose a spot on the board to place it.

            // player 0 can then either confirm (by clicking the confirm button) or undo (by left-clicking their placed piece).

            // only when the player clicks confirm, will the next player get to choose where to place their piece.

            // players can choose ANY of their 3 pieces to place first, but will only place them 1 at a time.




        }

        private void EnablePlacement()
        {
            _placementManager.Enable();

            // enable the grid.
            _boardBuilder.BuildBoard();
        }

        private void DisablePlacement()
        {
            _placementManager.Disable();
        }

        private void ResetPlacement()
        {

        }

        #endregion
    }
}