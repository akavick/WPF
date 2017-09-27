using System;

namespace ChessHorseWalk
{
    public struct IntPoint : IEquatable<IntPoint>
    {
        public int X;
        public int Y;

        public static bool operator ==(IntPoint left, IntPoint right) => left.X == right.X && left.Y == right.Y;
        public static bool operator !=(IntPoint left, IntPoint right) => left.X != right.X || left.Y != right.Y;

        public bool Equals(IntPoint other) => X == other.X && Y == other.Y;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is IntPoint ip && Equals(ip);
        }

        public override int GetHashCode() => (X * 397) ^ Y;
    }
}
