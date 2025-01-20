using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] float _force = 10f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * _force, ForceMode.Impulse);
        }
    }
}
