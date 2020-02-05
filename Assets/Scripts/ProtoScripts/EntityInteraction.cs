using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.UISystem;
using Venus.Interaction;

public class EntityInteraction : MonoBehaviour
{
    [SerializeField]
    private LayerMask friendlyDetectionLayers;

    [SerializeField]
    private float detectionRadius = 3;

    IInteractable interactable;

    private void Start()
    {
        //Subscribe to the interact delegate
        InputManager.S_INSTANCE.Interact += TryInteract;
        InputManager.S_INSTANCE.FrameUpdate += TryUpdateKeyPopUpPosition;
    }

    private void Update()
    {
        CheckForInteractions();
    }

    /// <summary>
    /// Searches for the closest interactable and references to it with the interactable
    /// </summary>
    private void CheckForInteractions ()
    {
        interactable = GetClosestInteractable(GetComponentsFromColliderArray<IInteractable>(Physics.OverlapSphere(transform.position, detectionRadius, friendlyDetectionLayers))); //A long boi getting the closest intentory that can be traded with.
    }

    /// <summary>
    /// Tries to intract with the possible closest interactable
    /// </summary>
    public void TryInteract ()
    {
        if (interactable != null && InputManager.S_INSTANCE.CanInteract)
        {
            interactable.Interact();
        }
    }
    
    /// <summary>
    /// Tries to update the key pop up position to the closest interactable.
    /// </summary>
    public void TryUpdateKeyPopUpPosition ()
    {
        if (interactable != null && InputManager.S_INSTANCE.CanInteract) //Quick and dirty xd
        {
            UIManager.S_INSTANCE.SetKeyPopupActive(true);
            UIManager.S_INSTANCE.UpdateKeyPopupPosition(interactable.GetTransform().position);
        }
        else
        {
            UIManager.S_INSTANCE.SetKeyPopupActive(false);
        }
    }

    /// <summary>
    /// Searches through the list of interactables and finds the closest one of them and returns it.
    /// </summary>
    /// <param name="interactables">List of the interactables to search from.</param>
    /// <returns></returns>
    private IInteractable GetClosestInteractable (IInteractable[] interactables)
    {
        if (interactables == null || interactables.Length <= 0) //Array null or empty. Return
        {
            return null;
        }

        IInteractable closestInteractable = interactables[0]; //Choose the first element for starters

        float closestDistance = Vector3.Distance(transform.position, closestInteractable.GetTransform().position);

        foreach (IInteractable interactable in interactables)
        {
            float currentComponentDistance = Vector3.Distance(transform.position, interactable.GetTransform().position);

            if (currentComponentDistance < closestDistance) //New closest interactable
            {
                closestInteractable = interactable;
                closestDistance = currentComponentDistance;
            }
        }

        return closestInteractable;
    }

    /// <summary>
    /// Searches for the given component from the giver array of colliders and returns an array of the found components.
    /// </summary>
    /// <typeparam name="T">Type of component to search for.</typeparam>
    /// <param name="colliderArray">Found components in an array.</param>
    /// <returns></returns>
    private T[] GetComponentsFromColliderArray<T> (Collider[] colliderArray)
    {
        List<T> componentList = new List<T>();

        if (colliderArray != null && colliderArray.Length > 0)
        {
            foreach (Collider collider in colliderArray) //Loop through
            {
                T gotComponent = collider.GetComponent<T>();

                if (gotComponent != null) //Found a component
                {
                    componentList.Add(gotComponent);
                }
            }
        }

        return componentList.ToArray(); //Return list as array
    }

    /// <summary>
    /// Gets the closest component of the list of components that inherit from monobehaviour.
    /// </summary>
    /// <typeparam name="T">Type of component used.</typeparam>
    /// <param name="componentArray">Array of the components to check from.</param>
    /// <returns></returns>
    private T GetClosestComponent<T> (T[] componentArray) where T : MonoBehaviour
    {
        if (componentArray == null || componentArray.Length <= 0) //Array null or empty. Return
        {
            return null;
        }

        T closestComponent = componentArray[0]; //Choose the first element for starters

        float closestDistance = Vector3.Distance(transform.position, closestComponent.transform.position);

        foreach (T component in componentArray)
        {
            float currentComponentDistance = Vector3.Distance(transform.position, component.transform.position);

            if (currentComponentDistance < closestDistance) //New closest component
            {
                closestComponent = component;
                closestDistance = currentComponentDistance;
            }
        }

        return closestComponent;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
