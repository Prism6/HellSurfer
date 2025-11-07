using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float time;
    [SerializeField] private float currentTime;

    int minute;
    int second;

    // Start is called before the first frame update
    private void Awake()
    {
        time = 180;
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        currentTime = time;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            minute = (int)currentTime / 60;
            second = (int)currentTime % 60;
            text.text = minute.ToString("00") + ":" + second.ToString("00");
            yield return null;

            if (currentTime <= 0)
            {
                Debug.Log("시간 종료");
                currentTime = 0;
                yield break;
            }
        }
    }
}
