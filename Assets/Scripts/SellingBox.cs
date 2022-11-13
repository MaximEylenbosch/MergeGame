using UnityEngine;

public class SellingBox : NonSoilInteractable
{
    private Transform _sellRootTransform;

    protected override void Awake()
    {
        base.Awake();

        _sellRootTransform = transform.GetChild(0);
    }

    public override void OnClick()
    {
        //click on ground logic
    }

    public override void OnRelease()
    {
        if (SoilManager.CanGrab == false)
            SellPlant();
    }

    private void SellPlant()
    {
        int value = SoilManager.HoldingPlant.ScriptablePlant.Value;
        Debug.Log("Added value to gameManager: " + value);
        //GameManager.Instance.AddValue(value);

        SoilManager.RemovePlant(_sellRootTransform.position);
    }
}
