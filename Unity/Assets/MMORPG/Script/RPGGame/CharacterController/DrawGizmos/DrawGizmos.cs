using UnityEngine;
using UnityEngine.Serialization;
public class DrawGizmos : MonoBehaviour
{
    public float skinWidth = 0.08f;

    [FormerlySerializedAs("m_Center")]
    public Vector3 center;

    [FormerlySerializedAs("m_Radius")]
    public float radius = 0.5f;

    [FormerlySerializedAs("m_Height")]
    public float height = 2.0f;

    // 胶囊的默认高度
    public float defaultHeight { get; private set; }

    // 应用相关缩放比例的胶囊半径（例如，如果对象比例不是1,1,1）
    public float scaledRadius
    {
        get
        {
            Vector3 scale = transform.lossyScale;
            float maxScale = Mathf.Max(Mathf.Max(scale.x, scale.y), scale.z);
            return radius * maxScale;
        }
    }

    // 应用缩放和旋转的胶囊中心。
    Vector3 transformedCenter { get { return transform.TransformVector(center); } }

    // 应用相关缩放的胶囊高度(例如，如果对象缩放不是1,1,1)
    float scaledHeight { get { return height * transform.lossyScale.y; } }

#if UNITY_EDITOR
    // Draws the debug Gizmos
    void OnDrawGizmosSelected()
    {
        // Foot position
        Gizmos.color = Color.cyan;
        Vector3 footPosition = GetFootWorldPosition(transform.position);
        Gizmos.DrawLine(footPosition + Vector3.left * scaledRadius,
                        footPosition + Vector3.right * scaledRadius);
        Gizmos.DrawLine(footPosition + Vector3.back * scaledRadius,
                        footPosition + Vector3.forward * scaledRadius);

        // Top of head
        Vector3 headPosition = transform.position + transformedCenter + Vector3.up * (scaledHeight / 2.0f + skinWidth);
        Gizmos.DrawLine(headPosition + Vector3.left * scaledRadius,
                        headPosition + Vector3.right * scaledRadius);
        Gizmos.DrawLine(headPosition + Vector3.back * scaledRadius,
                        headPosition + Vector3.forward * scaledRadius);

        // Center position
        Vector3 centerPosition = transform.position + transformedCenter;
        Gizmos.DrawLine(centerPosition + Vector3.left * scaledRadius,
                        centerPosition + Vector3.right * scaledRadius);
        Gizmos.DrawLine(centerPosition + Vector3.back * scaledRadius,
                        centerPosition + Vector3.forward * scaledRadius);
    }
#endif

    // 获得足部 world position.
    Vector3 GetFootWorldPosition(Vector3 position)
    {
        return position + transformedCenter + (Vector3.down * (scaledHeight / 2.0f + skinWidth));
    }

}