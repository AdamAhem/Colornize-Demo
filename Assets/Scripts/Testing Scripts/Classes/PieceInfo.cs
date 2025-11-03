using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PieceInfo
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;

        public int ID => _id;
        public string Name => _name;
        public Sprite Icon => _icon;

        // also include move algorithm.
    }
}