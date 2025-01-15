using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletinBoard : MonoBehaviour
{
    [SerializeField] List<YarnLine> _yarnLines = new();

    [SerializeField] List<ClueInfo> _cluesOnBoard = new();

    [SerializeField] ClueInfo[] _cluesNeeded;

    [SerializeField] WalkieTalkie _walkieTalkie;

    public void AddYarnLines(YarnLine yarnLine)
    {
        _yarnLines.Add(yarnLine);
        UpdateYarnLines();
    }

    public void RemoveYarnLine(YarnLine yarnLine)
    {
        _yarnLines.Remove(yarnLine);
        UpdateYarnLines();
    }

    private void Awake()
    {
        _walkieTalkie = FindAnyObjectByType<WalkieTalkie>();
    }

    public void UpdateYarnLines()
    {
        _cluesOnBoard.Clear();

        foreach (YarnLine yarnLine in _yarnLines)
        {
            Debug.Log("Step 1");
            foreach (Polaroid polaroids in yarnLine.AttachedPolaroids)
            {
                Debug.Log("Step 2");
                foreach (ClueInfo clueInfo in polaroids.ClueInfos)
                {
                    Debug.Log("Step 3");

                    if (!_cluesOnBoard.Contains(clueInfo))
                    {
                        _cluesOnBoard.Add(clueInfo);

                        CheckForPhotoClueQuality();
                    }
                }
            }
        }
    }

    void CheckForPhotoClueQuality()
    {
        // Reset all clue validations first
        foreach (var clue in _cluesNeeded)
        {
            clue.validClue = false;
            clue.cluePercentage = 0;
        }

        // Check each photo's individual quality
        for (int i = 0; i < _cluesOnBoard.Count; i++)
        {
            var currentClue = _cluesOnBoard[i];
            currentClue.cluePercentage = GetPercentage(currentClue.clueData.cluePointsNeeded, currentClue.cluePoints);

            // If this photo is good enough, mark its clue type as valid
            for (int y = 0; y < _cluesNeeded.Length; y++)
            {
                if (currentClue.clueData == _cluesNeeded[y].clueData)
                {
                    _cluesNeeded[y].cluePercentage += currentClue.cluePercentage;
                    break;
                }

                if (_cluesNeeded[y].cluePercentage >= 100)
                {
                    _cluesNeeded[y].validClue = true;
                    CheckForWin();
                }
            }
        }
    }

    void CheckForWin()
    {
        _walkieTalkie.CanWin = false;

        // Early exit if we don't have enough clues
        if (_cluesOnBoard.Count < _cluesNeeded.Length)
        {
            return;
        }

        // Filter to only valid clues first
        var validClues = _cluesNeeded.Where(c => c.validClue).ToList();
        if (validClues.Count < _cluesNeeded.Length)
        {
            return;
        }

        for (int i = 0; i < _cluesNeeded.Length; i++)
        {
            if (!_cluesNeeded[i].validClue)
            {
                return;
            }
        }

        _walkieTalkie.CanWin = true;
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
