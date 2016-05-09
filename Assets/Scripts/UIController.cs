using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    private const float FADE_ANIMATION_DURATION = 0.5f;

    private const float MESSAGE_DISPLAY_DURATION = 1.0f;

    private IEnumerator messageCorutine;

    private Text messageText;

    private Image[] lives;

    private Image[] bombs;

    private Text timerValue;

    private Text scoreValue;

    private Text rangeValue;

    private Text speedValue;

    private Image remoteDetonation;

    void Awake()
    {
        this.messageText = GameObject.Find("Message").GetComponent<Text>();

        this.lives = new Image[6];
        this.lives[0] = GameObject.Find("Heart Outer Right").GetComponent<Image>();
        this.lives[1] = GameObject.Find("Heart Inner Right").GetComponent<Image>();
        this.lives[2] = GameObject.Find("Heart Middle Right").GetComponent<Image>();
        this.lives[3] = GameObject.Find("Heart Middle Left").GetComponent<Image>();
        this.lives[4] = GameObject.Find("Heart Inner Left").GetComponent<Image>();
        this.lives[5] = GameObject.Find("Heart Outer Left").GetComponent<Image>();

        this.bombs = new Image[5];
        this.bombs[0] = GameObject.Find("Bomb Outer Right").GetComponent<Image>();
        this.bombs[1] = GameObject.Find("Bomb Inner Right").GetComponent<Image>();
        this.bombs[2] = GameObject.Find("Bomb Middle").GetComponent<Image>();
        this.bombs[3] = GameObject.Find("Bomb Inner Left").GetComponent<Image>();
        this.bombs[4] = GameObject.Find("Bomb Outer Left").GetComponent<Image>();

        this.timerValue = GameObject.Find("Timer Value").GetComponent<Text>();
        this.scoreValue = GameObject.Find("Score Value").GetComponent<Text>();
        this.rangeValue = GameObject.Find("Range Bonus Value").GetComponent<Text>();
        this.speedValue = GameObject.Find("Speed Bonus Value").GetComponent<Text>();

        this.remoteDetonation = GameObject.Find("Remote Detonation Bonus").GetComponent<Image>();
    }

    public void ShowTimedMessage(string text)
    {
        if (messageCorutine != null)
        {
            StopCoroutine(messageCorutine);
        }
        messageCorutine = DoShowMessage(text);
        StartCoroutine(messageCorutine);

        /* DEMO CODE */
        System.Random rand = new System.Random();
        int r = rand.Next(0, 7);
        SetLivesCount(r);
        SetBombsCount(r);
        SetScoreValue(rand.Next(0, 10000000));
        SetRangeBonusValue(r);
        SetSpeedBonusValue(rand.Next(100, 1000) / 100.0);
        SetRemoteDetonationBonusAvailable(rand.Next(0, 2) == 1);
        Debug.Log(r);
    }

    public void SetLivesCount(int count)
    {
        SetArrayItemsCount(lives, count);
    }

    public void SetBombsCount(int count)
    {
        SetArrayItemsCount(bombs, count);
    }

    private void SetArrayItemsCount(Image[] array, int count)
    {
        if (count >= 0 && count <= array.Length)
        {
            for (int i = 0; i < count; i++)
            {
                array[i].enabled = true;
            }
            for (int i = count; i < array.Length; i++)
            {
                array[i].enabled = false;
            }
        }
    }

    public void SetTimerValue(float timer)
    {
        float minutes = Mathf.Floor(timer / 60);
        float seconds = Mathf.RoundToInt(timer % 60);

        if (seconds < 10)
        {
            timerValue.text = minutes.ToString() + ":" + "0" + seconds.ToString();
        }
        else
        {
            timerValue.text = minutes.ToString() + ":" + seconds.ToString();
        }
    }

    public void SetScoreValue(double score)
    {
        scoreValue.text = ((int)score).ToString();
    }

    public void SetRangeBonusValue(int value)
    {
        rangeValue.text = "X" + value.ToString();
    }

    public void SetSpeedBonusValue(double value)
    {
        speedValue.text = "X" + value.ToString();
    }

    public void SetRemoteDetonationBonusAvailable(bool available)
    {
        remoteDetonation.enabled = available;
    }

    private IEnumerator DoShowMessage(string text)
    {
        messageText.canvasRenderer.SetAlpha(0.0f);
        messageText.CrossFadeAlpha(1.0f, FADE_ANIMATION_DURATION, false);
        messageText.text = text;
        yield return new WaitForSeconds(MESSAGE_DISPLAY_DURATION);
        messageText.CrossFadeAlpha(0.0f, FADE_ANIMATION_DURATION, false);
    }

}
