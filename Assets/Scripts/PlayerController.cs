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
        Move();
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
        animator.SetTrigger("stopShooting");
    }

    /// <summary>
    /// Moves the player by an increment in the given direction
    /// </summary>
    private void Move()
    {
        if (_moving.Equals(Vector2.zero)) {
            animator.SetBool("Running", false);
            return;
        }        

        var change = new Vector3(_moving.x * Speed * Time.deltaTime, 0,
            _moving.y * Speed * Time.deltaTime);

        this.transform.Translate(change, Space.World);
        
        Rotate();
    }

    private void Rotate()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = new Vector3(_moving.x, 0, _moving.y);

        // The step size is equal to speed times frame time.
        float singleStep = Speed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
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
        animator.SetTrigger("Shoot");
    }

}
