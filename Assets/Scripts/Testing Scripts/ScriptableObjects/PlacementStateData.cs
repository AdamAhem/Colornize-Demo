using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PlacementData", menuName = "SO/Placement Data")]
    public class PlacementStateData : ScriptableObject
    {
        private enum PlacementState { ChoosingPiece, ChoosingCell, AwaitingConfirm }

        [Header("Data")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private PieceCatalog _catalog;

        [Header("Visualisation")]
        [SerializeField][ReadOnly] private PlacementData[] _placementDataStatus;
        [SerializeField][ReadOnly] private int _currentPlayerIndex;
        [SerializeField][ReadOnly] private int _currentSlotIndex;
        [SerializeField][ReadOnly] private PlacementState _currentState;


        public void InitializePlacementData()
        {
            _placementDataStatus = new PlacementData[_gameData.NumberOfPlayers];

            _currentPlayerIndex = 0;
            _currentSlotIndex = 0;
            _currentState = PlacementState.ChoosingPiece;
        }
    }
}