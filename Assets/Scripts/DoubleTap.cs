using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTap : MonoBehaviour
{
    private const float DoubleTapThreshold = 0.3f;
    private int _tapCount;
 
    IEnumerator SingleOrDoubleTap()
    {
        yield return new WaitForSeconds(DoubleTapThreshold);
 
        if (_tapCount == 1)
        {
            Debug.Log("SingleTap");
            _tapCount = 0;
        }
        else if (_tapCount == 2)
        {
            Debug.Log("Double Tap");
            _tapCount = 0;
        }
 
    }
 
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                _tapCount++;
                StartCoroutine(SingleOrDoubleTap());
            }
        }
    }
}
