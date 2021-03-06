﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurvatureType
{
    Left, Right, Top, Bottom, TopLeft, TopRight, BottomLeft, BottomRight
}

public class CurvatureController : MonoBehaviour
{
    public static CurvatureController Instance;

    [Header("Material")]
    public Material tunnelMat;

    [Header("Curvature")]
    public int curvatureMultiplier = 45;
    public float curvatureDelay = 5;
    public Vector2 curvature;

    [Header("Tiling")]
    public Vector2 tiling = Vector2.one;

    [Header("Testing")]
    public CurvatureType curvatureType = CurvatureType.Top;

    static Dictionary<CurvatureType, Vector2> curveMapSimple, curveMapComplex;

    Vector2 currentCurvature = Vector2.zero;
    Vector2 currentTiling = new Vector2(.1f, 1);
    bool isCurveSimple;

    LTDescr curveLT, tilingLT;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (!Instance)
        {
            Instance = this;

            curveMapSimple = new Dictionary<CurvatureType, Vector2> {
                { CurvatureType.Top, Vector2.up },
                { CurvatureType.Left, Vector2.left },
                { CurvatureType.Right, Vector2.right },
                { CurvatureType.Bottom, Vector2.down },
            };

            curveMapComplex = new Dictionary<CurvatureType, Vector2> {
                { CurvatureType.TopLeft, Vector2.left + Vector2.up },
                { CurvatureType.TopRight, Vector2.right + Vector2.up },
                { CurvatureType.BottomLeft, Vector2.left + Vector2.down },
                { CurvatureType.BottomRight, Vector2.right + Vector2.down }
            };
        }
    }

    private void Start()
    {
        StartCoroutine(ChangeCurve());
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Pseudo/Capture")]
    public static void Capture()
    {
        ScreenCapture.CaptureScreenshot(string.Format("{0}.png", System.DateTime.Now.Ticks.ToString()));
    }
#endif

    IEnumerator ChangeCurve()
    {
        while (!Utility.isPoolingOver)
        {
            CrossFadeCurvature((isCurveSimple = !isCurveSimple) ? curveMapSimple[(CurvatureType)Random.Range(0, 4)] : curveMapComplex[(CurvatureType)Random.Range(4, 8)]);
            yield return new WaitForSeconds(curvatureDelay);
        }
    }

    public void CrossFadeCurvature(Vector2 curvature)
    {
        if (Utility.isPoolingOver)
            return;
        // If game is over add a check to not add a leantween
        if (curveLT != null)
            if (LeanTween.isTweening(curveLT.uniqueId))
                curveLT.setOnComplete(() => { curveLT = LeanTween.value(gameObject, UpdateCurvature, currentCurvature, (currentCurvature = curvature * curvatureMultiplier), 3f); });
        curveLT = LeanTween.value(gameObject, UpdateCurvature, currentCurvature, (currentCurvature = curvature * curvatureMultiplier), 3f);
    }

    public void CrossFadeTiling(Vector2 tiling)
    {
        if (Utility.isPoolingOver)
        {
            if (tilingLT != null)
                LeanTween.cancel(tilingLT.uniqueId);
            tilingLT = LeanTween.value(gameObject, UpdateTiling, currentTiling, currentTiling = tiling, 3f);
            return;
        }
        if (tilingLT != null)
            if (LeanTween.isTweening(tilingLT.uniqueId))
                tilingLT.setOnComplete(() => { tilingLT = LeanTween.value(gameObject, UpdateTiling, currentTiling, currentTiling = tiling, 3f); });
        tilingLT = LeanTween.value(gameObject, UpdateTiling, currentTiling, currentTiling = tiling, 3f);
    }

    public void CrossFadeTiling(Vector2 tiling, float time)
    {
        if (Utility.isPoolingOver)
        {
            if (tilingLT != null)
                LeanTween.cancel(tilingLT.uniqueId);
            tilingLT = LeanTween.value(gameObject, UpdateTiling, currentTiling, currentTiling = tiling, time);
            return;
        }
        if (tilingLT != null)
            if (LeanTween.isTweening(tilingLT.uniqueId))
                tilingLT.setOnComplete(() => { tilingLT = LeanTween.value(gameObject, UpdateTiling, currentTiling, currentTiling = tiling, time); });
        tilingLT = LeanTween.value(gameObject, UpdateTiling, currentTiling, currentTiling = tiling, time);
    }

    void UpdateCurvature(Vector2 curve)
    {
        tunnelMat.SetVector("_QOffset", curve);
    }

    void UpdateTiling(Vector2 tiling)
    {
        tunnelMat.SetTextureScale("_MainTex", tiling);
    }
}
