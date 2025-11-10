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

        [Header("Main Managers")]
        [SerializeField] private PlayerSelectionManager _selectionManager;
        [SerializeField] private PiecePlacementManager _placementManager;
        [SerializeField] private GameplayManager _gameplayManager;
        [SerializeField] private BoardManager _boardManager;

        [Header("Data")]
        [SerializeField] private SelectionStateData _selectionData;
        [SerializeField] private PlacementStateData _placementData;
        [SerializeField] private Colors _defaultColors;
        [SerializeField] private Colors _gameplayColors;

        [Header("Events")]
        [SerializeField] private GameEvent _selectionToPlacementTransitionEvent;
        [SerializeField] private GameEvent _placementToSelectionTransitionEvent;
        [SerializeField] private GameEvent _placementToGameplayTransitionEvent;
        [SerializeField] private GameEvent _gameplayToPlacementTransitionEvent;

        private void Awake()
        {
            HideEverything();
            ResetToDefaults();

            Debug.Log($"<color=yellow>EVERYTHING HAS BEEN RESET.</color>");

            switch (_currentPhase)
            {
                case GamePhase.Selection: InitializeSelection(); break;
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

            ObserveTransitionEvents();
        }

        private void OnDisable()
        {
            switch (_currentPhase)
            {
                case GamePhase.Selection: DisableSelection(); break;
                case GamePhase.Placement: DisablePlacement(); break;
                case GamePhase.Gameplay: DisableGameplay(); break;
            }

            IgnoreTransitionEvents();
        }

        private void HideEverything()
        {
            // hide selection manager
            _selectionManager.Hide();

            // hide placement manager
            _placementManager.Hide();

            // hide gameplay manager
            _gameplayManager.Hide();

            // hide board manager
            _boardManager.Hide();
        }

        private void ResetToDefaults()
        {
            _selectionData.ResetAllDataToDefaults();
            _gameplayColors.List = _defaultColors.List;
            _placementData.ClearPlacementData();
        }

        #region selection methods

        private void InitializeSelection()
        {
            _selectionManager.Initialize();
        }

        private void EnableSelection()
        {
            _selectionManager.Enable();
            _selectionManager.Show();
        }

        private void DisableSelection()
        {
            _selectionManager.Hide();
            _selectionManager.Disable();
        }

        #endregion

        #region placement methods

        private void EnablePlacement()
        {
            _placementManager.Enable();
            _placementManager.Show();
        }

        private void DisablePlacement()
        {
            _placementManager.Hide();
            _placementManager.Disable();
        }

        #endregion

        #region gameplay methods

        private void EnableGameplay()
        {
            _gameplayManager.Enable();
            _gameplayManager.Show();
        }

        private void DisableGameplay()
        {
            _gameplayManager.Disable();
            _gameplayManager.Hide();
        }

        #endregion

        #region transition methods

        private void ObserveTransitionEvents()
        {
            _selectionToPlacementTransitionEvent.AddEvent(OnTransitionFromSelectionToPlacement);
            _placementToSelectionTransitionEvent.AddEvent(OnTransitionFromPlacementToSelection);
            _placementToGameplayTransitionEvent.AddEvent(OnTransitionFromPlacementToGameplay);
            _gameplayToPlacementTransitionEvent.AddEvent(OnTransitionFromGameplayToPlacement);
        }

        private void IgnoreTransitionEvents()
        {
            _selectionToPlacementTransitionEvent.RemoveEvent(OnTransitionFromSelectionToPlacement);
            _placementToSelectionTransitionEvent.RemoveEvent(OnTransitionFromPlacementToSelection);
            _placementToGameplayTransitionEvent.RemoveEvent(OnTransitionFromPlacementToGameplay);
            _gameplayToPlacementTransitionEvent.RemoveEvent(OnTransitionFromGameplayToPlacement);
        }

        private void OnTransitionFromSelectionToPlacement()
        {
            Debug.Log($"<color=cyan>TRANSITIONING FROM SELECTION TO PLACEMENT.</color>");
            _currentPhase = GamePhase.Placement;

            _placementData.MarkPlacementDataAsUnusable(false);

            _boardManager.InitializeBoard(); // board must be initialized at least once. method also automatically checks if the board has already been initialized.
            _boardManager.Show();

            DisableSelection();
            EnablePlacement();
        }

        private void OnTransitionFromPlacementToSelection()
        {
            Debug.Log($"<color=cyan>TRANSITIONING FROM PLACEMENT TO SELECTION.</color>");

            _currentPhase = GamePhase.Selection;

            _placementData.MarkPlacementDataAsUnusable(false);

            _boardManager.Hide();

            DisablePlacement();
            EnableSelection();
        }

        private void OnTransitionFromPlacementToGameplay()
        {
            Debug.Log($"<color=cyan>TRANSITIONING FROM PLACEMENT TO GAMEPLAY.</color>");

            _currentPhase = GamePhase.Gameplay;

            _placementData.MarkPlacementDataAsUnusable(true);

            DisablePlacement();
            EnableGameplay();
        }

        private void OnTransitionFromGameplayToPlacement()
        {
            Debug.Log($"<color=cyan>TRANSITIONING FROM GAMEPLAY TO PLACEMENT.</color>");

            _currentPhase = GamePhase.Placement;

            _placementData.MarkPlacementDataAsUnusable(true);

            DisableGameplay();
            EnablePlacement();
        }

        #endregion

    }
}