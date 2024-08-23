using System;

namespace NonogramCore.Core
{
    [Serializable]
    public class Tile
    {
        private TileState _state = TileState.Plain;

        public TileState State
        {
            get { return _state;}
            set { _state = value; }
        }
    }
}
