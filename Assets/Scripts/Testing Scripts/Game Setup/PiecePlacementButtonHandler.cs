using UnityEngine;

namespace Game
{
    public class PiecePlacementButtonHandler : MonoBehaviour
    {
        [Header("IDs")]
        [SerializeField] private int _slotID;

        [Header("Events")]
        [SerializeField] private GameEvent _clickedPiecePlacementButtonEvent;

        [Header("Data")]
        [SerializeField] private PlacementStateData _placementStateData;

        public void OnPressPiece_UI_BUTTON()
        {
            _placementStateData.UpdateLastClickedPiecePlacementButton(_slotID);
            _clickedPiecePlacementButtonEvent.Raise();
        }
    }
}