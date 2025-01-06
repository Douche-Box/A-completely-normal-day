using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletinBoard : MonoBehaviour
{
    [SerializeField] List<YarnLine> _yarnLines = new();
    public List<YarnLine> YarnLines
    { get { return _yarnLines; } set { _yarnLines = value; UpdateYarnLines(); } }

    [SerializeField] List<ClueInfo> _cluesOnBoard = new();

    [SerializeField] ClueData[] _cluesNeeded;

    private void UpdateYarnLines()
    {
        _cluesOnBoard.Clear();

        foreach (YarnLine yarnLine in _yarnLines)
        {
            foreach (Polaroid polaroids in yarnLine.AttachedPolaroids)
            {

                foreach (ClueInfo clueInfos in polaroids.ClueInfos)
                {
                    _cluesOnBoard.Add(clueInfos);

                    CheckForPhotoClueQuality(clueInfos);
                }
            }
        }
    }

    void CheckForPhotoClueQuality(ClueInfo clueInfo)
    {
        clueInfo.cluePercentage = GetPercentage(clueInfo.clueData.cluePointsNeeded, clueInfo.cluePoints);

        if (clueInfo.cluePercentage >= 100)
        {
            clueInfo.validClue = true;

            CheckForWin();
        }
    }

    void CheckForWin()
    {
        // Early exit if we don't have enough clues
        if (_cluesOnBoard.Count < _cluesNeeded.Length)
        {
            return;
        }

        // Filter to only valid clues first
        var validClues = _cluesOnBoard.Where(c => c.validClue).ToList();
        if (validClues.Count < _cluesNeeded.Length)
        {
            return;
        }

        // Track found clues
        int matchedClues = 0;

        // Check each required clue
        foreach (var requiredClue in _cluesNeeded)
        {
            if (validClues.Any(c => c.clueData == requiredClue))
            {
                matchedClues++;
            }
        }

        // Win condition
        if (matchedClues == _cluesNeeded.Length)
        {
            Debug.Log("You win!");
            // TODO: Trigger win state
        }
    }

    float GetPercentage(float maxValue, float value)
    {
        Debug.Log($"{maxValue} {value}");
        if (maxValue == 0)
        {
            Debug.Log("Max value cannot be 0");
            return 0.0f;
        }
        Debug.Log($"{value / maxValue * 100}%");
        return value / maxValue * 100;
    }
}
