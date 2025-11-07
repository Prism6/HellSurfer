using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // (2025.01.02 추가)
using TMPro;
using Spine.Unity;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.Rendering.PostProcessing;

public class PlayerController : MonoBehaviour
{
    public GameObject target;
    
    [Header("# Player Speed Settings")] //플레이어 스피드 관련 헤더 (2024.10.18. 추가)
    [SerializeField] float baseSpeed = 40f;
    [SerializeField] float boostSpeed = 60f;

    [Header("# Player Jump Settings")]
    [SerializeField] float jumpForce = 30f;
    private bool isGrounded = false;
    public bool IsGrounded => isGrounded; // Getter 추가

    [Header("# Player Rotate")] //플레이어 회전 관련 헤더 (2024.10.18. 추가)
    [SerializeField] float torqueAmount = 1f;

    bool canmove = true;

    [Header("# Player Sound Effect")] //플레이어 사운드 이펙트 관련 헤더 (2024.10.18. 추가)
    public AudioSource jumpSFX; //사운드 추가 (2024.10.18. 추가)
    public AudioSource landingSFX; //사운드 추가 (2024.10.18. 추가)
    public AudioSource deathSFX;//사운드 추가 (2024.10.18. 추가)


    private Rigidbody2D rb2d;


    SurfaceEffector2D surfaceEffector2D;  

    [Header("# Player Fail Settings")]
    public TextMeshProUGUI lifeRemainText;
    public float lifeRemain; // 남은 목숨 (2024.11.05. 추가)
    public float lifeExhaust; // 목숨 고갈 (2024.11.05. 추가)
    public GameObject youDied; // "You Died" 메시지 출력으로 변경 (2024.10.21. 변경)

    [Header("# Score Settings")]
    public TextMeshProUGUI scoreText;
    public float score; //점수
    public float perfectScore; //만점
    [SerializeField] private float scoreIncrement = 1f;

    public SkeletonAnimation _skeletonAnimation;
    public GameObject _Lava;

    short index = 0;

    [Header("# GrayScale")]
    public PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;

    [Header("# Turbo")]
    public ParticleSystem turboEffect;

    [Header("# TurboMode")] //( 2025. 02. 14. 추가)
    public Image[] turboUI;
    public Sprite turboOffSprite;
    public Sprite turboOnSprite;   
    private Turbo turboSystem;
    public float BaseSpeed => baseSpeed; 
    public SurfaceEffector2D SurfaceEffector => surfaceEffector2D; 
    public Rigidbody2D Rigidbody => rb2d;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
        turboSystem = new Turbo(this, turboUI, turboOffSprite, turboOnSprite, turboEffect);
    }

    // Update is called once per frame
    void Update()
    {
        if (canmove)
        {
            RotatePlayer();
            RespondToBoost();
            RespondToJump();
            //TurboActivate();
        }

        if (lifeRemain <= lifeExhaust)
        {
            youDied.SetActive(true);
            DisableControlls();  // 플레이어 움직임 중단
            Debug.Log("You Died 출력");
            Time.timeScale = 0.01f; // 게임 속도를 1%로 감소
            if (postProcessVolume.profile.TryGetSettings(out colorGrading))
            {
                colorGrading.saturation.value = -100;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f; // 게임 속도 다시 빠르게
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }


        if(!isGrounded && index == 0)
        {
            index += 1;
            _skeletonAnimation.AnimationState.SetAnimation(0, "fall", true);
        }
        else if(!isGrounded) _Lava.SetActive(false);

        turboSystem.CheckRotationForTurbo();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            turboSystem.ActivateTurbo();
            //StartCoroutine(TurboSpeedChanger());
        }
    }

    private void RespondToJump()
    {
        // 스페이스바를 누른 상태, 플레이어가 바닥에 있을 때의 조건에서만 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  // 위쪽으로 힘을 가함
            isGrounded = false;  //점프 후에는 공중에 있으므로 false로 설정
            _skeletonAnimation.AnimationState.SetAnimation(0, "jump", true); // 점프 애니메이션 관련 (2025.01.03. 추가)
            jumpSFX.Play();  //점프 사운드 재생
            Debug.Log("Jumped!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿으면 isGrounded를 true로 설정
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            _skeletonAnimation.AnimationState.SetAnimation(0, "landing", true);
            _skeletonAnimation.AnimationState.AddAnimation(0,"run",true,0f);
            _Lava.gameObject.SetActive(true);
            //landingSFX.Play();  // 착지 사운드 재생
        }
    }

    private void OnCollisionExit2D (Collision2D collision)
    {
        // 바닥에 닿으면 isGrounded를 true로 설정
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("떨어짐");
            index = 0;
            isGrounded = false;
        }
    }

    public void DisableControlls()
    {
        canmove = false;
    }

    public void EnableControlls()
    {
        canmove = true;
    }

    private void RespondToBoost()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            surfaceEffector2D.speed = boostSpeed;
            Debug.Log("가속 적용");
        }
        else
        {
            surfaceEffector2D.speed = baseSpeed;
        }
    }

    /*
    private void TurboActivate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            turboEffect.Play();
            Debug.Log("터보발동");
            StartCoroutine(TurboSpeedChanger());
        }
    }

    IEnumerator TurboSpeedChanger()
    {
        baseSpeed = 60;
        boostSpeed = 70;
        yield return new WaitForSeconds(3);
        baseSpeed = 40;
        boostSpeed = 60;
        yield break;
    }
    */

    private void RotatePlayer() // 플레이어 회전과 관계됨
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb2d.AddTorque(torqueAmount);
            //Debug.Log("좌측 화살표 회전");
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb2d.AddTorque(-torqueAmount);
           // Debug.Log("우측 화살표 회전");
        }
    }

    private void CalculateLeftRotation()
    {
        //Rigidbody2D의 현재 회전 각도를 가져오기 (Z축 기준 회전 각도)
        float currentRotation = rb2d.rotation;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("Current Left Rotation Angle: " + currentRotation);
        }
    }

    private void CalculateRightRotation()
    {
        //Rigidbody2D의 현재 회전 각도를 가져오기 (Z축 기준 회전 각도)
        float currentRotation = rb2d.rotation;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("Current Right Rotation Angle: " + currentRotation);
        }
    } 

    public void LifeRemain()
    {
        if (lifeRemain > lifeExhaust)
        {
            lifeRemain--;
            lifeRemainText.text = $"{lifeRemain}";
            Debug.Log("Life Remain 감소");
        }
    }

    public void PlusScore()
    {
        score += scoreIncrement;
        scoreText.text = $"{score}";
    }
}
