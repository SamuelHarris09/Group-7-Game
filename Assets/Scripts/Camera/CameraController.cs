using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform leftBound;
    [SerializeField] Transform rightBound;
    [SerializeField] Transform topBound;
    [SerializeField] Transform bottomBound;

    Vector2 moveVector;

    Camera cam;

    float camHalfWidth;
    float camHalfHeight;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        CameraBound();
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;
    }

    void CameraBound()
    {
        Vector3 newPos = transform.position;

        newPos += (Vector3)moveVector * moveSpeed * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, leftBound.position.x + camHalfWidth, rightBound.position.x - camHalfWidth);

        newPos.y = Mathf.Clamp(newPos.y, bottomBound.position.y + camHalfHeight, topBound.position.y - camHalfHeight);

        transform.position = newPos;
    }

    public void SetMoveInput(Vector2 input)
    {
        moveVector = input.normalized;
    }
}