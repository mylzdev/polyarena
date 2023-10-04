using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody bulletRB;
    public float speed = 10f;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        bulletRB.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Target>() != null)
        {
            // hit target
            Debug.Log("target hit");
        } 
        else
        {
            // hit something
            Debug.Log("target something");
        }

        Destroy(gameObject); 
    }
}
