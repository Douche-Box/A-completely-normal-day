using System.Collections.Generic;
using UnityEngine;

public class YarnLine : MonoBehaviour
{
    [SerializeField] LineRenderer _yarnLine;
    [SerializeField] List<Transform> _connectedPoints = new();
    [SerializeField] List<Polaroid> _attachedPolaroids = new();

    private Transform _startThumbtack;
    private bool _isDrawing;
    public List<Polaroid> AttachedPolaroids => _attachedPolaroids;

    [SerializeField] BulletinBoard _bulletinBoard;

    private void Awake()
    {
        _bulletinBoard = FindObjectOfType<BulletinBoard>();
    }

    private void Update()
    {
        UpdateLinePositions();
    }

    public bool TryStartConnection(Transform thumbtack)
    {
        if (!_isDrawing)
        {
            _connectedPoints.Clear();
            _attachedPolaroids.Clear();
            _isDrawing = true;
        }

        if (_startThumbtack == null)
        {
            _startThumbtack = thumbtack;
            if (!_connectedPoints.Contains(thumbtack))
            {
                _connectedPoints.Add(thumbtack);
                UpdatePolaroids(thumbtack);
            }
            _yarnLine.positionCount = _connectedPoints.Count + 1;
            UpdateLinePositions();
            return true;
        }
        return false;
    }

    public bool TryCompleteConnection(Transform endThumbtack)
    {
        if (_startThumbtack == null || !CanConnect(_startThumbtack, endThumbtack))
        {
            return false;
        }

        AddPoint(endThumbtack);
        _startThumbtack = endThumbtack;
        return true;
    }

    public void StopDrawing()
    {
        _isDrawing = false;
        _startThumbtack = null;

        if (_connectedPoints.Count < 2)
        {
            Destroy(gameObject);
        }

        _bulletinBoard.AddYarnLines(this);
    }

    public void UpdatePreviewPosition(Vector3 position)
    {
        if (_startThumbtack != null)
        {
            _yarnLine.SetPosition(_yarnLine.positionCount - 1, position);
        }
    }

    private bool CanConnect(Transform from, Transform to)
    {
        if (from == to) return false;

        // Check all existing segments
        for (int i = 0; i < _connectedPoints.Count - 1; i++)
        {
            if (IsSegmentDuplicate(_connectedPoints[i], _connectedPoints[i + 1], from, to))
            {
                return false;
            }
        }

        // Check last segment if connecting back to start
        if (_connectedPoints.Count > 1)
        {
            if (IsSegmentDuplicate(_connectedPoints[0], _connectedPoints[_connectedPoints.Count - 1], from, to))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsSegmentDuplicate(Transform seg1Start, Transform seg1End, Transform seg2Start, Transform seg2End)
    {
        return (seg1Start == seg2Start && seg1End == seg2End) || (seg1Start == seg2End && seg1End == seg2Start);
    }

    private void AddPoint(Transform point)
    {
        _connectedPoints.Add(point);
        UpdatePolaroids(point);
        _yarnLine.positionCount = _connectedPoints.Count;
        UpdateLinePositions();
    }

    private void UpdateLinePositions()
    {
        for (int i = 0; i < _connectedPoints.Count; i++)
        {
            _yarnLine.SetPosition(i, _connectedPoints[i].position);
        }

        if (_startThumbtack != null)
        {
            _yarnLine.SetPosition(_yarnLine.positionCount - 1, _startThumbtack.position);
        }
    }

    public void DetachThumbtack(Transform thumbtack)
    {
        _connectedPoints.Remove(thumbtack);
        UpdateAttachedPolaroids();

        if (_connectedPoints.Count < 2)
        {
            _bulletinBoard.RemoveYarnLine(this);
            Destroy(gameObject);
            return;
        }
        else
        {
            _yarnLine.positionCount = _connectedPoints.Count;
            UpdateLinePositions();
        }

        _bulletinBoard.UpdateYarnLines();
    }

    private void UpdatePolaroids(Transform thumbtack)
    {
        var polaroid = thumbtack.GetComponentInChildren<Thumbtack>()?.AttachedPolaroid;
        if (polaroid != null && !_attachedPolaroids.Contains(polaroid))
        {
            _attachedPolaroids.Add(polaroid);
        }
    }

    private void UpdateAttachedPolaroids()
    {
        _attachedPolaroids.Clear();
        foreach (var point in _connectedPoints)
        {
            UpdatePolaroids(point);
        }
    }

    private void CancelConnection()
    {
        _startThumbtack = null;
        UpdateLinePositions();
    }
}