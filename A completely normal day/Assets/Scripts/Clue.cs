using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    [SerializeField] List<Transform> cluePoints = new();
    public List<Transform> CluePoints
    { get { return cluePoints; } }
}
