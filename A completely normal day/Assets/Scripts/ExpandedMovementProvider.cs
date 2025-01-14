using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class ExpandedMovementProvider : MonoBehaviour
{
    public static event Action ClimbActive;
    public static event Action ClimbInActive;

    [SerializeField] private CharacterController charachter;
    [SerializeField] private ContinuousMoveProviderBase movementProvider;

    [SerializeField] private SnapTurnProviderBase _snapTurnProvider;
    [SerializeField] private ContinuousTurnProviderBase _smoothTurnProvider;

    [Header("Inputs")]
    #region
    [SerializeField] private InputActionProperty velocityLeft;
    [SerializeField] private InputActionProperty velocityRight;

    [SerializeField] private InputActionProperty gripLeft;
    [SerializeField] private InputActionProperty gripRight;

    [SerializeField] private bool gripLeftInput;
    [SerializeField] private bool gripRightInput;

    [SerializeField] private bool _rightActive = false;
    [SerializeField] private bool _leftActive = false;

    public bool teleport;

    #endregion
    [Header("Interactors")]
    #region
    [SerializeField] private XRDirectExtraInteractor extrainteractorLeft;
    [SerializeField] private XRDirectExtraInteractor extrainteractorRight;
    #endregion

    private void Start()
    {
        XRDirectExtraInteractor.ClimbHandActivated += HandActivated;
        XRDirectExtraInteractor.ClimbHandDeactivated += HandDeactivated;
    }
    private void OnDestroy()
    {
        XRDirectExtraInteractor.ClimbHandActivated += HandActivated;
        XRDirectExtraInteractor.ClimbHandDeactivated += HandDeactivated;
    }
    private void Update()
    {
        #region
        gripLeft.action.performed += hfhi => gripLeftInput = true;
        gripLeft.action.canceled += hfhi => gripLeftInput = false;

        gripRight.action.performed += hfhi => gripRightInput = true;
        gripRight.action.canceled += hfhi => gripRightInput = false;

        if (!gripRightInput)
        {
            _rightActive = false;
        }
        if (!gripLeftInput)
        {
            _leftActive = false;
        }
        #endregion

        if (!extrainteractorLeft.canMove && !extrainteractorRight.canMove && !extrainteractorLeft.canClimb && !extrainteractorRight.canClimb)
        {
            ToggleTurning(true);
        }
        else
        {
            ToggleTurning(false);
        }
        if (!extrainteractorLeft.canTurn && !extrainteractorRight.canTurn)
        {
            ToggleTurning(true);
        }
        else
        {
            ToggleTurning(false);
        }
    }
    private void HandActivated(string _controllerName)
    {
        if (_controllerName == "LeftHand Controller")
        {
            _leftActive = true;
            _rightActive = false;
        }
        else
        {
            _leftActive = false;
            _rightActive = true;
        }

        ClimbActive?.Invoke();
    }
    private void HandDeactivated(string _controllerName)
    {
        if (_rightActive && _controllerName == "RightHand Controller")
        {
            _rightActive = false;
            ClimbActive?.Invoke();
        }
        else if (_leftActive && _controllerName == "LeftHand Controller")
        {
            _leftActive = false;
            ClimbActive?.Invoke();
        }
    }
    private void FixedUpdate()
    {
        if (teleport)
        {
            teleport = false;
            extrainteractorLeft.canClimb = false;
            extrainteractorRight.canClimb = false;
        }

        if (_rightActive || _leftActive)
        {
            ToggleMovement(false);

            movementProvider.useGravity = false;

            if (extrainteractorLeft.canClimb || extrainteractorRight.canClimb)
            {
                Climb();
            }
        }
        else
        {
            if (extrainteractorLeft.canMove && extrainteractorRight.canMove)
            {
                ToggleMovement(true);
            }
            else
            {
                return;
            }

            movementProvider.useGravity = true;
        }
    }
    private void ToggleMovement(bool newToggle)
    {
        movementProvider.enabled = newToggle;
    }

    private void ToggleTurning(bool newToggle)
    {
        _smoothTurnProvider.enabled = newToggle;
    }

    private void Climb()
    {
        // Determine which hand's velocity to use
        Vector3 handVelocity;

        if (_leftActive)
        {
            handVelocity = velocityLeft.action.ReadValue<Vector3>(); // Use left hand velocity
        }
        else if (_rightActive)
        {
            handVelocity = velocityRight.action.ReadValue<Vector3>(); // Use right hand velocity
        }
        else
        {
            return; // No active hand; no climbing
        }

        // Apply the velocity for climbing
        charachter.Move(charachter.transform.rotation * -handVelocity * Time.fixedDeltaTime);
    }

}