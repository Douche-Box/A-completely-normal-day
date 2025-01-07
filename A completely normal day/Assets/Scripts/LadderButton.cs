using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderButton : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void LadderBtn(bool trueOrFalse)
    {
        _animator.SetBool("Open", trueOrFalse);
    }
}
