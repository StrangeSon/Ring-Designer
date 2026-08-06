using System;
using System.Collections;
using sc.modeling.splines.runtime;
using UnityEngine;
using UnityEngine.Splines;

namespace RingDesigner
{
    public class CircleSplineCreator : MonoBehaviour
    {
        public SplineMesher SplineMesher;

        public float radius = 5f;
        public int segments = 8;
        [Range(0f, 360f)]
        public float arcAngle = 360f;
        [Range(0f, 360f)]
        public float startAngle = 0f;

        public void OnValidate()
        {
            Create();
        }

        [ContextMenu("Create Circle")]
        public void Create()
        {
            if (!this.TryGetComponent(out SplineContainer container))
                container = this.gameObject.AddComponent<SplineContainer>();

            // Clear existing splines
            foreach (Spline existingSpline in container.Splines)
                container.RemoveSpline(existingSpline);

            var spline = new Spline();

            Vector3 center = Vector3.zero;
            float startRad = startAngle * Mathf.Deg2Rad;
            float arcRad = arcAngle * Mathf.Deg2Rad;
            float angleStep = arcRad / Mathf.Max(segments - 1, 1);

            // Calculate knot positions and tangents
            for (int i = 0; i < segments; i++)
            {
                float angle = startRad + i * angleStep;
                Vector3 position = new Vector3(
                    center.x + radius * Mathf.Cos(angle),
                    center.y,
                    center.z + radius * Mathf.Sin(angle)
                );

                // Calculate the tangent direction (perpendicular to the radius)
                // For a circle, the tangent at a point is perpendicular to the radius vector
                Vector3 radiusVector = (position - center).normalized;
                Vector3 tangent = new Vector3(-radiusVector.z, 0, radiusVector.x); // Rotate 90 degrees in XZ plane
                float tangentLength = (4f / 3f) * radius * Mathf.Tan(angleStep / 4f);
                tangent = tangent.normalized * tangentLength;

                // Add knot with manually calculated tangents
                spline.Add(new BezierKnot(position, -tangent, tangent));
            }

            container.AddSpline(spline);
        }

        public void Update3DText(Mesh mesh)
        {
            SplineMesher.sourceMesh = mesh;
            SplineMesher.Rebuild();
        }

    }
}
