using UnityEngine.SceneManagement;
using UnityEngine;
public class line : MonoBehaviour
{
    [SerializeField] GameObject gameManager;
    private float stayTime;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("seed")||collision.CompareTag("ojama"))
        {
            stayTime += Time.deltaTime;
            if (stayTime > 4.0f)
            {
                SceneManager.LoadScene("ScoreScene");
                GetScore.score = gameManager.GetComponent<GameManager>().GetScore();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("seed")||collision.CompareTag("ojama"))
        {
            stayTime = 0;
        }
    }
}
