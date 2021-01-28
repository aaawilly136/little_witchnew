using UnityEngine;

public class control: MonoBehaviour
{
    [Header("跳躍速度"), Range(0, 1000)]
    public float jump = 10;
    [Header("移動速度"), Range(0, 1000)]
    public float speed = 10;
    [Header("旋轉速度"), Range(0, 1000)]
    public float turn = 10;
    [Header("攝影機角度限制")]
    public Vector2 camlimit = new Vector2(-20, 0);


    /// <summary>
    /// 是否在地上
    /// </summary>

    private bool Isground;
    public Animator Ani;
    public Rigidbody rig;
    private Transform cam;
    private float x;
    private float y;

    private void Awake()
    {
        Ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        cam = GameObject.Find("攝影機根物件").transform;

    }
    private void Update()
    {
        Move();
        TurnCamera();
    }
    /// <summary>
    /// 移動方法
    /// </summary>
    private void Move()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camNew = cam;
        camNew.eulerAngles = new Vector3(0, cam.eulerAngles.y, 0);

        rig.velocity = ((camNew.forward * v + transform.right * h) * speed * Time.deltaTime) + transform.up * rig.velocity.y;

        Ani.SetBool("跑步開", rig.velocity.magnitude > 0);
    }
    private void TurnCamera()
    {
        x += Input.GetAxis("Mouse X") * turn * Time.deltaTime;
        y += Input.GetAxis("Mouse Y") * turn * Time.deltaTime;
        y = Mathf.Clamp(y, camlimit.x, camlimit.y);
       
        cam.localEulerAngles = new Vector3(y, x, 0) ;
    }
}
