using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScroller : MonoBehaviour
{

    public float moveLerp = 10;
    public Transform camLimiter;
    public Transform camMoveBox;
    Camera cam;
    Vector2 vpSize;
    Vector2 cornerMax;
    Vector2 cornerMin;
    Vector3 tarPos;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        UpdateCameraScale();
        tarPos = transform.position;
        camMoveBox.gameObject.SetActive(false);
    }

    void UpdateCameraScale()
    {
        var trCorner = cam.ViewportToWorldPoint(new Vector2(1, 1));
        var blCorner = cam.ViewportToWorldPoint(new Vector2(0, 0));
        vpSize = new Vector2(trCorner.x - blCorner.x, trCorner.y - blCorner.y);
        cornerMax.y = camLimiter.position.y + camLimiter.localScale.y * 0.5f - vpSize.y * 0.5f;
        cornerMin.y = camLimiter.position.y - camLimiter.localScale.y * 0.5f + vpSize.y * 0.5f;
        cornerMax.x = camLimiter.position.x + camLimiter.localScale.x * 0.5f - vpSize.x * 0.5f;
        cornerMin.x = camLimiter.position.x - camLimiter.localScale.x * 0.5f + vpSize.x * 0.5f;
    }

    void Update()
    {
        var m = MouseData2D.Inst.mouseWorldPos;
        var boxCornerMax = new Vector2(camMoveBox.position.x + camMoveBox.localScale.x * 0.5f, camMoveBox.position.y + camMoveBox.localScale.y * 0.5f);
        var boxCornerMin = new Vector2(camMoveBox.position.x - camMoveBox.localScale.x * 0.5f, camMoveBox.position.y - camMoveBox.localScale.y * 0.5f);

        if (m.x > boxCornerMax.x) tarPos.x = transform.position.x + m.x - boxCornerMax.x;
        if (m.y > boxCornerMax.y) tarPos.y = transform.position.y + m.y - boxCornerMax.y;
        if (m.x < boxCornerMin.x) tarPos.x = transform.position.x + (m.x - boxCornerMin.x);
        if (m.y < boxCornerMin.y) tarPos.y = transform.position.y + (m.y - boxCornerMin.y);

        if (tarPos.x > cornerMax.x) tarPos.x = cornerMax.x;
        if (tarPos.y > cornerMax.y) tarPos.y = cornerMax.y;
        if (tarPos.x < cornerMin.x) tarPos.x = cornerMin.x;
        if (tarPos.y < cornerMin.y) tarPos.y = cornerMin.y;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, tarPos, moveLerp * Time.fixedDeltaTime);
    }

    
}
