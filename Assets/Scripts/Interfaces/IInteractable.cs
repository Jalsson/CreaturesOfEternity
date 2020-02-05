using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venus.Interaction
{
    public interface IInteractable
    {
        Transform GetTransform();

        void Interact();
    }
}