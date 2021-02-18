using UnityEngine;

public class propmanager : MonoBehaviour
{
    [Header("寶箱關閉")]
    public GameObject objClose;
    [Header("寶箱開啟")]
    public GameObject objOpen;
    [Header("面相角度範圍")]
    public float faceRange = 15;
    [Header("補血特效")]
    public GameObject objHp;
    [Header("治癒的值")]
    public float cure = 20;


    private bool playerIn;
    private Transform player;

    private bool Open;
    private void OpenProp()
    {
        if (!Open && playerIn && Input.GetKeyDown(KeyCode.Mouse0) && Vector3.Angle(player.forward, transform.position - player.position) < faceRange)
        {
            Open = true;
            objClose.SetActive(false);
            objOpen.SetActive(true);
            objHp.SetActive(true);
            player.GetComponent<control>().Cure(cure);
        }
    }
    private void Awake()
    {
        player = GameObject.Find("玩家").transform;
    }
    private void Update()
    {
        OpenProp();
    }

    //進入觸發區
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "玩家") playerIn = true;
    }
    //走出觸發區
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "玩家") playerIn = false;
    }
}
