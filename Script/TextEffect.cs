using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

// ���¼ҽ� ���

public class TextEffect : BaseMeshEffect
{
    Text text;
    public Gradient gradient;
    float time;

    protected override void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        text.FontTextureChanged();
    }
    public override void ModifyMesh(VertexHelper vh)
    {
        List<UIVertex> vertices = new List<UIVertex>();
        vh.GetUIVertexStream(vertices);

        float min = vertices.Min(t => t.position.x);
        float max = vertices.Max(t => t.position.x);

        for (int i = 0; i < vertices.Count; i++) {
            var v = vertices[i];
            float cur = Mathf.InverseLerp(min, max, v.position.x);
            cur = Mathf.PingPong(cur + time, 1f);
            Color c = gradient.Evaluate(cur);
            v.color = new Color(c.r, c.g, c.b, 1);
            vertices[i] = v;
        }

        vh.Clear(); // ���� �� �ٽ� ���
        vh.AddUIVertexTriangleStream(vertices);
    }
}
