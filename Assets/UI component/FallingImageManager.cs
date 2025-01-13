using System.Collections;
using UnityEngine;

public class FallingImageManager : MonoBehaviour
{
    public GameObject imagePrefab; // ��������摜��Prefab
    public RectTransform canvasRect; // Canvas��RectTransform
    public float spawnIntervalMin = 0.5f; // �摜�����̍ŏ��Ԋu
    public float spawnIntervalMax = 2.0f; // �摜�����̍ő�Ԋu
    public float fallSpeed = 200f; // �������x

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
        // �����_����X���W�𐶐�
        float randomX = Random.Range(0, canvasRect.rect.width);

        // �摜�𐶐�
        GameObject newImage = Instantiate(imagePrefab, canvasRect);
        RectTransform imageRect = newImage.GetComponent<RectTransform>();

        // �摜�̏����ʒu��ݒ�
        imageRect.anchoredPosition = new Vector2(randomX, canvasRect.rect.height);

        // �����������J�n
        StartCoroutine(FallImage(newImage));
    }

    private IEnumerator FallImage(GameObject image)
    {
        RectTransform imageRect = image.GetComponent<RectTransform>();

        while (imageRect.anchoredPosition.y > -canvasRect.rect.height)
        {
            // ���Ɉړ�
            imageRect.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        // ��ʊO�ɏo����폜
        Destroy(image);
    }
}
