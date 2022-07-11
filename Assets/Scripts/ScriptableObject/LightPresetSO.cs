using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Light Preset", menuName = "ScriptableObjects/Light Preset", order = 1)]
public class LightPresetSO : ScriptableObject
{
    public LightPreset[] presets = new LightPreset[9];
}
