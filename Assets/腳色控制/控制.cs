using UnityEngine;

public class player : MonoBehaviour
{
    [Header("跳躍速度"), Range(0, 1000)]
    public float jump = 10;
    [Header("移動速度"), Range(0, 1000)]
    public float speed = 10;

    /// <summary>
    /// 是否在地上
    /// </summary>

    public bool Isground;
    public Animation Ani;
    public Rigidbody rig;

    private void Awake()
    {
        Ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        
    }
}
