using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnLine : MonoBehaviour
{
    [SerializeField] LineRenderer _yarnLine;

    [SerializeField] List<Transform> _connectedThumbtacks = new();

    [SerializeField] Transform _previousThumbtack;

    private void Update()
    {
        for (int i = 0; i < _connectedThumbtacks.Count; i++)
        {
            _yarnLine.SetPosition(i, _connectedThumbtacks[i].position);
        }
    }

    public void AttachThumbtack(Transform thumbtackToAttach)
    {
        if (_previousThumbtack != null)
        {
            if (!thumbtackToAttach.GetComponent<Thumbtack>().ConnectedThumbtacks.Contains(_previousThumbtack) && thumbtackToAttach.GetComponent<Thumbtack>().ConnectedThumbtacks.Count > 0)
            {
                _yarnLine.positionCount++;

                _connectedThumbtacks.Add(thumbtackToAttach);

                thumbtackToAttach.GetComponent<Thumbtack>().ConnectedThumbtacks.Add(_previousThumbtack);

                _previousThumbtack.GetComponent<Thumbtack>().ConnectedThumbtacks.Add(thumbtackToAttach);

                _previousThumbtack = thumbtackToAttach;
            }
        }
        else
        {
            _yarnLine.positionCount++;

            _connectedThumbtacks.Add(thumbtackToAttach);

            _previousThumbtack = thumbtackToAttach;
        }
    }

    // Might need a rework // // Might need a rework // // Might need a rework // // Might need a rework //
    public void DetachThumbtack(Transform thumbtackToDetach)
    {
        _yarnLine.positionCount--;

        _connectedThumbtacks.Remove(thumbtackToDetach);

        if (_yarnLine.positionCount == 0)
        {
            Destroy(gameObject);
        }
    }
    // Might need a rework // // Might need a rework // // Might need a rework // // Might need a rework //
}
