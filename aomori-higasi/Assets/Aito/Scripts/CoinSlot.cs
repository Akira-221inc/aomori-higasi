using UnityEngine;

public class CoinSlot : MonoBehaviour
{
    private GachaController gacha;

    void Start()
    {
        gacha = FindObjectOfType<GachaController>();
    }

    void OnMouseDown()
    {
        gacha.InsertCoin();
    }
}
