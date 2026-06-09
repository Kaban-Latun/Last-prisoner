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
    }

    private void Update()
    {
        // 1. Постоянно перемещаем объект партиклов вслед за курсором мыши в мире
        if (mainCamera != null)
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            transform.position = mouseWorldPos;
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartParticles();
        }
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
            particleSys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            isEmitting = false;
        }
    }
}
