using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turbo
{
    public int turboGauge = 0;
    public int maxTurboGauge = 3;
    private bool isTurboActivate = false;
    public bool IsTurboActivate => isTurboActivate;

    public float turboSpeedMultiplier = 2.0f;
    private PlayerController playerController;

    private float previousRotation = 0f;
    private float totalRotation = 0f;

    private Image[] turboIcons;

    private Sprite turboOffSprite;
    private Sprite turboOnSprite;

    private ParticleSystem turboEffect;

    public Turbo(PlayerController controller, Image[] uiIcons, Sprite offSprite, Sprite onSprite, ParticleSystem effect)
    {
        this.playerController = controller;
        this.turboIcons = uiIcons;
        this.turboOffSprite = offSprite;
        this.turboOnSprite = onSprite;
        this.turboEffect = effect;

        previousRotation = playerController.transform.rotation.eulerAngles.z;
        // Debug.Log("터보 클래스 생성");
        UpdateTurboUI();
    }

    public void CheckRotationForTurbo()
    {
        if(playerController == null)
        {
            return;
        }

        if (playerController.IsGrounded)
        {
            return;
        }

        float currentRotation = playerController.transform.rotation.eulerAngles.z;
        float rotationDelta = Mathf.DeltaAngle(previousRotation, currentRotation);


        totalRotation += rotationDelta;
        previousRotation = currentRotation;

        if (totalRotation >= 360.0f)
        {
            AddTurboGauge();
            totalRotation -= 360.0f;
            Debug.Log("360도 회전 감지 → 터보 게이지 증가!");
        }

        else if(totalRotation <= -360.0f)
        {
            AddTurboGauge();
            totalRotation += 360.0f;
            Debug.Log("360도 회전 감지 → 터보 게이지 증가!");
        }
        
    }

    public void AddTurboGauge()
    {
        if (turboGauge < maxTurboGauge)
        {
            turboGauge++;
            Debug.Log($"터보 게이지 증가: {turboGauge}");
            UpdateTurboUI();
        }
    }

    public void ActivateTurbo()
    {
        if (!isTurboActivate && turboGauge == maxTurboGauge)
        {
            isTurboActivate = true;
            turboGauge = 0;
            UpdateTurboUI();
            if(turboEffect != null)
            {
                turboEffect.Play();
            }
            playerController.StartCoroutine(TurboMode());
        }
    }

    private IEnumerator TurboMode()
    {
        if(playerController.SurfaceEffector == null)
        {
            Debug.Log("SurfaceEffector2D is null");
        }
        else
        {
            Debug.Log($"surfaceeffector2D 현재 속도: {playerController.SurfaceEffector.speed}");
        }

        Debug.Log("터보 모드 활성화");
        Debug.Log($"BaseSpeed: {playerController.BaseSpeed}");
        Debug.Log($"터보 속도 적용 전: {playerController.SurfaceEffector.speed}");
        playerController.SurfaceEffector.speed = playerController.BaseSpeed * turboSpeedMultiplier;
        Debug.Log($"터보 속도 적용 후: {playerController.SurfaceEffector.speed}");
        yield return new WaitForSeconds(3.0f);
        playerController.SurfaceEffector.speed = playerController.BaseSpeed;
        isTurboActivate = false;
        Debug.Log($"터보 모드 종료! 현재 속도: {playerController.SurfaceEffector.speed}");
    }

    private void UpdateTurboUI()
    {
        for(int i = 0; i < turboIcons.Length; i++)
        {
            if (i < turboGauge)
            {
                turboIcons[i].sprite = turboOnSprite;
            }
            else
            {
                turboIcons[i].sprite = turboOffSprite;
            }
        }
    }
}
