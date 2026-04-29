using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;//objetivo a seguir
    public float min_X, max_X, min_Y, max_Y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x,min_X,max_X), Mathf.Clamp(target.position.y, min_Y, max_Y), -10);
    }
}
