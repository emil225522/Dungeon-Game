using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class RoomConstants
    {
        public const int DOOR_NONE = 0;
        public const int DOOR_CLOSED = 1;
        public const int DOOR_OPEN = 2;

        public static Vector2[] DOOR_POSITIONS = { new Vector2(50, 300), new Vector2(450, 50), new Vector2(850, 300), new Vector2(450, 550) };

        public enum Direction
        {
            Left,
            Up,
            Right,
            Down
        }

        public enum TypeOfRoom
        {
            Normal,
            Puzzle,
            Boss,
            Bonus,
            Empty
        }
    }
}
