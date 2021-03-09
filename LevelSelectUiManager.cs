using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUiManager : MonoBehaviour
{
    [SerializeField] GameObject levelHolder;

    [SerializeField] float moveX = 900;
    [SerializeField] float movementTime = 0.2f;
    [SerializeField] LeanTweenType type;

    float position = 0;
    int currentLevelIndex = 1; // TODO set current level from save in start and use DoMove

    private void Start()
    {
        position = levelHolder.GetComponent<RectTransform>().position.x;
    }

    public void DoMove(int levelEntry)
    {
        position -= moveX + moveX * levelEntry;
        LeanTween.moveX(levelHolder, position, movementTime).setEase(type);
    }
    public void MoveNext()
    {
        DoMove(0);
        currentLevelIndex++;
    }
    public void MovePrevious()
    {
        if (currentLevelIndex <= 1) return;
        DoMove(-2);
        currentLevelIndex--;
    }
}
