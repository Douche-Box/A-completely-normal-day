using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string nextScene;

    public float transition = 1.0f;

    public void Move()
    {
        StartCoroutine(Go());
    }

    public IEnumerator Go()
    {
        yield return new WaitForSeconds(transition);

        SceneManager.LoadScene(nextScene);
    }
}