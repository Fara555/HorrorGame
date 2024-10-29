using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text batteryText;
    [SerializeField] private Image[] batteriesCell;

    [Header("FlashLight")]
    [SerializeField] private float lifetime = 100f;
    [SerializeField] private float tickAmount = 1;
    [SerializeField] private AudioSource lightOnOff;
    [SerializeField] private AudioSource reloadLight;
    public float batteriesAmount = 0;


    [Header("Keybinds")]
    [SerializeField] private KeyCode flashlightKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode reloadFlashlightKey = KeyCode.R;

    private Light _light;

    private void Start()
    {
        _light = GetComponent<Light>();
        lightOnOff = GetComponent<AudioSource>();
        _light.enabled = false;
        UpdateBatteryCells();
    }

    private void Update()
    {
        batteryText.text = batteriesAmount.ToString();

        FlashLight();

        if (Input.GetKeyDown(reloadFlashlightKey) && batteriesAmount > 0)
        {
            ReloadFlashLight();
        }
    }

    private void FlashLight()
    {
        if (Input.GetKeyDown(flashlightKey))
        {
            lightOnOff.Play();
            _light.enabled = !_light.enabled;
        }

        if (_light.isActiveAndEnabled)
        {
            lifetime -= tickAmount * Time.deltaTime;
            UpdateBatteryCells();
        }

        if (lifetime <= 0)
        {
            _light.enabled = false;
            lifetime = 0;
            UpdateBatteryCells();
        }
    }

    private void ReloadFlashLight()
    {
        reloadLight.Play();
        batteriesAmount -= 1;
        lifetime = 100;
        UpdateBatteryCells();
    }

    private void UpdateBatteryCells()
    {
        // Предполагаем, что у нас 3 ячейки, поэтому каждая ячейка представляет примерно 33.3% заряда
        float chargePerCell = 100f / batteriesCell.Length;
        int activeCells = Mathf.CeilToInt(lifetime / chargePerCell);

        for (int i = 0; i < batteriesCell.Length; i++)
        {
            batteriesCell[i].enabled = i < activeCells;
        }
    }
}
