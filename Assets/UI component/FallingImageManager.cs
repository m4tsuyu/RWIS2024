using System.Collections;
using UnityEngine;

public class FallingImageManager : MonoBehaviour
{
    public GameObject imagePrefab; // 落下する画像のPrefab
    public RectTransform canvasRect; // CanvasのRectTransform
    public float spawnIntervalMin = 0.5f; // 画像生成の最小間隔
    public float spawnIntervalMax = 2.0f; // 画像生成の最大間隔
    public float fallSpeed = 200f; // 落下速度

    private void Start()
    {
        canvasRect = GetComponent<RectTransform>();
        
        StartCoroutine(SpawnImages());
    }

    private IEnumerator SpawnImages()
    {
        while (true)
        {
            float spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(spawnInterval);

            SpawnImage();
        }
    }

    private void SpawnImage()
    {
        // ランダムなX座標を生成
        float randomX = Random.Range(0, canvasRect.rect.width);

        // 画像を生成
        GameObject newImage = Instantiate(imagePrefab, canvasRect);
        RectTransform imageRect = newImage.GetComponent<RectTransform>();

        // 画像の初期位置を設定
        imageRect.anchoredPosition = new Vector2(randomX, canvasRect.rect.height);

        // 落下処理を開始
        StartCoroutine(FallImage(newImage));
    }

    private IEnumerator FallImage(GameObject image)
    {
        RectTransform imageRect = image.GetComponent<RectTransform>();

        while (imageRect.anchoredPosition.y > -canvasRect.rect.height)
        {
            // 下に移動
            imageRect.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        // 画面外に出たら削除
        Destroy(image);
    }
}
