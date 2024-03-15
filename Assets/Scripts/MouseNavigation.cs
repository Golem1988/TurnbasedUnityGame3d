using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MouseNavigation : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform.GetComponent<Camera>();
    }
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}
