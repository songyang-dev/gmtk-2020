using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Natural displacement between player and camera
    /// </summary>
    public Vector3 OffsetPosition;

    /// <summary>
    /// Natural rotation to look at the player
    /// </summary>
    public Vector3 DefaultRotation;

    /// <summary>
    /// How fast the camera moves, units/second
    /// </summary>
    public float Speed;

    /// <summary>
    /// Percentage of the vertical screen size to count as being inside the edge
    /// </summary>
    public float ScreenEdgeVertical;

    /// <summary>
    /// Percentage of the horizontal screen size to count as being inside the edge
    /// </summary>
    public float ScreenEdgeHorizontal;

    /// <summary>
    /// Edges of the screen
    /// </summary>
    private enum ScreenEdge
    {
        Top,
        Bottom,
        Left,
        Right
    }

    /// <summary>
    /// Detects erroneous settings at the start
    /// </summary>
    private void Start()
    {
        if (ScreenEdgeHorizontal <= 0 || ScreenEdgeHorizontal >= 1)
            Debug.LogError("horizontal edge percentage is not within (0,1)");

        if (ScreenEdgeVertical <= 0 || ScreenEdgeVertical >= 1)
            Debug.LogError("vertical edge percentage is not within (0,1)");

        if (Speed == 0)
            Debug.LogError($"Camera speed is {Speed}");
    }

    /// <summary>
    /// Constantly watches for camera movement
    /// </summary>
    private void Update()
    {
        Pan();
    }

    /// <summary>
    /// Moves the camera if the cursor is close to the edge of the screen
    /// </summary>
    private void Pan()
    {
        // detects whether the mouse is inside edges of the screen
        // if not, don't move
        var inEdges = InScreenEdge(Mouse.current.position.ReadValue());

        // move in that direction
        Move(inEdges);
    }

    /// <summary>
    /// Moves the camera
    /// </summary>
    /// <param name="horizontal">Which horizontal edge is touched</param>
    /// <param name="vertical">Which vertical edge is touched</param>
    private void Move((ScreenEdge? horizontal, ScreenEdge? vertical) inEdges)
    {
        // left-right is x-axis in world space
        // up-down is z-axis in world space

        switch (inEdges.horizontal)
        {
            case ScreenEdge.Left:
                this.transform.Translate(-1 * Speed * Time.deltaTime, 0, 0,
                    Space.World);
                break;

            case ScreenEdge.Right:
                this.transform.Translate(Speed * Time.deltaTime, 0, 0,
                    Space.World);
                break;

            default:
                break;
        }

        switch (inEdges.vertical)
        {
            case ScreenEdge.Bottom:
                this.transform.Translate(0,0, -1 * Speed * Time.deltaTime,
                    Space.World);
                break;

            case ScreenEdge.Top:
                this.transform.Translate(0,0, Speed * Time.deltaTime,
                    Space.World);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Determines whether the mouse pointer is close to the screen
    /// </summary>
    /// <param name="position">Position of the mouse in pixels</param>
    /// <returns>Touched edges</returns>
    private (ScreenEdge? horizontal, ScreenEdge? vertical) InScreenEdge(Vector2 position)
    {
        var normalizedVerticalPosition = position.y / Screen.height;
        var normalizedHorizontalPosition = position.x / Screen.width;
        
        (ScreenEdge? horizontal, ScreenEdge? vertical) result = (null, null);

        // touches the left of the screen
        if (normalizedHorizontalPosition < this.ScreenEdgeHorizontal)
        {
            result.horizontal = ScreenEdge.Left;
        }
        // touches the right of the screen
        if (normalizedHorizontalPosition > 1 - this.ScreenEdgeHorizontal)
        {
            result.horizontal = ScreenEdge.Right;
        }

        // touches the bottom of the screen
        if (normalizedVerticalPosition < this.ScreenEdgeVertical)
        {
            result.vertical = ScreenEdge.Bottom;
        }

        // touches the top of the screen
        if (normalizedVerticalPosition > 1 - this.ScreenEdgeVertical)
        {
            result.vertical = ScreenEdge.Top;
        }

        return result;

    }
}
