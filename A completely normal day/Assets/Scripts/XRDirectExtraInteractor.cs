using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class XRDirectExtraInteractor : XRDirectInteractor
{
    public static event Action<string> ClimbHandActivated;
    public static event Action<string> ClimbHandDeactivated;

    [SerializeField] string _controllerName;

    [SerializeField] InputActionProperty _gripValue;
    [SerializeField] bool _isGripping;

    [SerializeField] ExpandedMovementProvider _expandedMovementProvider;

    public bool canClimb;
    public bool canMove;
    public bool canTurn;

    protected override void Awake()
    {
        base.Awake();

        _expandedMovementProvider = GetComponentInParent<ExpandedMovementProvider>();

        canMove = true;
        canTurn = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _gripValue.action.started += OnGrip;
        _gripValue.action.performed += OnGrip;
        _gripValue.action.canceled += OnGrip;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _gripValue.action.started -= OnGrip;
        _gripValue.action.performed -= OnGrip;
        _gripValue.action.canceled -= OnGrip;
    }

    private void OnGrip(InputAction.CallbackContext context)
    {
        float gripValue = context.ReadValue<float>();

        _isGripping = gripValue == 1f;
    }

    protected override void Start()
    {
        base.Start();
        _controllerName = gameObject.name;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (args.interactableObject.transform.tag == "Climbable")
        {
            canClimb = true;
            canMove = false;
            canTurn = false;
        }
        else if (args.interactableObject.transform.tag == "Teleporter")
        {
            _expandedMovementProvider.teleport = true;
        }
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        canClimb = false;
        canMove = true;
        canTurn = true;
    }
    public void FixedUpdate()
    {
        if (canClimb && _isGripping)
        {
            ClimbHandActivated?.Invoke(_controllerName);
        }
        else
        {
            ClimbHandDeactivated?.Invoke(_controllerName);
        }
    }
}
