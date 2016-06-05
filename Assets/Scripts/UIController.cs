using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Model;
using Assets.Scripts.Position;

public class UIController : MonoBehaviour
{
    private const float MESSAGE_FADE_ANIMATION_DURATION = 0.5f;

    private const float MESSAGE_DISPLAY_DURATION = 1.0f;

    private const float WIDTH_CORRECTION = 7;

    private const float HEIGHT_CORRECTION = 5;

    private const float MINIMAP_X_CORRECTION = 3;

    private const float MINIMAP_Y_CORRECTION = 2;

    private const float LEVEL_MESSAGE_DISPLAY_DURATION = 3.0f;

    private const float GAMEPLAY_FADE_DURATION = 1.0f;

    private const float LETTER_TYPING_DELAY = 0.05f;

    private IEnumerator messageCorutine;

    /******************* HUD ELEMENTS *******************/

    private Text messageText;

    private Text levelMessageText;

    private Image[] lives;

    private Image[] bombs;

    private Text timerValue;

    private Text scoreValue;

    private Text rangeValue;

    private Text speedValue;

    private Image remoteDetonation;

    /***************** SUMMARY ELEMENTS *****************/

    private IEnumerator summaryDisplayCoroutine;

    private GameObject summaryPane;

    private Text summaryText;

    private Text clearBonusText;

    private Text clearBonusValue;

    private Text blocksBonusText;

    private Text blocksBonusValue;

    private Text enemiesBonusText;

    private Text enemiesBonusValue;

    private Text pickupBonusText;

    private Text pickupBonusValue;

    private Text timeBonusText;

    private Text timeBonusValue;

    private Text totalScoreText;

    private string clearBonusValueString;

    private string blocksBonusValueString;

    private string monsterBonusValueString;

    private string pickupBonusValueString;

    private string timeBonusValueString;

    private string totalScoreString;

    private bool displaySkipAllowed = false;

    private bool summarySkipAllowed = false;

    void Awake()
    {
        InitializeHUDElements();
        InitializeSummaryElements();
        InitializeWatermark();
    }

    private void InitializeHUDElements()
    {
        this.messageText = GameObject.Find("Message").GetComponent<Text>();
        this.levelMessageText = GameObject.Find("Level Message").GetComponent<Text>();

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

    private void InitializeSummaryElements()
    {
        this.summaryPane = GameObject.Find("Level Summary");
        this.summaryText = GameObject.Find("Summary Message").GetComponent<Text>();

        this.clearBonusText = GameObject.Find("Clear Bonus Text").GetComponent<Text>();
        this.clearBonusValue = GameObject.Find("Clear Bonus Value").GetComponent<Text>();
        this.blocksBonusText = GameObject.Find("Block Destroy Bonus Text").GetComponent<Text>();
        this.blocksBonusValue = GameObject.Find("Block Destroy Bonus Value").GetComponent<Text>();
        this.enemiesBonusText = GameObject.Find("Enemy Kill Bonus Text").GetComponent<Text>();
        this.enemiesBonusValue = GameObject.Find("Enemy Kill Bonus Value").GetComponent<Text>();
        this.pickupBonusText = GameObject.Find("Pickup Bonus Text").GetComponent<Text>();
        this.pickupBonusValue = GameObject.Find("Pickup Bonus Value").GetComponent<Text>();
        this.timeBonusText = GameObject.Find("Time Bonus Text").GetComponent<Text>();
        this.timeBonusValue = GameObject.Find("Time Bonus Value").GetComponent<Text>();

        this.totalScoreText = GameObject.Find("Total Message").GetComponent<Text>();
    }

    private void InitializeWatermark()
    {
        if (GameManager.instance.IsWatermarkEnabled())
        {
            Text watermark = GameObject.Find("Watermark").GetComponent<Text>();
            watermark.text = Version.WATERMARK_TEXT;
        }
    }

    public void InitializeHUD(float mazeWidth, float mazeLength)
    {
        Player player = GameManager.instance.GetPlayer();
        SetLivesCount(player.remainingLives);
        SetBombsCount(player.bombs);
        SetScoreValue(player.score);
        SetRangeBonusValue(player.bombRange);
        SetSpeedBonusValue(player.speed);
        SetRemoteDetonationBonusAvailable(player.remoteDetonationBonus);
        PrepareMiniMap(player, mazeWidth, mazeLength);
        StartCoroutine(DoShowLevelMessage());
    }

    public void ShowTimedMessage(string text)
    {
        if (messageCorutine != null)
        {
            StopCoroutine(messageCorutine);
        }
        messageCorutine = DoShowMessage(text);
        StartCoroutine(messageCorutine);
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
        int hours = (int)Mathf.Floor(timer / 3600);
        int minutes = (int)Mathf.Floor((timer - 3600 * hours) / 60);
        int seconds = Mathf.RoundToInt(timer % 60);

        if (hours > 0)
        {
            timerValue.text = hours.ToString() + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }
        else
        {
            timerValue.text = minutes.ToString() + ":" + seconds.ToString("D2");
        }
    }

    public void SetScoreValue(long score)
    {
        scoreValue.text = score.ToString();
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
        messageText.CrossFadeAlpha(1.0f, MESSAGE_FADE_ANIMATION_DURATION, false);
        messageText.text = text;
        yield return new WaitForSeconds(MESSAGE_DISPLAY_DURATION);
        messageText.CrossFadeAlpha(0.0f, MESSAGE_FADE_ANIMATION_DURATION, false);
    }

    private void PrepareMiniMap(Player player, float mazeWidth, float mazeLength)
    {
        GameObject minimapCameraObject = GameObject.Find(Constants.MINIMAP_CAMERA_NAME);
        Camera minimapCamera = minimapCameraObject.GetComponent<Camera>();
        AdjustMiniMapSizeAndPosition(minimapCamera);
        player.cameraPositionListener = new CameraPositionListener(minimapCamera, mazeWidth, mazeLength);
        player.cameraPositionListener.OnPostionChanged(player.transform.position);
    }

    private void AdjustMiniMapSizeAndPosition(Camera minimapCamera)
    {
        GameObject minimap = GameObject.Find(Constants.MINIMAP_NAME);
        RectTransform transform = minimap.GetComponent<RectTransform>();
        Rect cameraRect = minimapCamera.rect;
        cameraRect.width = (transform.rect.width - WIDTH_CORRECTION) / Screen.width;
        cameraRect.height = (transform.rect.height - HEIGHT_CORRECTION) / Screen.height;
        cameraRect.x = (MINIMAP_X_CORRECTION + transform.position.x - transform.rect.width / 2) / Screen.width;
        cameraRect.y = (MINIMAP_Y_CORRECTION + transform.position.y - transform.rect.height / 2) / Screen.height;
        minimapCamera.rect = cameraRect;
    }

    private IEnumerator DoShowLevelMessage()
    {
        levelMessageText.canvasRenderer.SetAlpha(0.0f);
        levelMessageText.CrossFadeAlpha(1.0f, 2 * MESSAGE_FADE_ANIMATION_DURATION, false);
        levelMessageText.text = "LEVEL " + GameManager.instance.GetCurrentLevelNumber();
        yield return new WaitForSeconds(LEVEL_MESSAGE_DISPLAY_DURATION);
        levelMessageText.CrossFadeAlpha(0.0f, 3 * MESSAGE_FADE_ANIMATION_DURATION, false);
    }

    /***************************************** LEVEL SUMMARY SCREEEN METHODS *****************************************/

    public void Update()
    {
        if (displaySkipAllowed)
        {
            if (Input.anyKeyDown)
            {
                StopCoroutine(summaryDisplayCoroutine);
                clearBonusText.text = SummaryStringConstants.CLEAR_BONUS_MESSAGE;
                clearBonusValue.text = clearBonusValueString;
                blocksBonusText.text = SummaryStringConstants.BLOCKS_BONUS_MESSAGE;
                blocksBonusValue.text = blocksBonusValueString;
                enemiesBonusText.text = SummaryStringConstants.MONSTERS_BONUS;
                enemiesBonusValue.text = monsterBonusValueString;
                pickupBonusText.text = SummaryStringConstants.PICKUPS_BONUS;
                pickupBonusValue.text = pickupBonusValueString;
                timeBonusText.text = SummaryStringConstants.TIME_BONUS;
                timeBonusValue.text = timeBonusValueString;
                totalScoreText.text = totalScoreString;
                displaySkipAllowed = false;
                summarySkipAllowed = true;
            }
        }
        else if (summarySkipAllowed)
        {
            if (Input.anyKeyDown)
            {
                summarySkipAllowed = false;
                GameManager.instance.OnSummaryDisplayingFinished();
            }
        }
    }

    public void DisplayLevelSummary(bool cleared, int remainingLives, int clearBonus, int blocksBonus, int monsterBonus, int pickupBonus, int timeBonus)
    {
        this.clearBonusValueString = clearBonus.ToString();
        this.blocksBonusValueString = blocksBonus.ToString();
        this.monsterBonusValueString = monsterBonus.ToString();
        this.pickupBonusValueString = pickupBonus.ToString();
        this.timeBonusValueString = timeBonus.ToString();
        this.totalScoreString = string.Format(SummaryStringConstants.TOTAL_SCORE, (clearBonus + blocksBonus + monsterBonus + pickupBonus + timeBonus));

        this.summaryDisplayCoroutine = SummaryDisplayCoroutine(GetTopMessageString(cleared, remainingLives));
        StartCoroutine(summaryDisplayCoroutine);
    }

    private string GetTopMessageString(bool levelCleared, int remainingLives)
    {
        if (levelCleared)
        {
            return SummaryStringConstants.LEVEL_CLEARED_MESSAGE;
        } else
        {
            return remainingLives > 0 ? SummaryStringConstants.PLAYER_KILLED_MESSAGE : SummaryStringConstants.GAME_OVER_MESSAGE;
        }
    }

    private IEnumerator SummaryDisplayCoroutine(string topMessage)
    {
        // fade gameplay
        Image image = summaryPane.GetComponent<Image>();
        while (image.color.a < 1)
        {
            float oldAlpha = image.color.a;
            summaryPane.GetComponent<Image>().color = new Color(0, 0, 0, oldAlpha + Time.deltaTime / GAMEPLAY_FADE_DURATION);
            yield return null;
        }

        // display summary - top message
        for (int i = 0; i <= topMessage.Length; i++)
        {
            summaryText.text = topMessage.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        this.displaySkipAllowed = true;
        // display summary - clear bonus
        for (int i = 0; i <= SummaryStringConstants.CLEAR_BONUS_MESSAGE.Length; i++)
        {
            clearBonusText.text = SummaryStringConstants.CLEAR_BONUS_MESSAGE.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        for (int i = 0; i <= this.clearBonusValueString.Length; i++)
        {
            clearBonusValue.text = this.clearBonusValueString.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        // display summary - blocks bonus
        for (int i = 0; i <= SummaryStringConstants.BLOCKS_BONUS_MESSAGE.Length; i++)
        {
            blocksBonusText.text = SummaryStringConstants.BLOCKS_BONUS_MESSAGE.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        for (int i = 0; i <= this.blocksBonusValueString.Length; i++)
        {
            blocksBonusValue.text = this.blocksBonusValueString.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        // display summary - enemies bonus
        for (int i = 0; i <= SummaryStringConstants.MONSTERS_BONUS.Length; i++)
        {
            enemiesBonusText.text = SummaryStringConstants.MONSTERS_BONUS.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        for (int i = 0; i <= this.monsterBonusValueString.Length; i++)
        {
            enemiesBonusValue.text = this.monsterBonusValueString.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        // display summary - pickup bonus
        for (int i = 0; i <= SummaryStringConstants.PICKUPS_BONUS.Length; i++)
        {
            pickupBonusText.text = SummaryStringConstants.PICKUPS_BONUS.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        for (int i = 0; i <= this.pickupBonusValueString.Length; i++)
        {
            pickupBonusValue.text = this.pickupBonusValueString.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        // display summary - time bonus
        for (int i = 0; i <= SummaryStringConstants.TIME_BONUS.Length; i++)
        {
            timeBonusText.text = SummaryStringConstants.TIME_BONUS.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        for (int i = 0; i <= this.timeBonusValueString.Length; i++)
        {
            timeBonusValue.text = this.timeBonusValueString.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        // display summary - total score
        for (int i = 0; i <= this.totalScoreString.Length; i++)
        {
            totalScoreText.text = this.totalScoreString.Substring(0, i);
            yield return new WaitForSeconds(LETTER_TYPING_DELAY);
        }
        this.displaySkipAllowed = false;
        this.summarySkipAllowed = true;
    }

    private class SummaryStringConstants
    {
        public const string LEVEL_CLEARED_MESSAGE = "Level Cleared!";

        public const string GAME_OVER_MESSAGE = "Game Over!";

        public const string PLAYER_KILLED_MESSAGE = "Player Killed!";

        public const string CLEAR_BONUS_MESSAGE = "Level Clear Bonus";

        public const string BLOCKS_BONUS_MESSAGE = "Block Destroy Bonus";

        public const string MONSTERS_BONUS = "Enemy Kill Bonus";

        public const string PICKUPS_BONUS = "Pickup Bonus";

        public const string TIME_BONUS = "Time Bonus";

        public const string TOTAL_SCORE = "Level Score: {0:0}";
    }

}
