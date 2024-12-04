using UnityEngine;

[CreateAssetMenu(fileName = "ClueData", menuName = "Data/Clue")]
public class ClueData : ScriptableObject
{
    public string clueName;
    public int cluePointsNeeded;
}
