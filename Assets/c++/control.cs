using UnityEngine;

public class control: MonoBehaviour
{
    [Header("跳躍速度"), Range(0, 1000)]
    public float jump = 10;
    [Header("移動速度"), Range(0, 1000)]
    public float speed = 10;
    [Header("攝影旋轉速度"), Range(0, 1000)]
    public float turn = 10;
    [Header("攝影機角度限制")]
    public Vector2 camlimit = new Vector2(-20, 0);
    [Header("角色旋轉速度"), Range(0, 1000)]
    public float turnspeed = 10;
    [Header("檢查地板球體半徑")]
    public float radius = 1f;
    [Header("檢查地板球體位移")]
    public Vector3 offset;
    [Header("檢查地板球體位移")]
    public int jumpCountLimit = 2;


    private int jumpCount;
    /// 是否在地上
    private bool Isground;
    public Animator Ani;
    public Rigidbody rig;
    private Transform cam;
    private float x;
    private float y;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + offset, radius);

    }
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
    private void FixedUpdate()
    {
        Jump();
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

        transform.rotation = Quaternion.Lerp(transform.rotation, camNew.rotation, 0.5f * turnspeed * Time.deltaTime); //角色的角度 = 角色與攝影機的差值

        rig.velocity = ((camNew.forward * v + camNew.right * h) * speed * Time.deltaTime) + transform.up * rig.velocity.y;

        Ani.SetBool("跑步開", rig.velocity.magnitude > 0.1f);
    }
    private void TurnCamera()
    {
        x = Input.GetAxis("Mouse X") * turn * Time.deltaTime;
        y = Input.GetAxis("Mouse Y") * turn * Time.deltaTime;
        y = Mathf.Clamp(y, camlimit.x, camlimit.y);
       
        cam.localEulerAngles = new Vector3(y, x, 0) ;
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < jumpCountLimit)
        {
            jumpCount++;
            rig.Sleep();
            rig.WakeUp();
            rig.AddForce(Vector3.up * jump);
            Ani.SetTrigger("跳躍觸發");
        }
        Collider[] hit = Physics.OverlapSphere(transform.position + offset, radius, 1 << 8);
        if (hit.Length > 0 && hit[0])
        {
            Isground = true;
            jumpCount = 0;
        }
        else Isground = false;

        Ani.SetBool("跳躍是否在地上", Isground);
    }
}
