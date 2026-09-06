namespace XavierChess
{
    public enum Team
    {
        White,
        Black,
        None
    }

    public enum PieceType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    [System.Serializable]
    public struct Moves
    {
        public int x;
        public int y;
        public bool moving;

        public Moves(int x, int y, bool moving = false)
        {
            this.x = x;
            this.y = y;
            this.moving = moving;
        }
    }
}