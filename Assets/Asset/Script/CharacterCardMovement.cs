using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardMovement : MonoBehaviour
{
    public Image imageToMove;
    public Image targetImage;
    public float moveSpeed = 200.0f; // Tốc độ di chuyển

    private RectTransform imageRectTransform;
    private RectTransform targetRectTransform;
    private Vector2 initialPosition; // Vị trí ban đầu của Image

    private void Start()
    {
        imageRectTransform = imageToMove.GetComponent<RectTransform>();
        targetRectTransform = targetImage.GetComponent<RectTransform>();
        initialPosition = imageRectTransform.anchoredPosition; // Lưu lại vị trí ban đầu
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveImageToTarget(); // Gọi hàm di chuyển khi nhấn nút Space
        }
    }

    public void MoveImageToTarget()
    {
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        Vector2 startPosition = imageRectTransform.anchoredPosition;
        Vector2 targetPosition = targetRectTransform.anchoredPosition;
        float distance = Vector2.Distance(startPosition, targetPosition);
        float moveTime = distance / moveSpeed; // Thời gian di chuyển dựa trên tốc độ

        float startTime = Time.time;

        while (Time.time - startTime < moveTime)
        {
            float t = (Time.time - startTime) / moveTime;
            imageRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // Di chuyển trở lại vị trí ban đầu
        startTime = Time.time;
        while (Time.time - startTime < moveTime)
        {
            float t = (Time.time - startTime) / moveTime;
            imageRectTransform.anchoredPosition = Vector2.Lerp(targetPosition, initialPosition, t);
            yield return null;
        }

        imageRectTransform.anchoredPosition = initialPosition; // Đảm bảo vị trí cuối cùng đúng chính xác
    }
}
