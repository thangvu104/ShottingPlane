using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Image _background_1;
    [SerializeField] private Image _background_2;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Transform _gameLayout;

    [SerializeField] private GameObject _enemiesLayout;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemiesStartPoint;
    [SerializeField] private List<Transform> _listPointInPath;

    [SerializeField] private Transform _playerStartPoint;
    [SerializeField] private GameObject _bulletsLayout;
    [SerializeField] private GameObject _playerPlanePrefab;

    private List<Enemies> _enemies = new List<Enemies>();
    private RectTransform _mainCanvasRecttransform;
    private float _speedMoveBackground = 50;
    private int _totalEnemies = 12;

    private Player _player = default;

    void Start()
    {
        _mainCanvasRecttransform = _mainCanvas.GetComponent<RectTransform>();

        PlayBackgroundEffect();
        CreatePlayerPlane();

        SpawnEnemies();
    }

    private void CreatePlayerPlane()
    {
        _player = Instantiate(_playerPlanePrefab, _playerStartPoint.position, Quaternion.identity, _gameLayout).GetComponent<Player>();

        _player.BulletsLayout = _bulletsLayout.transform;
    }
    private void PlayBackgroundEffect()
    {
        var distanceMove = _mainCanvasRecttransform.sizeDelta.y;
        LeanTween.moveLocalY(_background_1.gameObject, -_mainCanvasRecttransform.sizeDelta.y, distanceMove / _speedMoveBackground).setRepeat(Int32.MaxValue).setOnCompleteOnRepeat(true);
        LeanTween.moveLocalY(_background_2.gameObject, 0, distanceMove / _speedMoveBackground).setRepeat(Int32.MaxValue).setOnCompleteOnRepeat(true);
    }

    private async void SpawnEnemies()
    {
        for(int i = 0; i < _totalEnemies; i++)
        {
            var enemyContainer = Instantiate(_enemyPrefab, _enemiesLayout.transform);
            var enemyScript = enemyContainer.transform.Find("EnemySprite")?.GetComponent<Enemies>();

            _enemies.Add(enemyScript);
        }


        for(int i = _totalEnemies - 1; i >= 0; i--)
        {
            _enemies[i].transform.position = _enemiesStartPoint.position;
            _enemies[i].gameObject.SetActive(true);

            EnemyMoveInPath(_enemies[i]);
            await Task.Delay(1000);
        }
    }

    private async void EnemyMoveInPath(Enemies enemy)
    {
        var speed = 2f;
        var distance = Vector2.Distance(_enemiesStartPoint.position, _listPointInPath[0].position);
        float timeMoveBetweenTwoPoint = distance / speed;

        LeanTween.move(enemy.gameObject, _listPointInPath[0].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(_listPointInPath[0].position, _listPointInPath[1].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, _listPointInPath[1].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(_listPointInPath[1].position, _listPointInPath[2].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, _listPointInPath[2].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(_listPointInPath[2].position, _listPointInPath[3].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, _listPointInPath[3].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(_listPointInPath[3].position, _listPointInPath[4].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, _listPointInPath[4].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(_listPointInPath[4].position, _listPointInPath[5].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, _listPointInPath[5].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        distance = Vector2.Distance(_listPointInPath[5].position, _listPointInPath[0].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, _listPointInPath[0].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        var endPoint = enemy.transform.parent.TransformPoint(Vector2.zero);
        distance = Vector2.Distance(enemy.transform.position, endPoint);
        
        LeanTween.moveLocal(enemy.gameObject, Vector2.zero, distance / speed);
    }
}
