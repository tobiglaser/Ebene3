using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Mover : MonoBehaviour
{
    [SerializeField] private GameManager Manager;
    [SerializeField] private GameObject HollowTrain;
    [SerializeField] private GameObject HollowCar;
    [SerializeField] private GameObject HollowCanoe;
    [SerializeField] private Vector3 LiftingOffset = Vector3.up;
    private GameObject shape;
    private GameObject grabbedObj = null;
    Vector3 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        Manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Manager.CurrentState == GameManager.GameStates.Playing)
        {
            moverFunctions();
        }
    }

    private void moverFunctions()
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
        if (grabbedObj == null)
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

        Destroy(shape);
        callReleaseFunction(grabbedObj);
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
            callLiftFunction(grabbedObj);
            shape = Instantiate(getHollowObject(grabbedObj), grabbedObj.transform.position, grabbedObj.transform.rotation);
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



    private void callLiftFunction(GameObject obj)
    {
        Train t = obj.GetComponent<Train>();
        if (t)
        {
            t.OnLift();
            return;
        }
        Car ca = obj.GetComponent<Car>();
        if (ca)
        {
            ca.OnLift();
            return;
        }
        Canoe cu = obj.GetComponent<Canoe>();
        if (cu)
        {
            cu.OnLift();
            return;
        }
        return;
    }

    private void callReleaseFunction(GameObject obj)
    {
        Train t = obj.GetComponent<Train>();
        if (t)
        {
            t.OnRelease();
            return;
        }
        Car ca = obj.GetComponent<Car>();
        if (ca)
        {
            ca.OnRelease();
        }
        Canoe cu = obj.GetComponent<Canoe>();
        if (cu)
        {
            cu.OnRelease();
        }
        return;
    }

    private string getVehicleName(GameObject obj)
    {
        Train t = obj.GetComponent<Train>();
        if (t)
            return "Train";
        Car ca = obj.GetComponent<Car>();
        if (ca)
            return "Car";

        Canoe cu = obj.GetComponent<Canoe>();
        if (cu)
            return "Canoe";
        return "";
    }

    private GameObject getHollowObject(GameObject obj)
    {
        string name = getVehicleName(obj);
        switch (name)
        {
            case "Train":
                {
                    return HollowTrain;
                }
            case "Car":
                {
                    return HollowCar;
                }
            case "Canoe":
                {
                    return HollowCanoe;
                }
            default:
                {
                    return null;
                }
        }
    }
}