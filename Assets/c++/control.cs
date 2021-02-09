using UnityEngine;
using UnityEngine.UI;

public class control: MonoBehaviour
{
    [Header("跳躍速度"), Range(0, 1000)]
    public float jump = 10;
    [Header("跑步移動速度"), Range(0, 1000)]
    public float speed = 10;
    [Header("走路移動速度"), Range(0, 1000)]
    public float speedwalk = 10;
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
    public int jumpCountLimit = 1;
    
    //血條/魔力/體力
    [Header("血量"), Range(0, 5000)]
    public float hp = 100;
    private float hpMax;
    [Header("魔力"), Range(0, 5000)]
    public float mp = 500;
    private float mpMax;
    [Header("體力"), Range(0, 5000)]
    public float ps = 200;
    private float psMax;

    [Header("吧條")]
    public Image barHp;
    public Image barMp;
    public Image barPs;

    [Header("移動時每秒扣除體力"), Range(0, 5000)]
    public float psMove = 15;
    [Header("跳躍時扣除體力"), Range(0, 5000)]
    public float psJump = 25;
    [Header("休息時每秒恢復體力"), Range(0, 5000)]
    public float psRecover = 100;



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
        hpMax = hp;
        mpMax = mp;
        psMax = ps;

    }
    //呼叫系統
    private void Update()
    {
        Move();
        TurnCamera();
        Jump();
        PSSystem();
    }
    private void FixedUpdate() //50fps
    {
        
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

        if (ps>0)
        {
            rig.velocity = ((camNew.forward * v + camNew.right * h) * speed * Time.deltaTime) + transform.up * rig.velocity.y;

            Ani.SetBool("跑步開", rig.velocity.magnitude > 0.1f);
        }
        else
        {
            rig.velocity = ((camNew.forward * v + camNew.right * h) * speedwalk * Time.deltaTime) + transform.up * rig.velocity.y;

            Ani.SetBool("走路開關", rig.velocity.magnitude > 0.1f);
            Ani.SetBool("跑步開", false);
        }
        
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
        Collider[] hit = Physics.OverlapSphere(transform.position + offset, radius, 1 << 8);
        if (hit.Length > 0 && hit[0])
        {
            Isground = true;
            jumpCount = 0;
        }
        else Isground = false;

        Ani.SetBool("跳躍是否在地上", Isground);

        if (ps < psJump) return;
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < jumpCountLimit - 1)
        {
            jumpCount++;
            rig.Sleep();
            rig.WakeUp();
            rig.AddForce(Vector3.up * jump);
            Ani.SetTrigger("跳躍觸發");
            

            ps -= psJump;
            barPs.fillAmount = ps / psMax;

        }
        
    }
    //體力系統
    private void PSSystem()
    {
        if (Ani.GetBool("跑步開"))
        {
            ps -= psMove * Time.deltaTime;
            barPs.fillAmount = ps / psMax;
        }
        else if (!Ani.GetBool("走路開關"))
        {
            ps += psRecover * Time.deltaTime;
            barPs.fillAmount = ps / psMax;
        }
        ps = Mathf.Clamp(ps, 0, psMax);
    }
}
