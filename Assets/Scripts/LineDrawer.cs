using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject LinePrefabs;
    public LayerMask CantDrawOverLayer;
    int CantDrawOverLayerIndex;

    public float LinePointsMinDistance;
    public float LineWidth;
    public Gradient LineColor;

    Line CurrentLine;

    private void Start()
    {
        CantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) BeginDraw();
        if (CurrentLine != null) Draw();
        if (Input.GetMouseButtonUp(0)) EndDraw();
    }

    private void BeginDraw()
    {
        Debug.Log("BeginDraw");
        CurrentLine = Instantiate(LinePrefabs, this.transform).GetComponent<Line>();

        CurrentLine.SetLineColor(LineColor);
        CurrentLine.SetLineWidth(LineWidth);
        CurrentLine.SetPointMinDistance(LinePointsMinDistance);
        CurrentLine.UsePhysics(false);
    }

    private void Draw()
    {
        Debug.Log("Draw");
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.CircleCast(MousePos, LineWidth / 3f, Vector2.zero, 1f, CantDrawOverLayer);

        if(hit)
        {
            EndDraw();
        }
        else
        {
            CurrentLine.AddPoint(MousePos);
        }
    }

    private void EndDraw()
    {
        Debug.Log("EndDraw");
        if (CurrentLine != null)
        {
            if (CurrentLine.pointsCount < 2)
            {
                Destroy(CurrentLine.gameObject);
            }
            else
            {
                CurrentLine.gameObject.layer = CantDrawOverLayerIndex;
                CurrentLine.UsePhysics(true);
                CurrentLine = null;
            }
        }
    }
}
