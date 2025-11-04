using System;

namespace Game
{
    [Serializable]
    public class PremadeGameSettings
    {
        public GameSetting[] Settings;
    }

    [Serializable]
    public class  GameSetting
    {
        public int PlayerID;
        public PieceSetting[] Pieces;
    }
    [Serializable]
    public class PieceSetting
    {
        public int pieceID;
        public int row;
        public int column;
    }
}