using UnityEngine;

public class AddForcePiecesPot : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX;
    float dirY;
    float torque;
    // Start is called before the first frame update
    void Start()
    {
        dirX = Random.Range(-5, 5);
        dirY = Random.Range(-5, 8);
        torque = Random.Range(5, 15);
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2 (dirX, dirY), ForceMode2D.Impulse);
        rb.AddTorque(torque, ForceMode2D.Force);
        Destroy(gameObject, 1f);
    }
}
