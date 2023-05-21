using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public List<Canvas> canvases;

    public TextMeshProUGUI currentText;

    public List<Color> colors;

    public float speed;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        currentText = canvases[0].transform.GetChild(0).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        Application.targetFrameRate = 60;
    }

    public IEnumerator NextGame(int gameIndex)
    {
        Debug.Log("ah");
        CanvasGroup CG1 = canvases[gameIndex - 1].GetComponent<CanvasGroup>();
        CanvasGroup CG2 = canvases[gameIndex].GetComponent<CanvasGroup>();
        float t = 0;
        while (CG2.alpha < 1f)
        {
            t += speed * Time.deltaTime;
            Camera.main.backgroundColor = Color.Lerp(colors[gameIndex - 1], colors[gameIndex], t);
            CG1.alpha = Mathf.Lerp(1, 0, t);
            CG2.alpha = Mathf.Lerp(0, 1, t);

            yield return null;
        }
        Camera.main.backgroundColor = colors[gameIndex];
        CG1.alpha = 0;
        CG2.alpha = 1;
        if(gameIndex < 5)
            currentText = canvases[gameIndex].transform.GetChild(0).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        yield return new WaitForSecondsRealtime(.5f);
        Draw.instance.canDraw = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
