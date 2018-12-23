using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Level
{
    Level1, Level2, Level3, Level4, Level5, Level6, Level7
}

public struct LevelInfo
{
    public float movingSpeed;
    public Vector2 levelTiling;

    public LevelInfo(float movingSpeed, Vector2 levelTiling)
    {
        this.movingSpeed = movingSpeed;
        this.levelTiling = levelTiling;
    }
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public static Coroutine levelUpdater;

    static Dictionary<Level, LevelInfo> levelMap;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            levelMap = new Dictionary<Level, LevelInfo>()
            {
                { Level.Level1, new LevelInfo(2.0f, new Vector2(1,1)) },
                { Level.Level2, new LevelInfo(2.5f, new Vector2(2,1)) },
                { Level.Level3, new LevelInfo(3.0f, new Vector2(3,1)) },
                { Level.Level4, new LevelInfo(3.5f, new Vector2(3,2)) },
                { Level.Level5, new LevelInfo(4.0f, new Vector2(4,2.5f)) },
                { Level.Level6, new LevelInfo(4.5f, new Vector2(5,3)) },
                { Level.Level7, new LevelInfo(5.0f, new Vector2(6,3)) },
            };
        }
    }

    private void Start()
    {
        levelUpdater = StartCoroutine(LevelUpdater());
    }

    private void OnDestroy()
    {
        StopCoroutine(levelUpdater);
    }

    public void UpdateLevel(Level level)
    {
        var levelInfo = levelMap[level];
        CurvatureController.Instance.CrossFadeTiling(levelInfo.levelTiling);
        Utility.movingSpeed = levelInfo.movingSpeed;
        AudioManager.Instance.UpdatePitch(Constants.BACKGROUND_TUNNEL_AUDIO, Utility.movingSpeed);
    }

    public void StopLevelUpdater()
    {
        StopCoroutine(levelUpdater);
    }

    IEnumerator LevelUpdater()
    {
        AudioManager.Instance.Play(Constants.BACKGROUND_TUNNEL_AUDIO);
        UpdateLevel(Level.Level1);
        yield return new WaitForSeconds(Constants.LEVEL_1_EXIT_DURATION);
        UpdateLevel(Level.Level2);
        yield return new WaitForSeconds(Constants.LEVEL_2_EXIT_DURATION);
        UpdateLevel(Level.Level3);
        yield return new WaitForSeconds(Constants.LEVEL_3_EXIT_DURATION);
        UpdateLevel(Level.Level4);
        yield return new WaitForSeconds(Constants.LEVEL_4_EXIT_DURATION);
        UpdateLevel(Level.Level5);
        yield return new WaitForSeconds(Constants.LEVEL_5_EXIT_DURATION);
        UpdateLevel(Level.Level6);
        yield return new WaitForSeconds(Constants.LEVEL_6_EXIT_DURATION);
        UpdateLevel(Level.Level7);
        yield return new WaitForSeconds(Constants.LEVEL_7_EXIT_DURATION);
        Utility.isPoolingOver = true;
        TunnelManager.Instance.ReverseFinish(true);
        yield return new WaitForSeconds(Constants.FINISH_UP_TIME);
        Utility.isGameOver = true;
        Application.Quit();
    }
}
