using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOBasicDataType/New List Int Data Type", fileName = "Data List Int Name")]
public class SOListInt : ScriptableObject
{
    public List<int> itemList = new List<int>();
}