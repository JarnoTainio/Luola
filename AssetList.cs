using UnityEngine;

[CreateAssetMenu(fileName = "AssetList", menuName = "GemRPG/AssetList")]
public class AssetList : ScriptableObject
{
    [SerializeField]
    public Object[] items;
}
