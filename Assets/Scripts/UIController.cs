using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{

    private const float FADE_ANIMATION_DURATION = 0.5f;

    private const float MESSAGE_DISPLAY_DURATION = 1.0f;

    private Text messageText;

    void Awake()
    {
        this.messageText = GameObject.Find("Message").GetComponent<Text>();
    }

    public void ShowTimedMessage(string text)
    {
        StartCoroutine(DoShowMessage(text));
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
