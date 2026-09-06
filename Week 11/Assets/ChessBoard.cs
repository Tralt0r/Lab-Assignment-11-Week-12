using UnityEngine;

namespace XavierChess
{
    public class ChessBoard : MonoBehaviour
    {
        [Header("Board Layout")]
        [SerializeField] private float boardHeight = -1f;

        [Header("White Piece Sprites")]
        public Sprite pawnSprite;
        public Sprite rookSprite;
        public Sprite knightSprite;
        public Sprite bishopSprite;
        public Sprite queenSprite;
        public Sprite kingSprite;

        private static readonly PieceType[] BackRank =
        {
            PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen,
            PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook
        };

        protected struct TileInfo
        {
            public Vector2Int position;
            public Team team;
            public ChessPiece occupant;
        }

        private readonly TileInfo[,] tiles = new TileInfo[8, 8];

        private void Reset()
        {
            SpawnWhiteSet();
        }

        private void Start()
        {
            BuildBoard();
        }

        private void OnValidate()
        {
            ApplySpritesToExistingPieces();
        }

        private void ApplySpritesToExistingPieces()
        {
            foreach (ChessPiece piece in GetComponentsInChildren<ChessPiece>())
            {
                piece.Setup(piece.pieceType, piece.team, piece.tint,
                    pawnSprite, rookSprite, knightSprite, bishopSprite, queenSprite, kingSprite);
            }
        }

        [ContextMenu("Spawn White Set")]
        public void SpawnWhiteSet()
        {
            ClearPieces();

            for (int x = 0; x < 8; x++)
            {
                SpawnPiece(PieceType.Pawn, x, 1);
                SpawnPiece(BackRank[x], x, 0);
            }

            BuildBoard();
        }

        [ContextMenu("Clear All Pieces")]
        public void ClearPieces()
        {
            ChessPiece[] existing = GetComponentsInChildren<ChessPiece>();
            for (int i = existing.Length - 1; i >= 0; i--)
            {
                GameObject go = existing[i].gameObject;
                if (Application.isPlaying)
                    Destroy(go);
                else
                    DestroyImmediate(go);
            }
        }

        private void SpawnPiece(PieceType type, int x, int y)
        {
            GameObject go = new GameObject($"White {type}");
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(x, boardHeight, y);

            go.AddComponent<SpriteRenderer>();
            ChessPiece piece = go.AddComponent<ChessPiece>();
            piece.Setup(type, Team.White, Color.white,
                pawnSprite, rookSprite, knightSprite, bishopSprite, queenSprite, kingSprite);

            PlacePiece(piece);
        }

        public void PlacePiece(ChessPiece piece)
        {
            if (piece == null) return;

            Vector2Int pos = piece.BoardPosition;
            if (!IsOnBoard(pos)) return;

            tiles[pos.x, pos.y] = new TileInfo
            {
                position = pos,
                team = piece.team,
                occupant = piece
            };
        }

        public void MovePiece(ChessPiece piece, Vector2Int from, Vector2Int to)
        {
            if (piece == null) return;

            if (IsOnBoard(from) && tiles[from.x, from.y].occupant == piece)
            {
                tiles[from.x, from.y] = new TileInfo
                {
                    position = from,
                    team = Team.None,
                    occupant = null
                };
            }

            if (IsOnBoard(to))
            {
                tiles[to.x, to.y] = new TileInfo
                {
                    position = to,
                    team = piece.team,
                    occupant = piece
                };
            }
        }

        private bool IsOnBoard(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
        }

        private void BuildBoard()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    tiles[x, y] = new TileInfo
                    {
                        position = new Vector2Int(x, y),
                        team = Team.None,
                        occupant = null
                    };
                }
            }

            foreach (ChessPiece piece in FindObjectsOfType<ChessPiece>())
            {
                PlacePiece(piece);
            }
        }

        private void OnDrawGizmos()
        {
            Color colorA = Color.white;
            Color colorB = Color.black;

            Matrix4x4 oldMatrix = Gizmos.matrix;

            Gizmos.matrix = transform.localToWorldMatrix;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Gizmos.color = (x + y) % 2 == 0
                        ? colorA
                        : colorB;

                    Vector3 position = new Vector3(x, boardHeight, y);

                    Gizmos.DrawWireCube(
                        position,
                        new Vector3(1, 0.01f, 1)
                    );
                }
            }
            Gizmos.matrix = oldMatrix;
        }
    }
}