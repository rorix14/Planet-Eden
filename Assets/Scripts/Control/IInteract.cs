using UnityEngine;
using System;

namespace RPG.Control
{
    public interface IInteract
    {
        event Action<bool> removeControl;

        void Interact(GameObject player);
    }
}