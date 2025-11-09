using UnityEngine;

namespace Game
{
    public class PiecePlacementManager : MonoBehaviour
    {
        [Header("INITIALIZATION STATUS")]
        [SerializeField][ReadOnly] private bool _initialized = false;

        [Header("Components")]
        [SerializeField] private PlacementPiecesInterface[] _interfaces;
        [SerializeField] private GameManager _gameManager;

        [Header("Game Objects")]
        [SerializeField] private GameObject _mainDisplayObject;

        [Header("Data")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private PlacementStateData _placementState;

        [Header("Events")]
        [SerializeField] private GameEvent _clickCellEvent;
        [SerializeField] private GameEvent _placementToSelectionTransitionEvent;
        [SerializeField] private GameEvent _placementToGameplayTransitionEvent;

        [Header("TESTING")]
        [SerializeField][Range(0, 5)] private int _testIndex;

        public void Initialize()
        {
            if (_initialized)
            {
                Debug.Log($"<color=magenta>Placement Manager already initialized</color>");
                return;
            }

            Debug.Log("<color=lime>Placement Manager Initialized</color>");
            _initialized = true;

            _placementState.GenerateNewPlacementData();

            // need to see what are the pieces each player has selected and generate data for placement state data.


        }

        public void Enable()
        {
            Debug.Log("<color=lime>Placement Manager Enabled</color>");
            _clickCellEvent.AddEvent(OnClickCell);

            _placementToSelectionTransitionEvent.AddEvent(OnTransitionToSelection);
            _placementToGameplayTransitionEvent.AddEvent(OnTransitionToGameplay);

            // reset placement state data 
            _placementState.BeginPlacement();

            // enable the interface of that player (set all buttons as interactable, use the active color for text and background, DO NOT HIGHLIGHT ANY BUTTONS)
            int numberOfPlayers = _gameData.NumberOfPlayers;

            // show interfaces of players up to number of players and hide the rest.
            for (int i = 0; i < _interfaces.Length; i++)
            {
                if (i < numberOfPlayers) _interfaces[i].Show();
                else _interfaces[i].Hide();
            }

            // get the ID of the default player that gets to choose a piece first
            int defaultPlayerID = _placementState.PlayerId;

            _interfaces[defaultPlayerID].UseActiveColor();

            //PlacementData placement = _placementState
        }

        public void Disable()
        {
            Debug.Log("<color=lime>Placement Manager Disabled</color>");
            _clickCellEvent.RemoveEvent(OnClickCell);

            _placementToGameplayTransitionEvent.RemoveEvent(OnTransitionToSelection);
            _placementToGameplayTransitionEvent.RemoveEvent(OnTransitionToGameplay);
        }

        public void Show()
        {
            _mainDisplayObject.SetActive(true);
        }

        public void Hide()
        {
            if (_mainDisplayObject == null) return; //sometimes this gets destroyed when exiting playmode.
            _mainDisplayObject.SetActive(false);
        }

        #region Unity Event Button Methods;

        public void OnPressPlay_UI_BUTTON()
        {

        }

        public void OnPressReset_UI_BUTTON()
        {

        }

        public void OnPressConfirm_UI_BUTTON()
        {

        }

        public void OnPressRandomize_UI_BUTTON()
        {

        }

        public void OnPressReturn_UI_BUTTON()
        {
            Debug.Log("pressed return.");
            _placementToSelectionTransitionEvent.Raise();
        }

        public void OnPressQuit_UI_BUTTON()
        {

        }

        public void OnPressPieceButton_UI_Button(int slotID)
        {

        }

        #endregion

        private void OnClickCell()
        {

        }

        private void OnTransitionToSelection()
        {

        }

        private void OnTransitionToGameplay()
        {

        }
    }
}