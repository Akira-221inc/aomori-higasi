using UnityEngine;

public class DialLamp : MonoBehaviour
{
    public Color normalColor = Color.gray;
    public Color successColor = Color.green;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        SetNormal();
    }

    public void SetNormal()
    {
        rend.material.color = normalColor;
    }

    public void SetSuccess()
    {
        rend.material.color = successColor;
    }
}
