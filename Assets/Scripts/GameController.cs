using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Image background_1;
    [SerializeField] private Image background_2;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private GameObject enemiesLayout;
    [SerializeField] private GameObject enemyPrefab;

    private RectTransform mainCanvasRecttransform;
    private float speedMoveBackground = 50;
    private int totalEnemies = 12;
    void Start()
    {
        mainCanvasRecttransform = mainCanvas.GetComponent<RectTransform>();
        PlayBackgroundEffect();
    }

    private void PlayBackgroundEffect()
    {
        var distanceMove = mainCanvasRecttransform.sizeDelta.y;
        LeanTween.moveLocalY(background_1.gameObject, -mainCanvasRecttransform.sizeDelta.y, distanceMove / speedMoveBackground).setRepeat(Int32.MaxValue).setOnCompleteOnRepeat(true);
        LeanTween.moveLocalY(background_2.gameObject, 0, distanceMove / speedMoveBackground).setRepeat(Int32.MaxValue).setOnCompleteOnRepeat(true);
    }
}
