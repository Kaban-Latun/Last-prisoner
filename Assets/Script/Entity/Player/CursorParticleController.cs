using UnityEngine;

public class CursorParticleController : MonoBehaviour
{
    private Camera mainCamera;
    private ParticleSystem particleSys;
    private bool isEmitting = false;

    private void Start()
    {
        mainCamera = Camera.main;
        particleSys = GetComponent<ParticleSystem>();

        // На всякий случай гарантируем, что при старте эффект выключен
        if (particleSys != null)
        {
            particleSys.Stop();
        }
    }

    private void Update()
    {
        // 1. Постоянно перемещаем объект партиклов вслед за курсором мыши в мире
        if (mainCamera != null)
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // Устанавливаем Z в 0 (или調整 под глубину вашего 2D-мира), чтобы частицы не улетали за задний фон
            mouseWorldPos.z = 0f;
            transform.position = mouseWorldPos;
        }

        // 2. Нажатие левой кнопки мыши — запускаем проигрывание
        if (Input.GetMouseButtonDown(0))
        {
            StartParticles();
        }

        // 3. Отпускание левой кнопки мыши — останавливаем появление новых частиц
        if (Input.GetMouseButtonUp(0))
        {
            StopParticles();
        }
    }

    private void StartParticles()
    {
        if (particleSys != null && !isEmitting)
        {
            particleSys.Play();
            isEmitting = true;
        }
    }

    private void StopParticles()
    {
        if (particleSys != null && isEmitting)
        {
            // Метод Stop() с параметром true заставит старые частицы плавно дожить свой век,
            // но новые вылетать уже не будут, что обеспечит мягкое затухание эффекта
            particleSys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            isEmitting = false;
        }
    }
}
