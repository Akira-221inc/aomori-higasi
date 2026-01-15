using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Fuel")]
    public float fuelAmount = 0f;
    public float maxFuel = 100f;

    [Header("UI")]
    [Tooltip("Assign a GameObject (panel/text) that will be enabled when the game is cleared.")]
    public GameObject gameClearUI;

    [Tooltip("Optional UI Slider used to show fuel (normalized 0..1)")]
    public Slider fuelSlider;

    [Tooltip("Optional TextMeshProUGUI to show fuel percent (e.g. '42%')")]
    public TextMeshProUGUI fuelText;

    void Start()
    {
        // Initialize UI to current values
        UpdateFuelUI();

        // If slider is used and its range is not normalized, ensure a consistent convention
        if (fuelSlider != null)
        {
            // Use 0..1 normalized slider for simplicity
            fuelSlider.minValue = 0f;
            fuelSlider.maxValue = 1f;
            fuelSlider.wholeNumbers = false;
        }
    }

    public void AddFuel(float amount)
    {
        if (fuelAmount >= maxFuel)
            return;

        fuelAmount += amount;
        fuelAmount = Mathf.Clamp(fuelAmount, 0, maxFuel);

        Debug.Log("Fuel: " + fuelAmount);

        UpdateFuelUI();

        if (fuelAmount >= maxFuel)
            GameClear();
    }

    void UpdateFuelUI()
    {
        if (fuelSlider != null)
        {
            float normalized = (maxFuel > 0f) ? (fuelAmount / maxFuel) : 0f;
            fuelSlider.value = Mathf.Clamp01(normalized);
        }

        if (fuelText != null)
        {
            int percent = (maxFuel > 0f) ? Mathf.RoundToInt((fuelAmount / maxFuel) * 100f) : 0;
            fuelText.text = percent + "%";
        }
    }

    public void GameClear()
    {
        Debug.Log("GAME CLEAR!");
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }
    }
}
