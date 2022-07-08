using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ModelContainer", menuName = "ScriptableObjects/ModelContainer", order = 1)]
public class ModelContainerSO : ScriptableObject
{
    public List<GameObject> modelPrefabs = new List<GameObject>();
}
