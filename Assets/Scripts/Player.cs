using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private List<Transform> _listEndPointOfBullet;
    [SerializeField] private GameObject _bulletPrefab;

    private bool _startShotting;
    private Transform _bulletsLayout;

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
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _startShotting = false;
    }

    private void Update()
    {
        if(_startShotting)
        {
            Shotting(3);
        }
    }

    private void Shotting(int numberOfBullet)
    {
        if (numberOfBullet <= 0) return;

        for (int i = 0; i < numberOfBullet; i++)
        {
            var bullet = Instantiate(_bulletPrefab, _bulletsLayout);

            bullet.transform.position = transform.TransformPoint(Vector3.zero);
            bullet.GetComponent<RectTransform>().rotation.SetEulerRotation(0, 0, 30);
        }
    }
}
