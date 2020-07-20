using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyDown : MonoBehaviour
{
    [SerializeField] AnimationCurve flyDownCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField, Min(0f)] float durationInSeconds = 3f;
    [SerializeField] float fromHeight = 5f;
    [SerializeField] float toHeight = 0f;

    private float lerp = 0;
    private float delay = 0f;

    public void Setup(float delay = 0f)
    {
        this.delay = delay;
        this.transform.localScale = Vector3.zero;
        this.transform.localPosition = new Vector3(0f, Mathf.Lerp(fromHeight, toHeight, flyDownCurve.Evaluate(lerp)), 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0f)
        {
            delay -= Time.deltaTime; 
            return;
        }

        this.transform.localScale = Vector3.one;

        if (lerp >= 1f) Destroy(this);

        lerp = Mathf.Clamp(lerp + Time.deltaTime / durationInSeconds, 0f, 1f);

        this.transform.localPosition = new Vector3(0f, Mathf.Lerp(fromHeight, toHeight, flyDownCurve.Evaluate(lerp)), 0f);
    }
}
