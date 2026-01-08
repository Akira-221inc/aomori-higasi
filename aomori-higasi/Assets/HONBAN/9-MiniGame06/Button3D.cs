using UnityEngine;

public class Button3D : MonoBehaviour
{
    public enum ButtonType
    {
        Left,
        Right,
        Decide
    }

    public ButtonType type;
    public MiniGame06Manager manager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int mask = LayerMask.GetMask("Button");

            if (Physics.Raycast(ray, out hit, 100f, mask))
            {
                Debug.Log("Button命中: " + hit.collider.name);
                hit.collider.GetComponent<Button3D>()?.OnPressed();
            }
        }
    }

    public void OnPressed()
    {
        if (manager == null)
        {
            Debug.LogError("Manager未設定");
            return;
        }

        Debug.Log("押された: " + type);

        switch (type)
        {
            case ButtonType.Left:
                manager.Prev();
                break;
            case ButtonType.Right:
                manager.Next();
                break;
            case ButtonType.Decide:
                manager.Decide();
                break;
        }
    }
}
