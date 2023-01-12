using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Image background_1;
    [SerializeField] private Image background_2;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private GameObject enemiesLayout;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private List<Transform> listPointInPath;

    private List<Enemies> enemies = new List<Enemies>();
    private RectTransform mainCanvasRecttransform;
    private float speedMoveBackground = 50;
    private int totalEnemies = 12;

    void Start()
    {
        mainCanvasRecttransform = mainCanvas.GetComponent<RectTransform>();
        PlayBackgroundEffect();

        SpawnEnemies();
    }

    private void PlayBackgroundEffect()
    {
        var distanceMove = mainCanvasRecttransform.sizeDelta.y;
        LeanTween.moveLocalY(background_1.gameObject, -mainCanvasRecttransform.sizeDelta.y, distanceMove / speedMoveBackground).setRepeat(Int32.MaxValue).setOnCompleteOnRepeat(true);
        LeanTween.moveLocalY(background_2.gameObject, 0, distanceMove / speedMoveBackground).setRepeat(Int32.MaxValue).setOnCompleteOnRepeat(true);
    }

    private async void SpawnEnemies()
    {
        for(int i = 0; i < totalEnemies; i++)
        {
            var enemyContainer = Instantiate(enemyPrefab, enemiesLayout.transform);
            var enemyScript = enemyContainer.transform.Find("EnemySprite")?.GetComponent<Enemies>();

            enemies.Add(enemyScript);
        }


        for(int i = totalEnemies - 1; i >= 0; i--)
        {
            enemies[i].transform.position = startPoint.position;
            enemies[i].gameObject.SetActive(true);

            EnemyMoveInPath(enemies[i]);
            await Task.Delay(1000);
        }
    }

    private async void EnemyMoveInPath(Enemies enemy)
    {
        var speed = 2f;
        var distance = Vector2.Distance(startPoint.position, listPointInPath[0].position);
        float timeMoveBetweenTwoPoint = distance / speed;

        LeanTween.move(enemy.gameObject, listPointInPath[0].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(listPointInPath[0].position, listPointInPath[1].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, listPointInPath[1].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(listPointInPath[1].position, listPointInPath[2].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, listPointInPath[2].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(listPointInPath[2].position, listPointInPath[3].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, listPointInPath[3].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(listPointInPath[3].position, listPointInPath[4].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, listPointInPath[4].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(listPointInPath[4].position, listPointInPath[5].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, listPointInPath[5].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(listPointInPath[5].position, listPointInPath[0].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, listPointInPath[0].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        var endPoint = enemy.transform.parent.TransformPoint(Vector2.zero);
        distance = Vector2.Distance(enemy.transform.position, endPoint);
        
        LeanTween.moveLocal(enemy.gameObject, Vector2.zero, distance / speed);
    }
}
