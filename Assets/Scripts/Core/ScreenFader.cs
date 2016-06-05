using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    private Material m_Material = null;

    private static SceneFader m_Instance = null;

    private static SceneFader instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (new GameObject("AutoFade")).AddComponent<SceneFader>();
            }
            return m_Instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        m_Instance = this;
        Shader shader = Shader.Find("ScreenFader");
        m_Material = new Material(shader);
    }

    private void DrawQuad(Color aColor, float aAlpha)
    {
        aColor.a = aAlpha;
        m_Material.SetPass(0);
        GL.Begin(GL.QUADS);
        GL.Color(aColor); // This function can only be called between GL.Begin and GL.End functions.
        GL.PushMatrix();
        GL.LoadOrtho();        
        GL.Vertex3(0, 0, 0);
        GL.Vertex3(0, 1, 0);
        GL.Vertex3(1, 1, 0);
        GL.Vertex3(1, 0, 0);
        GL.End();
        GL.PopMatrix();
    }

    private IEnumerator Fade(string aSceneName, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
            DrawQuad(aColor, t);
        }

        SceneManager.LoadScene(aSceneName);
        
        while (t > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
            DrawQuad(aColor, t);
        }
    }

    private void StartFade(string aSceneName, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        StartCoroutine(Fade(aSceneName, aFadeOutTime, aFadeInTime, aColor));
    }

    public static void LoadScene(string aSceneName, float aFadeOutTime, float aFadeInTime)
    {
        instance.StartFade(aSceneName, aFadeOutTime, aFadeOutTime, Color.black);
    }

}