using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField] public BoxCollider2D boxCollider;
    public Action OnDestroyCallback = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy") return;

        if(collision.gameObject.tag == "player")
        {
            collision.gameObject.GetComponent<Player>().OnLoseCallback?.Invoke();
        }
        OnDestroyCallback?.Invoke();

        LeanTween.cancel(collision.gameObject);
        Destroy(collision.gameObject);

        DestroyAnimation(() =>
        {
            LeanTween.cancel(gameObject);
            Destroy(gameObject);
        });
    }

    private void DestroyAnimation(Action callback = null)
    {
        Debug.Log("Enemies : DestroyAnimation");

        callback?.Invoke();
    }
}
