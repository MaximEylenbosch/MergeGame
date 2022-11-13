using System;
using UnityEngine;

public class Soil : MonoBehaviour, IInteractable
{
    private Transform _plantRoot = null;
    public Transform PlantRoot { get => _plantRoot; }

    public Plant CurrentPlant { get => _currentPlant; }
    private Plant _currentPlant;

    private SoilManager _soilManager;

    private void Awake()
    {
        _plantRoot = transform.GetChild(0);
        _soilManager = FindObjectOfType<SoilManager>();
    }

    public void PlacePlant(ScriptablePlant plant)
    {
        GameObject plantObject = Instantiate(plant.Prefab, _plantRoot.position, _plantRoot.rotation, _plantRoot);
        _currentPlant = plantObject.GetComponent<Plant>();
    }

    internal void PlaceExistingPlant(Plant holdingPlant)
    {
        _currentPlant = holdingPlant;
    }

    public bool IsEmpty()
    {
        return _currentPlant == null;
    }

    public void ClearPlant()
    {
        Destroy(_currentPlant.gameObject);
        _currentPlant = null;
    }

    internal void RepositionPlant()
    {
        _currentPlant = null;
    }

    public void OnClick()
    {
        Debug.Log("Clicked on: " + gameObject.name);
        _soilManager.TryClickPlant(this);
    }

    public void OnRelease()
    {
        Debug.Log("Released mouse on: " + gameObject.name);
        _soilManager.CheckSoil(this);
    }
}
