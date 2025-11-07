using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public PlayerController playercontrollerScript;
    public AudioSource collectCoinSFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            collectCoinSFX.Play();// 소울(코인)수집 음원
            Debug.Log("콜라이더 충돌 감지");
            playercontrollerScript.PlusScore(); 
        }
    }
}
