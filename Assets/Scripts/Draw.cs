using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public static Draw instance;

    public GameObject brush;
    public GameObject canvas;
    public bool canDraw = true;

    LineRenderer currentLineR;
    public GameObject brushInstance;

    Vector2 lastPos;
    Coroutine myCor;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        DrawSomething();
    }

    void DrawSomething()
    {
        if (!canDraw)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(brushInstance != null)
                Destroy(brushInstance);

            brushInstance = Instantiate(brush);
            currentLineR = brushInstance.GetComponent<LineRenderer>();

            Vector2 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            currentLineR.SetPosition(0, mouseLoc);
            currentLineR.SetPosition(1, mouseLoc);

            myCor = StartCoroutine(RGBChecker.instance.Check());
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (brushInstance == null)
                return;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos != lastPos) {
                currentLineR.positionCount++;
                int posIndex = currentLineR.positionCount - 1;
                currentLineR.SetPosition(posIndex, mousePos);
                lastPos = mousePos;
            }
        }
        else
            currentLineR = null;
    }

}
