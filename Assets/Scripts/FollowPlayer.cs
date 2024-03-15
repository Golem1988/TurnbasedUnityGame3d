using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject target;
    public GameObject follower;

    public float speed;
    private Vector3 vector3 = new Vector3(1f, 0f, 1f);

    void Update()
    {
        follower.transform.position = Vector3.MoveTowards(follower.transform.position, target.transform.position, speed);
    }
}
