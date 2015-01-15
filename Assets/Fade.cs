using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour
{
    public int materials;
    private bool startFading = false;
    private float alpha = 0;

    // Use this for initialization
    void Start()
    {
        //Blagen finden
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<MeshRenderer>() != null)
            {
                foreach (Material m in child.renderer.materials)
                {
                    m.shader = Shader.Find("Transparent/Specular VertexLit with Z");
                    float r, g, b;
                    r = m.color.r;
                    g = m.color.g;
                    b = m.color.b;
                    m.color = new Color(r, g, b, 0);
                }
            }
        }
    }

    void Update()
    {
        //Blagen finden
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<MeshRenderer>() != null)
            {
                foreach (Material m in child.renderer.materials)
                {
                    float r, g, b;
                    r = m.color.r;
                    g = m.color.g;
                    b = m.color.b;
                    alpha += 0.01f;
                    m.color = new Color(r, g, b, alpha);
                }
            }
        }
    }
}
