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

        [Header("Events")]
        [SerializeField] private GameEvent _clickCellEvent;
        [SerializeField] private GameEvent _placementToSelectionTransitionEvent;
        [SerializeField] private GameEvent _placementToGameplayTransitionEvent;

        [Header("State managers")]
        [SerializeField] private PlacementStateData _placementState;

        public void Initialize()
        {
            if (_initialized)
            {
                Debug.Log($"<color=magenta>Placement Manager already initialized</color>");
                return;
            }

            Debug.Log("<color=lime>Placement Manager Initialized</color>");
            _initialized = true;

            _placementState.InitializePlacementData();
        }

        public void Enable()
        {
            Debug.Log("<color=lime>Placement Manager Enabled</color>");
            _clickCellEvent.AddEvent(OnClickCell);
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
    }
}