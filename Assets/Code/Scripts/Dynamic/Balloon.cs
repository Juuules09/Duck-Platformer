using System.Collections;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] GameObject _platform;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Dissappear());
        }
    }

    IEnumerator Dissappear()
    {   
        yield return new WaitForSeconds(0.5f);
        _platform.SetActive(false);
        yield return new WaitForSeconds(2f);
        _platform.SetActive(true);
    }
}
