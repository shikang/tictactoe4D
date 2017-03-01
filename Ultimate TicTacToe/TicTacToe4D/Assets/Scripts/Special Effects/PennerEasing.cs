using UnityEngine;
using System.Collections;
using System;

public class PennerEasing : MonoBehaviour
{
    /// <summary>
    /// Constant Pi.
    /// </summary>
    private const float PI = Mathf.PI;

    static PennerEasing instance;
    public static PennerEasing Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null)
            throw new System.Exception("You have more than 1 PennerEasing in the scene.");

        Debug.Log("pennereasing created");
        // Initialize the static class variables
        instance = this;
    }
    /// <summary>
    /// Constant Pi / 2.
    /// t is the current delta time 
    /// b is the beginning value
    /// c is the amount to change ( end value - begin value )
    /// d is the entire duration 
    /// </summary>
    /// 
    public float Linear(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        return fChange * fTime / fDuration + fBegin;
    }

    public float LinearInOut(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration * 2;
        if (t < 1)
        {
            return fChange * 2 * fTime / fDuration + fBegin;
        }
        t = t - 1;
        return fChange * 2 * (fDuration - fTime) / fDuration + fBegin;
    }

    public float easeInQuad(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        return fChange * t * t + fBegin;
    }

    public float easeOutQuad(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        return -fChange * t * (t - 2) + fBegin;
    }

    public float easeInOutQuad(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration * 2;
        if (t < 1)
        {
            return fChange / 2 * t * t + fBegin;
        }
        t = t - 1;
        return -fChange / 2 * (t * (t - 2) - 1) + fBegin;
    }

    public float easeInCubic(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        return fChange * t * t * t + fBegin;
    }

    public float easeOutCubic(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        t = t - 1;
        return fChange * (t * t * t + 1) + fBegin;
    }

    public float easeInOutCubic(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration * 2;
        if(t < 1)
        {
            return fChange / 2 * t * t * t + fBegin;
        }
        t = t - 2;
        return fChange / 2 * (t * t * t + 2) + fBegin;
    }

    public float easeInQuart(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        return fChange * t * t * t * t + fBegin;
    }

    public float easeOutQuart(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        t = t - 1;
        return -fChange * (t * t * t * t - 1) + fBegin;
    }

    public float easeInOutQuart(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration * 2;
        if( t < 1 )
        {
            return fChange / 2 * t * t * t * t + fBegin;
        }
        t = t - 2;
        return -fChange / 2 * (t * t * t * t - 2) + fBegin;
    }

    public float easeInQuint(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        return fChange * t * t * t * t * t + fBegin;
    }

    public float easeOutQuint(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        t = t - 1;
        return fChange * (t * t * t * t * t + 1) + fBegin;
    }

    public float easeInOutQuint(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration * 2;
        if(t < 1)
        {
            return fChange / 2 * t * t * t * t * t + fBegin;
        }
        t = t - 2;
        return fChange / 2 * (t * t * t * t * t + 2) + fBegin;
    }

    public float easeInSine(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        return -fChange * Mathf.Cos(t / fDuration * (PI / 2)) + fChange + fBegin;
    }

    public float easeOutSine(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        return fChange * Mathf.Sin(t / fDuration * (PI / 2)) + fBegin;
    }

    public float easeInOutSine(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        return -fChange / 2 * (Mathf.Cos(PI * t / fDuration) - 1) + fBegin;
    }

    public float easeInExpo(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        return fChange * Mathf.Pow(2, 10 * (t / fDuration - 1)) + fBegin;
    }

    public float easeOutExpo(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        return fChange * ( -Mathf.Pow(2, 10 * t / fDuration) + 1) + fBegin;
    }

    public float easeInOutExpo(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration * 2;
        if(t < 1)
        {
            return fChange / 2 * (Mathf.Pow(2, -10 * t) + 2) + fBegin;
        }
        return fChange / 2 * (-Mathf.Pow(2, -10 * t) + 2) + fBegin;
    }

    public float easeInCirc(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        return -fChange * (Mathf.Sqrt(1 - t * t) - 1) + fBegin;
    }

    public float easeOutCirc(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        t = t - 1;
        return fChange * Mathf.Sqrt(1 - t * t) + fBegin;
    }

    public float easeInOutCirc(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration * 2;
        if (t < 1)
        {
            return -fChange / 2 * (Mathf.Sqrt(1 - t * t) + 1) + fBegin;
        }
        return fChange / 2 * (Mathf.Sqrt(1 - t * t) + 1) + fBegin;
    }

    public float easeInBack(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float s = 1.70158f;
        float t = fTime / fDuration;
        return fChange * t * t*((s + 1) * t - s) + fBegin;
    }

    public float easeOutBack(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float s = 1.70158f;
        float t = fTime / fDuration;
        return fChange * ((t - 1) * t * ((s + 1) * t + s) + 1) + fBegin;
    }

    public float easeInOutBack(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float s = 1.70158f;
        float t = fTime / fDuration * 2;
        if (t < 1)
        {
            return fChange / 2 * (t * t *(((s *= 1.525f) + 1) * t - s)) + fBegin;
        }
        return fChange / 2 * ((t - 2) * t * (((s *= 1.525f) + 1) * t + s) + 2) + fBegin;
    }

    public float easeInElastic(float fTime, float fBegin, float fEnd, float fDuration)
    {
        if ((fTime - float.Epsilon) <= 0.0f)
        { 
            return fBegin;
        }

        float t = fTime / fDuration;
        if (t >= 1.0f)
        {
            return fEnd;
        }
        float fChange = fEnd - fBegin;
        float p = fDuration * .3f;
        float a = fChange;
        float s = p / 4;

        float postFix = a * Mathf.Pow(2, 10 * (t -= 1));
        return -(postFix * Mathf.Sin((t * fDuration - s) * (2 * PI) / p)) + fBegin;
    }

    public float easeOutElastic(float fTime, float fBegin, float fEnd, float fDuration)
    {
        if ((fTime - float.Epsilon) <= 0.0f)
        {
            return fBegin;
        }

        float t = fTime / fDuration;
        if (t >= 1.0f)
        {
            return fEnd;
        }

        float fChange = fEnd - fBegin;
        float p = fDuration * .3f;
        float a = fChange;
        float s = p / 4;
        return (a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * fDuration - s) * (2 * PI) / p) + fChange + fBegin);
    }

    public float easeInOutElastic(float fTime, float fBegin, float fEnd, float fDuration)
    {
        if ((fTime - float.Epsilon) <= 0.0f)
        {
            return fBegin;
        }

        float t = fTime / fDuration * 2;
        if (t >= 1.0f)
        {
            return fEnd;
        }

        float fChange = fEnd - fBegin;
        float p = fDuration * (.3f * 1.5f);
        float a = fChange;
        float s = p / 4;

        float postFix = a * Mathf.Pow(2, 10 * (t -= 1));
        if (t < 1)
        { 
            return -.5f * (postFix * Mathf.Sin((t * fDuration - s) * (2 * PI) / p)) + fBegin;
        }
        postFix = a * Mathf.Pow(2, -10 * (t -= 1)); 
        return postFix * Mathf.Sin((t * fDuration - s) * (2 * PI) / p) * .5f + fChange + fBegin;
    }

    public float easeInBounce(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        return fChange - easeOutBounce(fTime, fBegin, fChange, fDuration);
    }

    public float easeOutBounce(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float t = fTime / fDuration;
        float fChange = fEnd - fBegin;
        if (t < (1 / 2.75f))
        {
            return fChange * (7.5625f * t * t) + fBegin;
        }
        else if (t < (2 / 2.75f))
        {
            float postFix = t -= (1.5f / 2.75f);
            return fChange * (7.5625f * (postFix) * t + .75f) + fBegin;
        }
        else if (t < (2.5 / 2.75))
        {
            float postFix = t -= (2.25f / 2.75f);
            return fChange * (7.5625f * (postFix) * t + .9375f) + fBegin;
        }
        else
        {
            float postFix = t -= (2.625f / 2.75f);
            return fChange * (7.5625f * (postFix) * t + .984375f) + fBegin;
        }
    }

    public float easeInOutBounce(float fTime, float fBegin, float fEnd, float fDuration)
    {
        float fChange = fEnd - fBegin;
        float t = fTime / fDuration;
        if (t < fDuration / 2)
        {
            return easeInBounce(t * 2, 0, fChange, fDuration) * .5f + fBegin;
        }
        return easeOutBounce(t * 2 - fDuration, 0, fChange, fDuration) * .5f + fChange * .5f + fBegin;
    }
}
