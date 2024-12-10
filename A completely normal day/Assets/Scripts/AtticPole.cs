using UnityEngine;

public class AtticPole : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            other.GetComponentInParent<Animator>().SetBool("Open", true);
        }
    }
}
