using UnityEngine;
public class ojama : MonoBehaviour
{
    private Rigidbody2D _rb;
    private bool isDrop = false;
    private float randomValue;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        randomValue = Random.Range(-0.5f, 0.5f);
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
    }
}