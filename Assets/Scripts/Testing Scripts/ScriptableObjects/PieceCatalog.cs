using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Piece Catalog", menuName = "SO/Piece Catalog")]
    public class PieceCatalog : ScriptableObject
    {
        [SerializeField] private PieceInfo[] _info;

        public int NumberOfPieces => _info.Length;

        public PieceInfo Get(int ID) => _info[ID];
    }
}