using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "TextureContainer", menuName = "ScriptableObjects/TextureContainer", order = 1)]
public class TextureContainerSO : ScriptableObject
{
    public List<Material> materials = new();
}
