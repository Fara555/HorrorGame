using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public bool isOpen = false;
    [SerializeField] private bool isRotatingDoor = true;
    [SerializeField] private float speed = 1f;
    [Header("Rotation Config")]
    [SerializeField] private float rotationAmount = 90f;
    [SerializeField] private float forwardDirection = 0f;

    private Vector3 _startRotation;
    private Vector3 _forward;

    private Coroutine animationCoroutine;

    public bool destroyable => false;

    private void Awake()
    {
        _startRotation = transform.rotation.eulerAngles;

        _forward = transform.forward;

        
    }

    public void Open(Vector3 userPosition)
    {
        if (!isOpen)
        {
            if(animationCoroutine != null) StopCoroutine(animationCoroutine);

            if (isRotatingDoor)
            {
                float dot = Vector3.Dot(_forward, (userPosition - transform.position).normalized);
                animationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

    private IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotaion;

        if (forwardAmount >= forwardDirection) endRotaion = Quaternion.Euler(new Vector3(0, _startRotation.y + rotationAmount, 0));
        else endRotaion = Quaternion.Euler(new Vector3(0, _startRotation.y - rotationAmount, 0));


        isOpen = true;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotaion, time);
            yield return null;

            time += Time.deltaTime * speed;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);

            if (isRotatingDoor) animationCoroutine = StartCoroutine(DoRotationClose());
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(_startRotation);

        isOpen = false;

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed; 
        }
    }

    public void Interact()
    {
        if (isOpen) Close();
        else Open(IInteractable.playerTransform.position);
    }
}
