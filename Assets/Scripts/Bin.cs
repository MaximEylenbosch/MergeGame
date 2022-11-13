using UnityEngine;

public class Bin : NonSoilInteractable
{
    private Transform _sellRootTransform;

    protected override void Awake()
    {
        base.Awake();

        _sellRootTransform = transform.GetChild(0);
    }

    public override void OnRelease()
    {
        if (SoilManager.CanGrab == false)
            ActivateBin();
    }

    private void ActivateBin()
    {
        Debug.Log("Removed plant: " + SoilManager.HoldingPlant.gameObject.name);

        SoilManager.RemovePlant(_sellRootTransform.position);
    }
}
