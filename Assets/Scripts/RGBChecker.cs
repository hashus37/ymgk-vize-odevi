using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RGBChecker : MonoBehaviour
{
    public static RGBChecker instance;

    public int gameIndex = 1;
    public List<GameObject> RGBCheckers1;
    public List<GameObject> RGBCheckers2;
    public List<GameObject> RGBCheckers3;
    public List<GameObject> RGBCheckers4;
    public List<GameObject> RGBCheckers5;
    public List<bool> isAllTrue;
    private Vector2[] tolarence = new Vector2[12] { new Vector2(0, 12), new Vector2(12, 0), new Vector2(0, -12), new Vector2(-12, 0), new Vector2(0, 25), new Vector2(25, 0), new Vector2(0, -25), new Vector2(-25, 0), new Vector2(0, 38), new Vector2(38, 0), new Vector2(0, -38), new Vector2(-38, 0)};
    private List<GameObject> currentRGBCheckers;
    private Texture2D SS;

    public TextMeshProUGUI text;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Debug.Break();

        //RGBChecker1.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //RGBChecker1.transform.position = new Vector3(RGBChecker1.transform.position.x, RGBChecker1.transform.position.y, 1f);
    }

    public IEnumerator Check()
    {
        switch (gameIndex)
        {
            case 0:
                currentRGBCheckers = RGBCheckers1;
                break;
            case 1:
                currentRGBCheckers = RGBCheckers2;
                break;
            case 2:
                currentRGBCheckers = RGBCheckers3;
                break;
            case 3:
                currentRGBCheckers = RGBCheckers4;
                break;
            case 4:
                currentRGBCheckers = RGBCheckers5;
                break;
        }
        while (true)
        {
            if (SS != null)
                Texture2D.DestroyImmediate(SS, true);

            yield return new WaitForEndOfFrame();
            SS = GetScreenshot(false);

            isAllTrue = new List<bool>(new bool[currentRGBCheckers.Count]);
            for(int i = 0; i < currentRGBCheckers.Count; i++)
            {
                Vector3 checkerPos = Camera.main.WorldToScreenPoint(currentRGBCheckers[i].transform.position);
                Color checkerColor = SS.GetPixel(Mathf.RoundToInt(checkerPos.x), Mathf.RoundToInt(checkerPos.y));

                if (checkerColor == new Color(1f, 1f, 1f))
                    isAllTrue[i] = true;
                else
                {
                    for(int y = 0; y < tolarence.Length; y++)
                    {
                        checkerColor = SS.GetPixel(Mathf.RoundToInt(checkerPos.x + tolarence[y].x), Mathf.RoundToInt(checkerPos.y + tolarence[y].y));
                        if (checkerColor == new Color(1f, 1f, 1f))
                        {
                            isAllTrue[i] = true;
                            break;
                        }
                    }
                }
            }
            if (isAllOfThemTrue())
            {
                if (!Draw.instance.canDraw)
                    yield break;
                Draw.instance.canDraw = false;
                UIManager.instance.currentText.text = "Þekli Doðru Bir Þekilde Tamamladýn!";
                yield return new WaitForSecondsRealtime(1.5f);
                Destroy(Draw.instance.brushInstance);
                isAllTrue.Clear();
                Debug.LogWarning("ahxxx");
                gameIndex++;
                StartCoroutine(UIManager.instance.NextGame(gameIndex));
                switch (gameIndex)
                {
                    case 0:
                        currentRGBCheckers = RGBCheckers1;
                        break;
                    case 1:
                        currentRGBCheckers = RGBCheckers2;
                        break;
                    case 2:
                        currentRGBCheckers = RGBCheckers3;
                        break;
                    case 3:
                        currentRGBCheckers = RGBCheckers4;
                        break;
                    case 4:
                        currentRGBCheckers = RGBCheckers5;
                        break;
                }
                yield break;
            }
        }
    }

    Texture2D GetScreenshot(bool argb32)
    {
        Rect viewRect = Camera.main.pixelRect;
        Texture2D tex = new Texture2D((int)viewRect.width, (int)viewRect.height, (argb32 ? TextureFormat.ARGB32 : TextureFormat.RGB24), false);
        tex.ReadPixels(viewRect, 0, 0, false);
        tex.Apply(false);
        return tex;
    }

    private bool isAllOfThemTrue()
    {
        for (int i = 0; i < isAllTrue.Count; i++)
        {
            if (isAllTrue[i] == false)
                return false;
        }
        return true;
    }


}
