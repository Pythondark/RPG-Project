using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayableDirector : MonoBehaviour
{

    public event Action<float> onFinish;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("OnFinish", 3f);
    }

    void OnFinish()
    {
        onFinish(34.0f);
    }
}
