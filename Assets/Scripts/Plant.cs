using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private ScriptablePlant _scriptablePlantObject = null;

    public ScriptablePlant ScriptablePlant { get => _scriptablePlantObject; }
}
