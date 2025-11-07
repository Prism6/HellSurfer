using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadySetGoTimer : MonoBehaviour
{
    /*
    [전체 로직 설명]
    1. 게임이 시작되면 Stage 진행상황과 타이머가 표시됩니다. 이 타이머가 바로 "ReadySetGoTimer"입니다.
    2. 타이머가 활성화된 상태에서는 게임은 멈춰있지만 타이머는 정상 가동합니다.
    3. 타이머의 초기값은 3초로 시간이 지날수록 3,2,1의 순서대로 점차 감소하며 이후에는 "GO!"라는 글자가 출력됩니다.
    4. "GO!" 라는 글자가 출력된 후 사라지며, 타이머는 비활성화 됩니다.
    */

    public GameObject ReadySetGoPanel;

    [Header("# ReadySetGoTimer Settings")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private float readyTime;
    [SerializeField] private float currentTime;

    int second;
    private bool isPaused = false;

    // Start is called before the first frame update
    private void Awake()
    {
        ReadySetGo();
        readyTime = 3;
        StartCoroutine(ReadyStartTimer());
    }

    public void ReadySetGo()
    {
        ReadySetGoPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    IEnumerator ReadyStartTimer()
    {
        currentTime = readyTime;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            second = (int)currentTime % 60;
            text.text = second.ToString("0");
            yield return null;

            if (currentTime <= 0)
            {
                Debug.Log("ReadySetGo!");
                currentTime = 0;
                yield break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
