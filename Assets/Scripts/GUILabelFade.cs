using UnityEngine;
using System.Collections;

public class GUILabelFade : Object
{

    private float duration = 5F;
    private float durCount = 0F;
    private string text = "";
    private Rect pos;
    public bool hasCompleted = false;
    //Fade direction, fade in = true
    private bool fadeIn = false;
    private int fontSize;

    private int min, max;

    public GUILabelFade(float dur, string text, Rect pos, int fontSize, bool fadeIn = true)
    {
        this.hasCompleted = false;
        this.duration = dur;
        this.text = text;
        this.pos = pos;
        this.fontSize = fontSize;
        this.fadeIn = fadeIn;

        if (fadeIn)
        {
            this.durCount = dur;
        }
    }
    public void Render(Color prevColor)
    {
        if (!hasCompleted)
        {
            if (this.fadeIn)
            {
                durCount -= Time.deltaTime;
            }
            else {
                durCount += Time.deltaTime;
            }
            if (durCount < 0)
            {
                durCount = 0;
            }
            else if (durCount > duration)
            {
                durCount = duration;
            }
            GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, durCount == 0 ? (fadeIn ? 1 : 0) : Mathf.Lerp(1, 0, durCount / duration));
            if ((GUI.color.a == 1 && fadeIn )|| (GUI.color.a == 0 && !fadeIn ))
            {
                hasCompleted = true;
            }
        }
        if ((!hasCompleted && !fadeIn) || (hasCompleted && fadeIn) || !hasCompleted)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = this.fontSize;
            style.normal.textColor = prevColor;
            GUI.Label(pos, text, style);
            GUI.color = prevColor;
        }
    }
}