using UnityEngine;

public class Player : MonoBehaviour
{
    #region Properties

    private const float inputRepeatDelay = 0.2f;

    public Piece activePiece;
    public float fallSpeedMultiplier = 10f;
    private bool active;
    private bool moving;

    #endregion

    #region Execution

    public void Update()
    {
        HandleInput();
    }

    #endregion

    #region Public Methods

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = true;
    }

    public void SetActivePiece(Piece piece)
    {
        if (piece != null)
        {
            activePiece = piece;

            Color color = piece.Color;
            color.a = 0.1f;

            activePiece.SetBorderColor(color);
        }
    }

    public void RemoveActivePiece()
    {
        if (!activePiece) return;

        activePiece.SetLayer(8);
        activePiece.SetBorderColor(Color.clear);
        activePiece = null;
    }

    #endregion

    #region Private Methods

    private void HandleInput()
    {
        if (!active) return;

        if (!moving && Input.GetButtonDown("Horizontal"))
        {
            moving = true;
            InvokeRepeating("MoveHorizontaly", 0f, inputRepeatDelay);
        }
        else if (moving && Input.GetButtonUp("Horizontal"))
        {
            moving = false;
            CancelInvoke("MoveHorizontaly");
        }

        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            activePiece?.Rotate();
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            activePiece?.SetSpeedMultiplier(fallSpeedMultiplier);
        }
        else
        {
            activePiece?.SetSpeedMultiplier(1f);
        }
    }

    private void MoveHorizontaly()
    {
        activePiece?.Move(Input.GetAxisRaw("Horizontal") > 0? 1 : -1, 0);
    }

    #endregion
}
