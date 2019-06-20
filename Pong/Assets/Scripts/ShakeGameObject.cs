using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeGameObject : MonoBehaviour
{
    public Transform Target;

    Vector3 originalPos;

    public void Shake(float _time = 0.1f, float _amount = 0.3f)
    {
        StartCoroutine(ShakeCoroutine(_time, _amount));
    }

    private IEnumerator ShakeCoroutine(float _time, float _amount)
    {
        originalPos = Target.localPosition;

        while (_time > 0)
        {
            Target.localPosition = originalPos + Random.insideUnitSphere * _amount;

            _time -= Time.deltaTime;

            yield return null;
        }

        Target.localPosition = originalPos;
    }
}
