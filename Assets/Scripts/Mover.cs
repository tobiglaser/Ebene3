using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Mover : MonoBehaviour
{

    [SerializeField] private GameObject shape;
    [SerializeField] private Vector3 LiftingOffset = Vector3.up;
    private GameObject grabbedObj = null;
    Vector3 originalPos;
    private Mesh initialMesh;

    // Start is called before the first frame update
    void Start()
    {
        initialMesh = shape.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grabObject();
        }

        if (Input.GetMouseButtonUp(0))
        {
            releaseObject();
        }

        if (grabbedObj != null)
        {
            moveWithCursor();
        }
    }

    private void moveWithCursor()
    {
        Vector3? groundPoint = getGroundUnderCursor();
        if (groundPoint.HasValue)
        {
            shape.transform.position = groundPoint.Value + new Vector3(0, originalPos.y + LiftingOffset.y, 0);
        }
    }

    private void releaseObject()
    {
        Vector3? cursorPos = getGroundUnderCursor();
        if (grabbedObj == null || cursorPos == null)
            return;
        Vector3 releasePos;
        if (isLegal(cursorPos))
        {
            releasePos = new Vector3(cursorPos.Value.x, cursorPos.Value.y + originalPos.y, cursorPos.Value.z);
        }
        else
        {
            releasePos = originalPos;
        }

        grabbedObj.transform.position = releasePos;

        shape.GetComponent<MeshFilter>().mesh = initialMesh;
        grabbedObj = null;
    }

    private bool isLegal(Vector3? position)
    {
        if (position.HasValue)
        {
            Ray ray = new Ray(position.Value + (10 * Vector3.up), Vector3.down);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("DropArea"))
                {
                    return true;
                }
            }
        }
        return false;
    }


    private void grabObject()
    {
        grabbedObj = getClickedGameObject();
        if (grabbedObj != null)
        {
            originalPos = grabbedObj.transform.position;
            grabbedObj.transform.Translate(LiftingOffset);
            Mesh mesh = grabbedObj.GetComponent<MeshFilter>().mesh;
            shape.GetComponent<MeshFilter>().mesh = mesh;
        }
    }


    // returns other GameObjects than tagged "Ground", null otherwise
    GameObject getClickedGameObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.collider == null)
        {
            return null;
        }

        Debug.Log($"Clicked: {hit.collider.tag}");

        if (hit.collider.CompareTag("Vehicle"))
        {
            return hit.transform.gameObject;
        }
        return null;
    }

    // returns the position on the "Ground" where the mouse is pointing, ignores objects between screen and ground.
    Vector3? getGroundUnderCursor()
    {
        Vector3 position = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag("Ground"))
            {
                return hits[i].point;
            }
        }
        return null;
    }
}