using System;
using System.Collections;
using UnityEngine;

public enum PlantAction { Cancel, Move, Merge, Remove }
public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _soilLayerMask;

    private SoilManager _soilManager;

    private void Awake()
    {
        _soilManager = FindObjectOfType<SoilManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _soilManager.CanGrab)
        {
            if (TryClickInteractable(out IInteractable interactable))
                interactable.OnClick();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (TryClickInteractable(out IInteractable interactable))
                interactable.OnRelease();
            //else if (_soilManager.HoldingPlantSoil != null)
            //    _soilManager.DropPlant();
        }
    }

    private bool TryClickInteractable(out IInteractable interactable)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, _soilLayerMask))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactableComponent))
            {
                interactable = interactableComponent;
                return true;
            }
        }

        interactable = null;
        return false;
    }
}
