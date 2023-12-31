using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public uint GenerationSize;

    public GameObject Panel;

    public GameObject Button;

    public void Start()
    {
        int pos = -30;

        pos += 145; // Je ne sais pas pourquoi il faut faire cette correction mais elle marche

        foreach (TypeInfo t in typeof(CameraMove).Assembly.DefinedTypes)
        {
            if (t.ImplementedInterfaces.Contains(typeof(IGenerator)))
            {
                GameObject p = Instantiate(Button, Panel.transform);
                p.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = t.Name;
                p.transform.localPosition = new(0, pos, 0);

                p.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Destroy(Panel);
                    cm = new ChunkManager(GenerationSize, (IGenerator)Activator.CreateInstance(t.AsType()));
                });

                pos -= 30 + 10;
            }
        }

        pos -= 145; // Inversion de la correction précédente

        Panel.GetComponent<RectTransform>().sizeDelta = new(400, -pos);
    }

    public void Update()
    {
        float ms = moveSpeed * Time.deltaTime;

        #region input

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

        #endregion input

        cm?.MoveTo(transform.position + (transform.forward * (10 * GenerationSize)));

        #region camera_angle

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            mousePos = Input.mousePosition;
            mouseMove = true;
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            mouseMove = false;
        }

        if (!mouseMove)
            return;

        mouseAngle += new Vector2(
            (mousePos.y - Input.mousePosition.y) * rotationSpeed, (Input.mousePosition.x - mousePos.x) * rotationSpeed);

        transform.eulerAngles = mouseAngle;

        mousePos = Input.mousePosition;

        #endregion camera_angle
    }

    private bool mouseMove;
    private Vector2 mousePos;
    private Vector2 mouseAngle;
    private ChunkManager cm;
}