using UnityEngine;

namespace XavierChess
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChessPiece : MonoBehaviour
    {
        public PieceType pieceType = PieceType.Pawn;
        public Team team = Team.White;
        public Color tint = Color.white;
        public Sprite pawnSprite;
        public Sprite rookSprite;
        public Sprite knightSprite;
        public Sprite bishopSprite;
        public Sprite queenSprite;
        public Sprite kingSprite;

        private SpriteRenderer spriteRenderer;

        public Vector2Int BoardPosition =>
            new Vector2Int(
                Mathf.RoundToInt(transform.localPosition.x),
                Mathf.RoundToInt(transform.localPosition.z)
            );
        private static Quaternion UprightRotation = Quaternion.Euler(-90f, 0f, 180f);

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            transform.rotation = UprightRotation;
            UpdatePiece();
        }

        private void OnValidate()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
            transform.rotation = UprightRotation;
            UpdatePiece();
        }

        public void Setup(PieceType newType, Team newTeam, Color newTint,
            Sprite pawn, Sprite rook, Sprite knight, Sprite bishop, Sprite queen, Sprite king)
        {
            pieceType = newType;
            team = newTeam;
            tint = newTint;
            pawnSprite = pawn;
            rookSprite = rook;
            knightSprite = knight;
            bishopSprite = bishop;
            queenSprite = queen;
            kingSprite = king;

            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            transform.rotation = UprightRotation;
            UpdatePiece();
        }

        private void UpdatePiece()
        {
            if (spriteRenderer == null) return;

            spriteRenderer.sprite = pieceType switch
            {
                PieceType.Pawn => pawnSprite,
                PieceType.Rook => rookSprite,
                PieceType.Knight => knightSprite,
                PieceType.Bishop => bishopSprite,
                PieceType.Queen => queenSprite,
                PieceType.King => kingSprite,
                _ => spriteRenderer.sprite
            };

            spriteRenderer.color = tint;
        }

        public Moves[] GetMoves()
        {
            int forward = team == Team.White ? 1 : -1;

            switch (pieceType)
            {
                case PieceType.Pawn:
                    return new[] { new Moves(0, forward) };

                case PieceType.Rook:
                    return new[]
                    {
                        new Moves(1, 0, true), new Moves(-1, 0, true),
                        new Moves(0, 1, true), new Moves(0, -1, true)
                    };

                case PieceType.Bishop:
                    return new[]
                    {
                        new Moves(1, 1, true), new Moves(1, -1, true),
                        new Moves(-1, 1, true), new Moves(-1, -1, true)
                    };

                case PieceType.Queen:
                    return new[]
                    {
                        new Moves(1, 0, true), new Moves(-1, 0, true),
                        new Moves(0, 1, true), new Moves(0, -1, true),
                        new Moves(1, 1, true), new Moves(1, -1, true),
                        new Moves(-1, 1, true), new Moves(-1, -1, true)
                    };

                case PieceType.King:
                    return new[]
                    {
                        new Moves(1, 0), new Moves(-1, 0),
                        new Moves(0, 1), new Moves(0, -1),
                        new Moves(1, 1), new Moves(1, -1),
                        new Moves(-1, 1), new Moves(-1, -1)
                    };

                case PieceType.Knight:
                    return new[]
                    {
                        new Moves(1, 2), new Moves(2, 1),
                        new Moves(2, -1), new Moves(1, -2),
                        new Moves(-1, -2), new Moves(-2, -1),
                        new Moves(-2, 1), new Moves(-1, 2)
                    };
            }

            return new Moves[0];
        }


        //CL = change location
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color (0f, 1f, 0f, 0.2f);

            Matrix4x4 oldMatrix = Gizmos.matrix;

            ChessBoard board = GetComponentInParent<ChessBoard>();

            if (board != null)
                Gizmos.matrix = board.transform.localToWorldMatrix;

            Vector2Int origin = BoardPosition;

            foreach (Moves cl in GetMoves())
            {
                int x = origin.x;
                int y = origin.y;

                do
                {
                    x += cl.x;
                    y += cl.y;

                    if (x < 0 || x >= 8 || y < 0 || y >= 8)
                        break;

                    Vector3 squareCenter =
                        new Vector3(x, 0.01f, y);

                    Gizmos.DrawCube(
                        squareCenter,
                        new Vector3(0.9f, 0.01f, 0.9f)
                    );

                } while (cl.moving);
            }

            Gizmos.matrix = oldMatrix;
        }
    }
}