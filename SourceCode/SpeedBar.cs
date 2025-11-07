using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    /*
    [전체 로직 설명]
    1. Player의 Speed를 Slider를 통해 시각화.
    2. Player의 속도가 증가하면 Slider의 값이 증가하여 우측으로 이동.
    3. Player의 속도가 감소하면 Slider의 값이 감소하여 좌측으로 이동.
    4. Slider의 최소/최대 값을 설정하여 적절한 범위 내에서 속도 시각화를 제한.
    */

    [Header("Speed Slider Settings")] // 헤더 추가 및 구성요소 선언 (2025.01.06 추가)
    [SerializeField] private Slider speedSlider; // 스피드 시각화를 위한 Slider 컴포넌트
    [SerializeField] private float maxSpeed = 100f; // Player의 최대 속도
    [SerializeField] private float minSpeed = 0f;   // Player의 최소 속도
    [SerializeField] private float smoothSpeed = 0.1f; // Slider 움직임의 부드러운 변화 속도   
    [SerializeField] private float targetProgress;

    private float currentSpeed;

    private void Awake()
    {
        if (speedSlider == null)
        {
            Debug.LogError("SpeedSlider가 설정되지 않았습니다!");
            return;
        }

        speedSlider.minValue = minSpeed; // Slider의 최소값 설정
        speedSlider.maxValue = maxSpeed; // Slider의 최대값 설정
        speedSlider.value = minSpeed;   // Slider 초기값 설정
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = 0f; // Player의 초기 속도를 0으로 설정
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateSpeedBar()
    {
        float newSliderValue = Mathf.Lerp(speedSlider.value, currentSpeed, smoothSpeed * Time.deltaTime);
        speedSlider.value = newSliderValue;
    }

    // Add Progress to the bar
    private void IncrementProgress(float newProgress)
    {
        speedSlider.value += newProgress;
    }
}
