using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float heightOffset = 1f;
    [SerializeField] private float forwardOffsetMultiplier = 0.5f; // Множитель для смещения вперед

    private Vector3 prevPlayerPosition;
    private ParticleSystem snowParticleSystem;

    private void Start()
    {
        prevPlayerPosition = player.position;
        snowParticleSystem = GetComponent<ParticleSystem>();

        if (snowParticleSystem != null)
        {
            var mainModule = snowParticleSystem.main;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Вычисляем вектор скорости игрока
            Vector3 playerVelocity = (player.position - prevPlayerPosition) / Time.deltaTime;

            // Направление и величина смещения в зависимости от скорости
            Vector3 forwardOffset = playerVelocity.normalized * forwardOffsetMultiplier * playerVelocity.magnitude;

            // Смещаем систему частиц впереди игрока
            transform.position = player.position + Vector3.up * heightOffset + forwardOffset;

            prevPlayerPosition = player.position;
        }
    }
}
