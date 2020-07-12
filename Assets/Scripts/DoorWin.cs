using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWin : MonoBehaviour
{
    /// <summary>
    /// Reference to the winning message
    /// </summary>
    public GameObject WinMessage;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.name.Equals("Player"))
            Win();
    }

    private void Win()
    {
        WinMessage.SetActive(true);
    }
}
