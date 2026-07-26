using UnityEngine;

/// <summary>
/// Add a little weird asteroid to the main menu to spice things up
/// </summary>
public class FlyAroundMainMenu : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private float yMin = -10f;
    [SerializeField] private float yMax = 14.5f;
    [SerializeField] private float spinSpeed = 74f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = spinSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * (scrollSpeed * Time.deltaTime), Space.World);
        if (transform.position.y > yMax)
        {
            transform.position = new Vector3(transform.position.x, yMin, transform.position.z);
        }
    }
}
