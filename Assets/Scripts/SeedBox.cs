using UnityEngine;

public class SeedBox : NonSoilInteractable
{
    [SerializeField] private float _timeToCompletion = 5f;
    [SerializeField] private ScriptablePlant _defaultSeedDrop;
    [SerializeField] private float _clickMultiplier = 3f;
    [SerializeField] private Transform _visualPlant = null;

    [SerializeField] private bool _autoCollect = false;
    public bool AutoCollect { set => _autoCollect = value; }

    float _tick;

    protected override void Awake()
    {
        base.Awake();

        _tick = 0;
    }

    public override void OnClick()
    {
        base.OnClick();

        if (CheckTickValue())
            AddTickValue(_clickMultiplier);
        else
            TryCollectDrop();
    }

    private void Update()
    {
        if (CheckTickValue())
            AddTickValue();
        else if (_autoCollect)
            TryCollectDrop();
    }

    private void AddTickValue(float multiplier = 1)
    {
        _tick += (Time.deltaTime * multiplier) / _timeToCompletion;
        _visualPlant.localScale = Vector3.one * Mathf.Clamp(_tick, 0, 1);
    }

    private bool CheckTickValue()
    {
        Debug.Log(_tick);
        return _tick < 1;
    }

    private void TryCollectDrop()
    {
        if (SoilManager.TryPlaceSeedDrop(_defaultSeedDrop))
            _tick = 0;
    }
}
