using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PlacementData", menuName = "SO/Placement Data")]
    public class PlacementStateData : ScriptableObject
    {
        private enum PlacementState { Initialized, ChoosingPiece, ChoosingCell, AwaitingConfirm, Ready }

        [Header("Data")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private PieceCatalog _catalog;

        [Header("Visualisation. <color=red>DO NOT MODIFY.</color>")]
        [SerializeField] private PlacementData[][] _placementDataStatus;
        [SerializeField][ReadOnly] private int _currentPlayerIndex;
        [SerializeField][ReadOnly] private int _currentSlotIndex;
        [SerializeField][ReadOnly] private PlacementState _currentState;

        public int PlayerId => _currentPlayerIndex;

        public int SlotID => _currentSlotIndex;

        public void GenerateNewPlacementData()
        {
            _placementDataStatus = new PlacementData[_gameData.NumberOfPlayers][];

            for (int i = 0; i < _placementDataStatus.Length; i++)
            {
                _placementDataStatus[i] = new PlacementData[_gameData.PiecesPerPlayer];
            }

            _currentState = PlacementState.Initialized;
        }

        public void BeginPlacement()
        {
            // need to check if current state = initialized or ready.


            _currentPlayerIndex = 0;
            _currentSlotIndex = 0;
            _currentState = PlacementState.ChoosingPiece;
        }

        //public PlacementData Get(int playerID)
        //{

        //}


    }
}