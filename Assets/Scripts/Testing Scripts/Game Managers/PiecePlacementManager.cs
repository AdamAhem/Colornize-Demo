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
        [SerializeField] private SelectionStateData _selectionStateData;
        [SerializeField] private PlacementStateData _placementStateState;

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

            // this initialize method may not be necessary at all.
        }

        public void Enable()
        {
            Debug.Log("<color=lime>Placement Manager Enabled</color>");
            _clickCellEvent.AddEvent(OnClickCell);

            // here, there's 2 places where users could have entered from.
            // 1) from selection phase
            // 2) from gameplay phase

            // if coming from the selection phase, then NONE of the pieces are placed yet
            // if coming from gameplay phase, ALL pieces are already placed.

            // no need to worry about board generation, that's already done by main manager.

            // when coming from selection, do the following:

            //  PLACEMENT DATA CONFIGURATION

            //  0) READ FROM SELECTION STATE DATA.

            //  1) INITIALIZE THE PLACEMENT DATA JAGGED ARRAY.
            //      first index: NUMBER OF PLAYERS
            //      2nd index: PIECES PER PLAYER
            _placementStateState.InitializePlacementData(_selectionStateData);


            //  PLACEMENT VISUALISATION

            //  1) if selection state data.numofplayers = N, then show the first N interfaces and hide the remaining.





            // reset placement state data 
            //_placementState.BeginPlacement();

            // enable the interface of that player (set all buttons as interactable, use the active color for text and background, DO NOT HIGHLIGHT ANY BUTTONS)
            //int numberOfPlayers = _gameData.NumberOfPlayers;

            // show interfaces of players up to number of players and hide the rest.
            //for (int i = 0; i < _interfaces.Length; i++)
            //{
            //    if (i < numberOfPlayers) _interfaces[i].Show();
            //    else _interfaces[i].Hide();
            //}

            // get the ID of the default player that gets to choose a piece first
            //int defaultPlayerID = _placementState.PlayerId;

            //_interfaces[defaultPlayerID].UseActiveColor();

            //PlacementData placement = _placementState
        }

        public void Disable()
        {
            Debug.Log("<color=lime>Placement Manager Disabled</color>");
            _clickCellEvent.RemoveEvent(OnClickCell);
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
            Debug.Log("pressed play.");
        }

        public void OnPressReset_UI_BUTTON()
        {
            Debug.Log("pressed reset.");
        }

        public void OnPressConfirm_UI_BUTTON()
        {
            Debug.Log("pressed confirm.");
        }

        public void OnPressRandomize_UI_BUTTON()
        {
            Debug.Log("pressed randomize.");
        }

        public void OnPressReturn_UI_BUTTON()
        {
            Debug.Log("pressed return.");
            _placementToSelectionTransitionEvent.Raise();
        }

        public void OnPressQuit_UI_BUTTON()
        {
            Debug.Log("<color=red>PRESSED QUIT BUTTON - WORK IN PROGRESS</color>");
        }

        public void OnPressPieceButton_UI_Button(int slotID)
        {
            Debug.Log($"clicked piece button from player: (current player) -> slot {slotID} ");
        }

        #endregion

        private void OnClickCell()
        {
            Debug.Log("clicked cell: (read placement state data most recently clicked cell)");
        }
    }
}