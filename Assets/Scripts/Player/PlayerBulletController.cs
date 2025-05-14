using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Rigidbody2D _bulletRb;

    private void Awake()
    {
        _bulletRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _bulletRb.linearVelocity = transform.up * speed;
    }
    
    private void Update()
    {
        Destroy(gameObject, 5f);
    }
}
