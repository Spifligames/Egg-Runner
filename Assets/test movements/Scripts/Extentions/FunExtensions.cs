using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 300IQ extentions^^
/// </summary>

//    namespace IQ300 
public static class FunExtensions
{
    public static void DoubleScale(this Transform t)
    {
        t.localScale = new Vector3(t.localScale.x * 2, t.localScale.y * 2, t.localScale.z * 2);
    }
}
