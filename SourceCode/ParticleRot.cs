using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleRot : MonoBehaviour
{
    public Transform characterTransform; // 캐릭터의 Transform
    private ParticleSystem particleSystem;
    private ParticleSystem.MainModule mainModule;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        mainModule = particleSystem.main;
    }

    void Update()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
        int numParticles = particleSystem.GetParticles(particles);

        for (int i = 0; i < numParticles; i++)
        {
            // 새롭게 생성된 파티클만 캐릭터의 회전값을 가져야 하므로, 초기화된 파티클인지 확인
            if (particles[i].remainingLifetime >= mainModule.startLifetime.constant - Time.deltaTime)
            {
                // 초기 회전값을 캐릭터의 회전값으로 설정 (라디안 변환 필요)
                particles[i].rotation3D = -characterTransform.eulerAngles;
            }
        }

        // 변경된 파티클 배열을 다시 설정
        particleSystem.SetParticles(particles, numParticles);
    }
}
