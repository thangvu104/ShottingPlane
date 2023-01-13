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
    private int _totalNumberOfEnemiesDestroyed = 0;

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

        _player.name = "Player";
        _player.BulletsLayout = _bulletsLayout.transform;
        _player.GetComponent<Player>().OnLoseCallback = () =>
        {
            Debug.Log("Lose");
        };
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
            enemyScript.OnDestroyCallback = () =>
            {
                _totalNumberOfEnemiesDestroyed++;

                if(_totalNumberOfEnemiesDestroyed >= _totalEnemies)
                {
                    Debug.Log("Win");
                }
            };

            _enemies.Add(enemyScript);
        }


        for(int i = _totalEnemies - 1; i >= 0; i--)
        {
            if (this == null) return;

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
        float timeMoveBetweenTwoPoint = 0;

        for(int i = 0; i< _listPointInPath.Count; i++)
        {
            if (this == null || enemy == null) return;

            distance = i > 0? 
                Vector2.Distance(_listPointInPath[i - 1].position, _listPointInPath[i].position) :
                Vector2.Distance(_enemiesStartPoint.position, _listPointInPath[0].position);

            timeMoveBetweenTwoPoint = distance / speed;

            LeanTween.move(enemy?.gameObject, _listPointInPath[i].position, timeMoveBetweenTwoPoint);
            await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));
        }

        if (this == null) return;

        distance = Vector2.Distance(_listPointInPath[5].position, _listPointInPath[0].position);
        timeMoveBetweenTwoPoint = distance / speed;
        LeanTween.move(enemy.gameObject, _listPointInPath[0].position, timeMoveBetweenTwoPoint);
        await Task.Delay((int)(timeMoveBetweenTwoPoint * 1000));

        var endPoint = enemy.transform.parent.TransformPoint(Vector2.zero);
        distance = Vector2.Distance(enemy.transform.position, endPoint);
        
        LeanTween.moveLocal(enemy.gameObject, Vector2.zero, distance / speed);
    }
}
