using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Light Preset Container", menuName = "ScriptableObjects/Light Preset Container", order = 1)]
public class LightPresetContainerSO : ScriptableObject
{
    public List<LightPresetSO> presets = new();
}
