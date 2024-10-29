using UnityEngine;

public class Battery : MonoBehaviour, IInteractable
{
    [SerializeField] private Flashlight flashlight;

    public bool destroyable => true;

    private void Awake()
    {
        flashlight = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<Flashlight>();
    }
    public void Interact()
    {
        flashlight.batteriesAmount++;
    }
}