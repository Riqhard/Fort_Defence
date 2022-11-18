using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Image fadeScreenImage;
    public GameObject gameOverUI;

    public float endScreenFadeDuration = 1f;
    public Color endScreenColor;

    public int gold;
    public int food;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI foodText;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<Forth>().OnDeath += GameOver;
    }

    public void GameOver()
    {
        StartCoroutine(Fade(Color.clear, endScreenColor, endScreenFadeDuration));
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;


        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadeScreenImage.color = Color.Lerp(from, to, percent);
            yield return null;
        }
        gameOverUI.SetActive(true);
    }



    public void ChangeGoldAmount(int amount)
    {
        gold += amount;
        goldText.text = "" + gold;
    }
    public void ChangeFoodAmount(int amount)
    {
        food += amount;
        foodText.text = "" + food;
    }

    // UI Input

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
