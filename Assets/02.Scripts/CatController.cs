using System.Collections;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Video;
using Cat;
using System;

public class CatController : MonoBehaviour
{
    public SoundManager soundManager;
    public VideoManager videoManager;

    public GameObject gameOverUI;
    public GameObject fadeUI;

    private Rigidbody2D catRb;
    private Animator catAnim;

    public int jumpCount = 0;
    public float jumpPower = 30f;
    public float limitPower = 25f;

    private void Awake()    //시작할 때 딱 한 번만 실행
    {
        catRb = GetComponent<Rigidbody2D>();
        catAnim = GetComponent<Animator>();
    }

    private void OnEnable() //켜질 때 마다 한 번씩 실행
    {
        transform.localPosition = transform.position; //고양이 처음 위치

        GetComponent<BoxCollider2D>().enabled = true;
        soundManager.audioSource.Play();
    }

    private void Update()
    {
        Jump();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 5)
        {
            catAnim.SetTrigger("Jump");
            catAnim.SetBool("isGround", false);
            jumpCount++;
            soundManager.OnJumpSound();
            catRb.AddForceY(jumpPower, ForceMode2D.Impulse);

            if (catRb.linearVelocityY > limitPower) //점프 속도 제한
                catRb.linearVelocityY = limitPower;
        }

        var catRotation = transform.eulerAngles;
        catRotation.z = catRb.linearVelocityY * 2.5f;
        transform.eulerAngles = catRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Apple"))
        {
            other.gameObject.SetActive(false);
            other.transform.parent.GetComponent<ItemEvent>().particle.SetActive(true);

            GameManager.score++;

            if (GameManager.score == 10)
            {
                fadeUI.SetActive(true);
                fadeUI.GetComponent<FadeRoutine>().OnFade(3f, Color.white, true);
                GetComponent<BoxCollider2D>().enabled = false;

                StartCoroutine(EndingRoutine(true));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Pipe"))
        {
            soundManager.OnColliderSound();

            gameOverUI.SetActive(true);
            fadeUI.SetActive(true);
            fadeUI.GetComponent<FadeRoutine>().OnFade(3f, Color.black, true);
            GetComponent<BoxCollider2D>().enabled = false;

            StartCoroutine(EndingRoutine(false));
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            catAnim.SetBool("isGround", true);
            jumpCount = 0;
        }
    }

    IEnumerator EndingRoutine(bool isHappy)
    {
        yield return new WaitForSeconds(3.5f);

        videoManager.VideoPlay(isHappy);
        yield return new WaitForSeconds(1f);

        var newColor = isHappy ? Color.white : Color.black;
        fadeUI.GetComponent<FadeRoutine>().OnFade(3f, newColor, false);

        yield return new WaitForSeconds(3f);
        fadeUI.SetActive(false);
        gameOverUI.SetActive(false);
        soundManager.audioSource.Stop();

        transform.parent.gameObject.SetActive(false);
    }
}
