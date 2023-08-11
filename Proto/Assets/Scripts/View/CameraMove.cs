using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public uint GenerationSize;

    public void Start() => cm = new(GenerationSize, new AnneSoGeneratorCorrected());

    public void Update()
    {
        float ms = moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            transform.position += (transform.forward * ms).WithY(0);

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            transform.position -= (transform.forward * ms).WithY(0);

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.position += (transform.right * ms).WithY(0);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.position -= (transform.right * ms).WithY(0);

        if (Input.GetKey(KeyCode.E))
            transform.position += Vector3.up * ms;

        if (Input.GetKey(KeyCode.Q))
            transform.position -= Vector3.up * ms;

        cm.MoveTo(transform.position);

        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
            mouseMove = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseMove = false;
        }

        if (!mouseMove)
            return;

        mouseAngle += new Vector2(
            (mousePos.y - Input.mousePosition.y) * rotationSpeed, (Input.mousePosition.x - mousePos.x) * rotationSpeed);

        transform.eulerAngles = mouseAngle;

        mousePos = Input.mousePosition;
    }

    private bool mouseMove;
    private Vector2 mousePos;
    private Vector2 mouseAngle;
    private ChunkManager cm;
}