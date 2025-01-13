using UnityEngine;
public class seed : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    public bool isDrop = false;
    public bool isMergeFlag = false;
    public int seedNo;
    private UImanager uimanager;
    private Vector2 mousePos;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        uimanager = GameObject.Find("UIDocument").GetComponent<UImanager>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && isDrop == false)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.Clamp(mousePos.x, -2.7f, 2.7f);
            mousePos.y = 3.5f;
            transform.position = mousePos;
            Drop();
        }
        if (isDrop) return;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Clamp(mousePos.x, -2.7f, 2.7f);
        mousePos.y = 3.5f;
        transform.position = mousePos;
    }
     private void Drop()
    {
        isDrop = true;
        _rb.simulated = true;
        GameManager.Instance.isNext = true;
        //UIの変更
        uimanager.changeRecordState();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colobj = collision.gameObject;
        if (colobj.CompareTag("seed"))
        {
            seed colseed = collision.gameObject.GetComponent<seed>();
            if (seedNo == colseed.seedNo && 
                !isMergeFlag && 
                !colseed.isMergeFlag) 
            {
                isMergeFlag = true;
                colseed.isMergeFlag = true;
                if(seedNo < GameManager.Instance.MaxSeedNo - 1)
                {
                    GameManager.Instance.MergeNext(transform.position, seedNo);
                }
                else if(seedNo == GameManager.Instance.MaxSeedNo - 1)
                {
                    GameManager.Instance.MergeLargest(transform.position, seedNo);
                }
                Destroy(gameObject);
                Destroy(colseed.gameObject);
            }
        }
    }
}