using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Plant")]
public class ScriptablePlant : ScriptableObject
{
    public string Name;
    public int Value;

    public GameObject Prefab;
    public ScriptablePlant NextPlant;
}
