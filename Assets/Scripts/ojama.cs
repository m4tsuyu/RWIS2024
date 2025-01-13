using UnityEngine;
public class ojama : MonoBehaviour
{
    private Rigidbody2D _rb;
    public bool isDrop = false;
    public bool isMergeFlag = false;
    private float randomValue;
    public int seedNo;
    private UImanager uimanager;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        uimanager = GameObject.Find("UIDocument").GetComponent<UImanager>();
        // randomValue = Random.Range(-0.5f, 0.5f);
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && isDrop == false)
        {
            Drop();
            randomValue = Random.Range(-0.5f, 0.5f);
        }
        if (isDrop) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Clamp(mousePos.x + randomValue, -2.7f, 2.7f);
        mousePos.y = 4.5f;
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

        if (colobj.CompareTag("ojama"))
        {
            ojama colojama = collision.gameObject.GetComponent<ojama>();
            if (seedNo == colojama.seedNo && 
                !isMergeFlag && 
                !colojama.isMergeFlag) 
            {
                isMergeFlag = true;
                colojama.isMergeFlag = true;
                if (seedNo < GameManager.Instance.MaxOjamaNo - 1)
                {
                    GameManager.Instance.MergeNextOjama(transform.position, seedNo);
                }
                Destroy(gameObject);
                Destroy(colojama.gameObject);
            }
        }
    }
}