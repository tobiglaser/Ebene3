using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Mover : MonoBehaviour
{

    [SerializeField] private GameObject shape;
    private GameObject grabbedObj = null;
    private Mesh initialMesh;

    // Start is called before the first frame update
    void Start()
    {
        initialMesh = shape.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        //grab
        if (Input.GetMouseButtonDown(0))
        {
            grabbedObj = getClickedGameObject();
            if (grabbedObj != null)
            {
                grabbedObj.transform.Translate(Vector3.up);
                Mesh mesh = grabbedObj.GetComponent<MeshFilter>().mesh;
                shape.GetComponent<MeshFilter>().mesh = mesh;
            }
        }

        //release
        if (Input.GetMouseButtonUp(0) && grabbedObj != null)
        {
            Vector3? newPos = getGroundUnderCursor();
            if (newPos == null)
            {
                // lets go
            }

            grabbedObj.transform.Translate(Vector3.down);
            shape.GetComponent<MeshFilter>().mesh = initialMesh;
            grabbedObj = null;
        }

        //move with cursor
        if (grabbedObj != null)
        {
            Vector3? groundPoint = getGroundUnderCursor();
            if (groundPoint != null)
            {
                shape.transform.position = (Vector3)groundPoint + new Vector3(0, grabbedObj.transform.position.y, 0);
            }
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
        if (!hit.collider.CompareTag("Ground"))
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