using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private List<Transform> _listEndPointOfBullet;
    [SerializeField] private GameObject _bulletPrefab;

    private bool _startShotting;
    private Transform _bulletsLayout;

    public Action OnLoseCallback = default;

    public Transform BulletsLayout
    {
        get => _bulletsLayout;
        set => _bulletsLayout = value;
    }
    void Start()
    {
        _startShotting = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        var rectTransform = gameObject.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _startShotting = true;
        Shotting(3);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _startShotting = false;
    }

    private async void Update()
    {
        /*if(_startShotting)
        {
            Shotting(3);
            await Task.Delay(500);
        }*/
    }

    private async void Shotting(int numberOfBullet)
    {
        if (numberOfBullet <= 0 || !_startShotting) return;

        var endPoint_1 = _listEndPointOfBullet[0].transform.position;
        var alpha = Mathf.Atan(endPoint_1.x / endPoint_1.y);
        var time = 1f;
        for (int i = 0; i < numberOfBullet; i++)
        {
            var bullet = Instantiate(_bulletPrefab, _bulletsLayout);
            bullet.name = "Bullet";
            bullet.transform.position = transform.TransformPoint(Vector3.zero);

            switch (i)
            {
                case 0:
                    bullet.transform.Rotate(0, 0, 0);
                    LeanTween.move(bullet, _listEndPointOfBullet[1].transform.position, time).setOnComplete(()=> {
                        Destroy(bullet);
                    });
                    break;
                case 1:
                    bullet.transform.Rotate(0, 0, alpha);
                    LeanTween.move(bullet, _listEndPointOfBullet[2].transform.position, time).setOnComplete(() => {
                        Destroy(bullet);
                    });
                    break;
                case 2:
                    bullet.transform.Rotate(0, 0, -alpha);
                    LeanTween.move(bullet, _listEndPointOfBullet[0].transform.position, time).setOnComplete(() => {
                        Destroy(bullet);
                    });
                    break;
                default:
                    bullet.transform.Rotate(0, 0, 0);
                    break;
            }
        }

        await Task.Delay(500);

        Shotting(numberOfBullet);
    }
}
