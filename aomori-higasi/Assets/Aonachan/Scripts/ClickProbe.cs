using UnityEngine;

public class ClickProbe : MonoBehaviour {
    void OnMouseDown() { Debug.Log("OnMouseDown hit: " + name, this); }
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var cam = Camera.main;
            if (!cam) { Debug.LogWarning("No Main Camera (tag)"); return; }
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 1000f))
                Debug.Log("Raycast hit: " + hit.collider.name);
            else
                Debug.Log("Raycast miss");
        }
    }
}
