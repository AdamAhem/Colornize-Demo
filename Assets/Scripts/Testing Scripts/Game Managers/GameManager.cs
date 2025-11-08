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
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private SelectionStateData _selectionData;
        [SerializeField] private Colors _defaultColors;
        [SerializeField] private Colors _gameplayColors;

        [Header("Events")]
        [SerializeField] private GameEvent _selectionToPlacementTransitionEvent;
        [SerializeField] private GameEvent _placementToSelectionTransitionEvent;
        [SerializeField] private GameEvent _placementToGameplayTransitionEvent;
        [SerializeField] private GameEvent _gameplayToPlacementTransitionEvent;

        //public void OnPressConfirmToPlay()
        //{
        //    _currentPhase = GamePhase.Gameplay;

        //    DisablePlacement();

        //    InitializeGameplay();
        //    EnableGameplay();
        //}

        private void Awake()
        {
            HideEverything();
            ResetToDefaults();

            Debug.Log($"<color=yellow>EVERYTHING HAS BEEN RESET.</color>");

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
            _gameData.ResetData();
            _selectionData.ResetData();
            _gameplayColors.List = _defaultColors.List;
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

        private void ResetSelection()
        {
            _gameData.ResetData();
        }

        #endregion

        #region placement methods

        private void InitializePlacement()
        {
            _boardManager.InitializeBoard();
            _placementManager.Initialize();
        }

        private void EnablePlacement()
        {
            _boardManager.Show();

            _placementManager.Enable();
            _placementManager.Show();
        }

        private void DisablePlacement()
        {
            _boardManager.Hide();

            _placementManager.Hide();
            _placementManager.Disable();
        }

        private void ResetPlacement()
        {

        }

        #endregion

        #region gameplay methods

        private void InitializeGameplay()
        {
            _selectionManager.gameObject.SetActive(false);
            //_placementMain.SetActive(false);
            _gameplayManager.Initialize();
        }

        private void EnableGameplay()
        {
            _gameplayManager.Enable();
            //_scoreboardMain.SetActive(true);
        }

        private void DisableGameplay()
        {
            _gameplayManager.Disable();
        }

        private void ResetGameplay()
        {

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

            DisableSelection();

            InitializePlacement();
            EnablePlacement();
        }

        private void OnTransitionFromPlacementToSelection()
        {
            Debug.Log($"pla -> sel");
        }

        private void OnTransitionFromPlacementToGameplay()
        {
            Debug.Log("pla -> gam");
        }

        private void OnTransitionFromGameplayToPlacement()
        {
            Debug.Log("gam -> pla");
        }

        #endregion

    }
}