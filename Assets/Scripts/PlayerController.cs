using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Receives events from the input system and controls the player
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Reference to the animator of the player
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Speed of the player in units/s
    /// </summary>
    public float Speed;

    /// <summary>
    /// Direction to where
    /// </summary>
    private Vector3 _moving = Vector3.zero;

    private void Update()
    {
        // be on the move
        Move(_moving);
    }

    /// <summary>
    /// Listener of the unity event in player input
    /// </summary>
    /// <param name="ctx"></param>
    public void Move(InputAction.CallbackContext ctx)
    {  
        // sliding effect
        //_moving = Vector2.Lerp(ctx.action.ReadValue<Vector2>(), _moving, .5f);

        // raw
        _moving = ctx.action.ReadValue<Vector2>();
        animator.SetBool("Running", true);
    }

    /// <summary>
    /// Moves the player by an increment in the given direction
    /// </summary>
    /// <param name="direction">Direction to move</param>
    private void Move(Vector2 direction)
    {
        if (_moving.Equals(Vector2.zero)) {
            animator.SetBool("Running", false);
            return;
        }
        
        var change = new Vector3(_moving.x * Speed * Time.deltaTime, 0,
            _moving.y * Speed * Time.deltaTime);

        this.transform.Translate(change, Space.World);
    }

    /// <summary>
    /// Listener of the unity event in player input
    /// </summary>
    /// <param name="ctx"></param>
    public void Fire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Fire();
        }
    }

    /// <summary>
    /// Fires a projectile in the orientation of the player
    /// </summary>
    private void Fire()
    {
        animator.SetBool("Shoot", true);
    }
}
