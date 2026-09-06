using UnityEditor;
using UnityEngine;

namespace XavierChess
{
    [CustomEditor(typeof(ChessPiece))]
    public class ChessPieceEditor : Editor
    {
        private void OnSceneGUI()
        {
            ChessPiece piece = (ChessPiece)target;
            Vector2Int oldBoardPos = piece.BoardPosition;

            EditorGUI.BeginChangeCheck();

            Vector3 newPosition = Handles.FreeMoveHandle(
                piece.transform.position,
                HandleUtility.GetHandleSize(piece.transform.position) * 0.5f,
                Vector3.zero,
                Handles.RectangleHandleCap);

            if (EditorGUI.EndChangeCheck())
            {
                ChessBoard board = piece.GetComponentInParent<ChessBoard>();

                if (board != null)
                {
                    Vector3 localPosition =
                        board.transform.InverseTransformPoint(newPosition);

                    localPosition.x = Mathf.Round(localPosition.x);
                    localPosition.z = Mathf.Round(localPosition.z);

                    localPosition.y = piece.transform.localPosition.y;

                    Undo.RecordObject(piece.transform, "Move Chess Piece");

                    piece.transform.localPosition = localPosition;

                    Vector2Int newBoardPos = piece.BoardPosition;

                    board.MovePiece(
                        piece,
                        oldBoardPos,
                        newBoardPos
                    );
                }

                EditorUtility.SetDirty(piece);
                SceneView.RepaintAll();
            }
        }
    }
}