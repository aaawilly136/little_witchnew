
using UnityEngine;

public class cameracontrol : MonoBehaviour
{
  
    [Header("追蹤速度"), Range(0, 300)]
    public float speed = 10;

    private Transform target;

    private void Awake()
    {
        target = GameObject.Find("玩家").transform;
    }
    private void LateUpdate()
    {
        Track();
    }
    private void Track()
    {
        Vector3 posTarget = target.position;
        Vector3 posCamera = target.position;

        posCamera = Vector3.MoveTowards(posCamera, posTarget, speed * Time.deltaTime);
    }  
}
