using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnLine : MonoBehaviour
{
    [SerializeField] LineRenderer _yarnLine;

    [SerializeField] List<Transform> _connectedThumbtacks = new();

    private void Update()
    {
        for (int i = 0; i < _connectedThumbtacks.Count; i++)
        {
            _yarnLine.SetPosition(i, _connectedThumbtacks[i].position);
        }
    }

    public void AttachOrDetachThumbtack(Transform newThumbtack)
    {
        if (_connectedThumbtacks.Contains(newThumbtack))
        {
            DetachThumbtack(newThumbtack);
        }
        else
        {
            AttachThumbtack(newThumbtack);
        }
    }

    void AttachThumbtack(Transform thumbtackToAttach)
    {
        _yarnLine.positionCount++;

        _connectedThumbtacks.Add(thumbtackToAttach);
    }

    void DetachThumbtack(Transform thumbtackToDetach)
    {
        _yarnLine.positionCount--;

        _connectedThumbtacks.Remove(thumbtackToDetach);
    }
}
