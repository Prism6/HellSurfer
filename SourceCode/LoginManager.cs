using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public GameObject LoginPanel, HowToPlayPanel, ChapterSelectPanel, CreditsPanel;
    
    //Start()를 Awake()로 변경
    private void Awake()
    {
        LoginPanel.SetActive(true); //New Game 선택 시  실제 기능하는 메인 메뉴 패널 ("New Game", "Chapter Select", "Exit"버튼 존재) 
        HowToPlayPanel.SetActive(false);
        ChapterSelectPanel.SetActive(false); //챕터 선택 용도로, 처음에는 비활성화 시키고 "Chapter Select" 선택 시에 활성화.
        CreditsPanel.SetActive(false);
    }

    //New Game 버튼 관련 메서드
    public void NewGame_Button()
    {
        SceneManager.LoadScene(1); //새 게임 시작 버튼으로, Level 1부터 시작함.
        Debug.Log("새 게임 시작");
    }

    //How To Play 버튼 관련 메서드
    public void HowToPlay_Button()
    {
        HowToPlayPanel.SetActive(true);
        Debug.Log("게임 조작법 안내 패널 활성화");
    }

    //Chapter Select 버튼 관련 메서드
    public void ChapterSelect_Button() 
    {
        // 챕터 선택 패널 활성화
        ChapterSelectPanel.SetActive(true);
        Debug.Log("챕터 선택 패널 활성화");
    }

    public void Credits_Button()
    {
        CreditsPanel.SetActive(true);
        Debug.Log("크레딧 패널 활성화");
    }

    //누르면 Chapter Select 패널 해제
    public void BackToMenu_Button()
    {
        // HowToPlayPanel이 활성화되어 있으면 비활성화 (2024.02.14. 변경)
        if (HowToPlayPanel.activeSelf)
        {
            HowToPlayPanel.SetActive(false);
            Debug.Log("HowToPlayPanel 비활성화");
        }

        // ChapterSelectPanel이 활성화되어 있으면 비활성화 (2024.02.14. 변경)
        if (ChapterSelectPanel.activeSelf)
        {
            ChapterSelectPanel.SetActive(false);
            Debug.Log("ChapterSelectPanel 비활성화");
        }

        // CreditsPanel이 활성화되어 있으면 비활성화 (2024.02.14. 변경)
        if (CreditsPanel.activeSelf)
        {
            CreditsPanel.SetActive(false);
            Debug.Log("CreditsPanel 비활성화");
        }

        Debug.Log("메인 메뉴로 돌아가기");
    }

    public void LoadChapter(int chapterNumber)
    {
        SceneManager.LoadScene("Level" + chapterNumber);
        Debug.Log("게임 챕터 선택");
    }

    //Exit 버튼 관련
    public void Exit_Button()
    {
        //게임 종료. 빌드 뜨고 나서 기능 동작하는지 확인 필요.
        Application.Quit();
        Debug.Log("게임 종료");
    }
}
