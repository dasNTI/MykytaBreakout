using UnityEngine;

public class WalkableArea : MonoBehaviour
{
    [SerializeField] private float closestScale;
    [SerializeField] private float farthestScale;
    [SerializeField] private Connor connor;
    private PolygonCollider2D pc;

    private void Start()
    {
        pc = GetComponent<PolygonCollider2D>();
    }

    private void OnMouseDown()
    {
        if (connor.moving) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float yDif = -pc.bounds.center.y + mousePos.y + pc.bounds.extents.y;
        float factor = yDif / pc.bounds.size.y;
        mousePos.Set(mousePos.x, mousePos.y, connor.gameObject.transform.position.z);
        connor.MoveTo(mousePos, Mathf.Lerp(closestScale, farthestScale, factor));
    }
}
