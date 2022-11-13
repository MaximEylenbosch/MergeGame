using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonSoilInteractable : MonoBehaviour, IInteractable
{
    private SoilManager _soilManager;
    public SoilManager SoilManager { get => _soilManager; }

    protected virtual void Awake()
    {
        _soilManager = FindObjectOfType<SoilManager>();
    }
    public virtual void OnClick()
    {
        Debug.Log("Clicked on nonSoilInteractable");
    }

    public virtual void OnRelease()
    {
        if (_soilManager.CanGrab == false)
            _soilManager.DropPlant();
    }
}
