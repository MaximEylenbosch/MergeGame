using System;
using System.Collections;
using UnityEngine;

public class SoilManager : MonoBehaviour
{
    [SerializeField] private Soil[] _soils;
    [SerializeField] private ScriptablePlant _basePlant = null;
    [SerializeField] private float _yOffsetHoldingPlant = 1.2f;
    [SerializeField] private LayerMask _groundLayerMask;

    private Plant _holdingPlant;
    private Transform _holdingPlantTransform;
    private Soil _holdingPlantSoil;
    private bool _followMouse = false;
    private bool _canGrab = true;

    public bool CanGrab { get => _canGrab; }
    public Plant HoldingPlant { get => _holdingPlant; }

    public void DropPlant()
    {
        StartCoroutine(PerformPlantAction(_holdingPlantSoil.PlantRoot.position, 0.1f, PlantAction.Cancel));
    }

    internal bool TryPlaceSeedDrop(ScriptablePlant plant)
    {
        if (SoilSpaceAvailable(out Soil emptySoil))
        {
            emptySoil.PlacePlant(plant);
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SoilSpaceAvailable(out Soil emptySoil))
                emptySoil.PlacePlant(_basePlant);
            else
                Debug.LogError("No free soil spots available!");
        }

        if (_followMouse)
            PlantFollowMouse();
    }

    private void PlantFollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, _groundLayerMask))
        {
            _holdingPlantTransform.position = hit.point + new Vector3(0, _yOffsetHoldingPlant, 0);
        }
    }

    private bool SoilSpaceAvailable(out Soil soil)
    {
        soil = null;
        for (int i = 0; i < _soils.Length; i++)
        {
            if (_soils[i].IsEmpty())
            {
                soil = _soils[i];
                return true;
            }
        }

        return false;
    }

    public void CheckSoil(Soil soil)
    {
        if (_holdingPlant == null)
            return;

        if (soil.IsEmpty())
            StartCoroutine(PerformPlantAction(soil.PlantRoot.position, 0.1f, PlantAction.Move, soil));
        else if (soil != _holdingPlantSoil)
            CheckPlant(soil);
        else
            StartCoroutine(PerformPlantAction(_holdingPlantSoil.PlantRoot.position, 0.1f, PlantAction.Cancel, soil));
    }

    public void CheckPlant(Soil soil)
    {
        if (_holdingPlant.ScriptablePlant == soil.CurrentPlant.ScriptablePlant)
            StartCoroutine(PerformPlantAction(soil.PlantRoot.position, 0.1f, PlantAction.Merge, soil));
        else
            StartCoroutine(PerformPlantAction(_holdingPlantSoil.PlantRoot.position, 0.1f, PlantAction.Cancel, soil));
    }

    IEnumerator PerformPlantAction(Vector3 targetPosition, float duration, PlantAction action, Soil soil = null)
    {
        _followMouse = false;
        float time = 0;
        Vector3 startPosition = _holdingPlantTransform.position;
        while (time < duration)
        {
            _holdingPlantTransform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _holdingPlantTransform.position = targetPosition;

        switch (action)
        {
            case PlantAction.Cancel:
                ClearHoldingPlant();
                break;
            case PlantAction.Move:
                Move(soil);
                break;
            case PlantAction.Merge:
                Merge(soil);
                break;
            case PlantAction.Remove:
                Remove();
                break;
            default:
                ClearHoldingPlant();
                break;
        }
    }

    private void Move(Soil soil)
    {
        _holdingPlantSoil.RepositionPlant();
        soil.PlaceExistingPlant(_holdingPlant);

        ClearHoldingPlant();
    }

    private void Merge(Soil soil)
    {
        _holdingPlantSoil.ClearPlant();
        soil.ClearPlant();
        soil.PlacePlant(_holdingPlant.ScriptablePlant.NextPlant);

        ClearHoldingPlant();
    }

    public void RemovePlant(Vector3 targetPos)
    {
        StartCoroutine(PerformPlantAction(targetPos, 0.1f, PlantAction.Remove));
    }

    private void Remove()
    {
        _holdingPlantSoil.ClearPlant();

        ClearHoldingPlant();
    }

    private void ClearHoldingPlant()
    {
        _holdingPlant = null;
        _holdingPlantSoil = null;
        _holdingPlantTransform = null;
        _canGrab = true;
    }

    private void SelectPlant(Soil soil)
    {
        _holdingPlant = soil.CurrentPlant;
        _holdingPlantTransform = soil.CurrentPlant.transform;
        _holdingPlantSoil = soil;
        _followMouse = true;
        _canGrab = false;
    }

    public void TryClickPlant(Soil soil)
    {
        if (soil.IsEmpty())
        {
            Debug.LogWarning("Soil has no plant");
            return;
        }

        SelectPlant(soil);
    }
}
