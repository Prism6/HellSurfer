using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBar : MonoBehaviour
{
    /*
    [전체 로직 설명]
    1. Player와 FinishLine 사이의 거리를 백분위로 환산하여 Slider(UI)에 표시합니다.
    2. Player가 FinishLine에 가까워질수록 Slider 값이 증가하여 우측으로 이동합니다.
    3. Player가 FinishLine에서 멀어지면 Slider 값이 감소하여 좌측으로 이동합니다.
    4. Player가 Respawn되면 ProgressBar 값이 초기화(minProgress)되고 다시 계산됩니다.
    5. PauseMenu.cs 스크립트의 PauseMenu가 활성화되면 LevelProgressBar 계산이 일시적으로 멈춥니다.
    */

    [Header("Level Progress Slider Settings")]
    [SerializeField] private Slider levelProgressSlider;
    [SerializeField] private float maxProgress = 100f;
    [SerializeField] private float minProgress = 0f;
    [SerializeField] private float goalProgress;

    [Header("Level Elements")]
    [SerializeField] private Transform spawnPoint; // 스폰 위치, RespawnPoint와 동일
    [SerializeField] private Transform finishLine; // 결승선 위치
    [SerializeField] private Transform player;     // 플레이어 위치

    [Header("PauseMenu Reference")]
    [SerializeField] private PauseMenu pauseMenu;  // PauseMenu 참조

    private float totalDistance;
    private float currentProgress;

    private void Awake()
    {
        if (levelProgressSlider == null)
        {
            Debug.LogError("levelProgressSlider가 설정되지 않았습니다!");
            return;
        }

        levelProgressSlider.minValue = minProgress; // Slider의 최소값 설정
        levelProgressSlider.maxValue = maxProgress; // Slider의 최대값 설정
        levelProgressSlider.value = minProgress;   // Slider 초기값 설정
    }

    // Start is called before the first frame update
    void Start()
    {
        totalDistance = Vector3.Distance(spawnPoint.position, finishLine.position);
        currentProgress = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // PauseMenu 활성화 시 진행 계산 멈춤
        //if (pauseMenu != null && pauseMenu.IsPaused())
        //{
        //    return;
        //}

        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        float remainingDistance = Vector3.Distance(player.position, finishLine.position);
        currentProgress = Mathf.Clamp((1 - (remainingDistance / totalDistance)) * maxProgress, minProgress, maxProgress);
        levelProgressSlider.value = currentProgress;
    }

    public void ResetProgress()
    {
        currentProgress = minProgress;
        levelProgressSlider.value = minProgress;
    }
}
