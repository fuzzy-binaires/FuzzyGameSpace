using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Helpy
{
    // ----------------------------
    // ---------- VECTOR ----------
    // ----------------------------

    /// converts a Vector3 to a Vector2, ignoring the Vector3.z
    public static Vector2 ToV2(Vector3 v3)
    {
        Vector2 v2 = new Vector2(v3.x, v3.y);
        return v2;
    }

    /// creates a random amplitude Vector2 (using UnityEngine.Range)
    public static Vector2 AmplitudeV2(float amplitude)
    {
        return new Vector2(UnityEngine.Random.Range(-amplitude, amplitude), UnityEngine.Random.Range(-amplitude, amplitude));
    }

    public static Vector3 AmplitudeV3(Vector3 perAxisAmplitude)
    {
        return new Vector3(
            UnityEngine.Random.Range(-perAxisAmplitude.x, perAxisAmplitude.x),
            UnityEngine.Random.Range(-perAxisAmplitude.y, perAxisAmplitude.y),
            UnityEngine.Random.Range(-perAxisAmplitude.z, perAxisAmplitude.z));
    }

    public static Vector3 AmplitudeV3(float amplitude)
    {
        return new Vector3(
            UnityEngine.Random.Range(-amplitude, amplitude),
            UnityEngine.Random.Range(-amplitude, amplitude),
            UnityEngine.Random.Range(-amplitude, amplitude));
    }

    public static Vector3 AmplitudeV3(float amplitudeX, float amplitudeY, float amplitudeZ)
    {
        return new Vector3(
            UnityEngine.Random.Range(-amplitudeX, amplitudeX),
            UnityEngine.Random.Range(-amplitudeY, amplitudeY),
            UnityEngine.Random.Range(-amplitudeZ, amplitudeZ));
    }

    /// creates a new Vector3.multiplier
    public static Vector3 ToV3(float f)
    {
        Vector3 v3 = new Vector3(f, f, f);
        return v3;
    }

    /// converts a float for x and z and a float for z to a Vector3
    public static Vector3 ToV3(float f, float z)
    {
        Vector3 v3 = new Vector3(f, f, z);
        return v3;
    }

    /// converts a Vector2 to a Vector3, the Vector3.z becoming 0
    public static Vector3 ToV3(Vector2 v2, float z = 0f)
    {
        Vector3 v3 = new Vector3(v2.x, v2.y, z);
        return v3;
    }

    /// returns a Vector3 with a new z value (defaults to 0f)
    public static Vector3 ToV3(Vector3 oldV3, float z = 0f)
    {
        Vector3 v3 = new Vector3(oldV3.x, oldV3.y, z);
        return v3;
    }

    /// converts a Vector3 in planar space to a Vector2
    public static Vector2 FromPlanar(this Vector3 v3)
    {
        Vector2 v2 = new Vector2(v3.x, v3.z);
        return v2;
    }

    /// converts a Vector2 to a Vector3 in planar space
    public static Vector3 ToPlanar(this Vector2 v2, float y = 0f)
    {
        Vector3 v3 = new Vector3(v2.x, y, v2.y);
        return v3;
    }

    /// set a Vector3's z-axis to 0f
    public static Vector3 ToPlanar(this Vector3 v3, float y = 0f)
    {
        v3.y = y;
        return v3;
    }

    public static float InverseLerpPerAxis(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    /// returns a multiplication of each of a's and b's x, y, z
    public static Vector3 MultiplyVectors(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }



    // ----------------------------
    // ----------- RECT -----------
    // ----------------------------

    public static bool VectorInsideRect(Vector2 v, Rect r)
    {
        if (v.x > r.x - r.width / 2f && v.x < r.x + r.width / 2f && v.y > r.y - r.height / 2f && v.y < r.y + r.height / 2f)
        {
            return true;
        }
        return false;
    }

    public static bool VectorInsideRectCircle(Vector2 v, Rect r)
    {
        Debug.Log("Range: " + Vector2.Distance(v, new Vector2(r.x, r.y)) + ", below " + r.width);
        return Vector2.Distance(v, new Vector2(r.x, r.y)) < r.width;
    }



    // ----------------------------
    // ----------- PATH -----------
    // ----------------------------

    public static Vector3[] CreateBezierPath(Vector3 start, Vector3 end, float amplitude)
    {
        Vector3 mid = (start + end) / 2f;

        Vector3[] bezierPath = new Vector3[4];

        bezierPath[0] = start;
        bezierPath[1] = mid + ToV3(AmplitudeV2(amplitude));
        bezierPath[2] = mid + ToV3(AmplitudeV2(amplitude));
        bezierPath[3] = end;

        return bezierPath;
    }

    /// Returns the position on a bezierPath, which needs to consist of 4 points.
    public static Vector3 GetBezierPoint(float t, Vector3[] bezierPath)
    {
        if (bezierPath.Length < 4)
        {
            Debug.LogError("BezierPath is out of bounds. Lenght: " + bezierPath.Length);
            return Vector3.zero;
        }

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector3 p = uuu * bezierPath[0];
        p += 3 * uu * t * bezierPath[1];
        p += 3 * u * tt * bezierPath[2];
        p += ttt * bezierPath[3];
        return p;
    }

    public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve, float smoothness)
    {
        List<Vector3> points;
        List<Vector3> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<Vector3>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector3>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints.ToArray());
    }



    // --------------------------------
    // ---------- GAMEOBJECT ----------
    // --------------------------------

    static public GameObject GetFirstChild(this GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    public static GameObject[] GetChildrenWithTag(Transform parent, string tag)
    {
        List<GameObject> taggedChildren = new List<GameObject>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                taggedChildren.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GameObject[] nestedTaggedChildred = GetChildrenWithTag(child, tag);
                foreach (GameObject go in nestedTaggedChildred)
                {
                    taggedChildren.Add(go);
                }
            }
        }

        return taggedChildren.ToArray();
    }



    // --------------------------------
    // ----------- EXTENSION ----------
    // --------------------------------

    public static void Reset(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static void Lerp(this Transform t, Transform from, Transform to, float lerp, bool local = false)
    {
        if (local)
        {
            t.localPosition = Vector3.Lerp(from.localPosition, to.localPosition, lerp);
            t.localRotation = Quaternion.Lerp(from.localRotation, to.localRotation, lerp);
        }
        else
        {
            t.position = Vector3.Lerp(from.position, to.position, lerp);
            t.rotation = Quaternion.Lerp(from.rotation, to.rotation, lerp);
        }
        t.localScale = Vector3.Lerp(from.localScale, to.localScale, lerp);
    }

    public static void Lerp(this Camera c, Camera from, Camera to, float lerp)
    {
        c.fieldOfView = Mathf.Lerp(from.fieldOfView, to.fieldOfView, lerp);
        c.backgroundColor = Color.Lerp(from.backgroundColor, to.backgroundColor, lerp);
        c.farClipPlane = Mathf.Lerp(from.farClipPlane, to.farClipPlane, lerp);
        c.nearClipPlane = Mathf.Lerp(from.nearClipPlane, to.nearClipPlane, lerp);
    }



    // ------------------------------
    // ---------- ALGEBRA -----------
    // ------------------------------

    // tell me if there is an intersection between two line segments
    public static bool SegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        Vector2 a = p2 - p1;
        Vector2 b = p3 - p4;
        Vector2 c = p1 - p3;

        float alphaNumerator = b.y * c.x - b.x * c.y;
        float alphaDenominator = a.y * b.x - a.x * b.y;
        float betaNumerator = a.x * c.y - a.y * c.x;
        float betaDenominator = alphaDenominator; /*2013/07/05, fix by Deniz*/

        bool doIntersect = true;

        if (alphaDenominator == 0 || betaDenominator == 0)
        {
            doIntersect = false;
        }
        else
        {

            if (alphaDenominator > 0)
            {
                if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
            {
                doIntersect = false;
            }

            if (doIntersect && betaDenominator > 0)
            {
                if (betaNumerator < 0 || betaNumerator > betaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (betaNumerator > 0 || betaNumerator < betaDenominator)
            {
                doIntersect = false;
            }
        }

        return doIntersect;
    }

    public static Vector2 GetClosestPointOnLineSegment(Vector2 A, Vector2 B, Vector2 P)
    {
        Vector2 AP = P - A;       //Vector from A to P   
        Vector2 AB = B - A;       //Vector from A to B  

        float magnitudeAB = Vector2.SqrMagnitude(AB);     //Magnitude of AB vector (it's length squared)     
        float ABAPproduct = Vector2.Dot(AP, AB);    //The DOT product of a_to_p and a_to_b     
        float distance = ABAPproduct / magnitudeAB; //The normalized "distance" from a to your closest point  

        if (distance < 0)     //Check if P projection is over vectorAB     
        {
            return A;

        }
        else if (distance > 1)
        {
            return B;
        }
        else
        {
            return A + AB * distance;
        }
    }

    public static bool IsPointInTriangle(Vector3 pt, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        bool b1, b2, b3;

        b1 = sign(pt, v1, v2) < 0.0f;
        b2 = sign(pt, v2, v3) < 0.0f;
        b3 = sign(pt, v3, v1) < 0.0f;

        return ((b1 == b2) && (b2 == b3));
    }

    private static float sign(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }

    // rotates a point around another point (3d positions, 2d rotation around z-axis)
    public static Vector3 RotatePoint(this Vector3 pointToRotate, Vector3 centerPoint, float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * (Mathf.PI / 180);
        float cosTheta = Mathf.Cos(angleInRadians);
        float sinTheta = Mathf.Sin(angleInRadians);
        return new Vector3(
                (cosTheta * (pointToRotate.x - centerPoint.x) -
                sinTheta * (pointToRotate.y - centerPoint.y) + centerPoint.x),

                (sinTheta * (pointToRotate.x - centerPoint.x) +
                cosTheta * (pointToRotate.y - centerPoint.y) + centerPoint.y), 0f
        );
    }

    public static Vector3 GetPositionInCircle(float radius, float angle)
    {
        float ang = angle * 360f;
        Vector3 pos = Vector3.zero;
        pos.x = radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    public static Vector3 GetPositionInCircle(float radius, float angle, float step, bool hasRotationOffset)
    {
        float ang = angle * 360f;
        if (hasRotationOffset)
        {
            ang += (step / 2f) * 360f;
        }
        Vector3 pos = Vector3.zero;
        pos.x = radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }



    // ---------------------------
    // ---------- COLOR ----------
    // ---------------------------

    /// <summary> return a random RGB color </summary>
    public static Color Random(this Color c)
    {
        return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
    }

    /// <summary> returns a random HSB color (with 1f saturation) </summary>
    public static HSBColor Random(this HSBColor h)
    {
        return new HSBColor(UnityEngine.Random.Range(0f, 1f), 1f, 1f);
    }

    /// <summary> returns a random HSB color (set saturation) </summary>
    public static HSBColor Random(this HSBColor h, float saturation)
    {
        return new HSBColor(UnityEngine.Random.Range(0f, 1f), saturation, 1f);
    }

    /// <summary> returns a random HSB color (set saturation and brightness) </summary>
    public static HSBColor Random(this HSBColor h, float saturation, float brightness)
    {
        return new HSBColor(UnityEngine.Random.Range(0f, 1f), saturation, brightness);
    }

    public static HSBColor Random(this HSBColor h, Vector2 minMax)
    {
        return new HSBColor(UnityEngine.Random.Range(minMax.x, minMax.y), 1f, 1f);
    }

    public static Color GetVariation(this Color baseColor, float amplitude = 0.03f)
    {
        HSBColor dropColor = HSBColor.FromColor(baseColor);
        dropColor.h = Mathf.Clamp(dropColor.h + UnityEngine.Random.Range(-amplitude, amplitude), 0f, 1f);

        return dropColor.ToColor();
    }



    // -------------------------
    // ---------- INT ----------
    // -------------------------

    /// returns the bigger of two ints
    public static int Max(int a, int b)
    {
        if (a >= b)
        {
            return a;
        }
        return b;
    }

    /// a clamp function for int
    public static int IntClamp(int value, int min, int max)
    {
        if (value > max)
        {
            return max;
        }
        else if (value < min)
        {
            return min;
        }
        return value;
    }

    /// return a random array index, but not one set as lastIndex
    public static int GetRandomExcludingLastIndex(int arrayLength, int lastIndex)
    {
        if (arrayLength < 2 || lastIndex < 0)
        {
            return 0;
        }

        if (lastIndex < 1)
        {
            return UnityEngine.Random.Range(1, arrayLength);
        }
        else if (lastIndex == arrayLength - 1)
        {
            return UnityEngine.Random.Range(0, arrayLength - 1);
        }
        else
        {
            bool before = false;
            if (UnityEngine.Random.Range(0, 100) > 50)
            {
                before = true;
            }

            if (before)
            {
                return UnityEngine.Random.Range(0, lastIndex);
            }
            else
            {
                return UnityEngine.Random.Range(lastIndex + 1, arrayLength);
            }
        }
    }

    public static bool IsEven(this int i)
    {
        return i % 2 == 0 ? true : false;
    }

    public static bool IsUneven(this int i)
    {
        return i % 2 == 1 ? true : false;
    }



    // ---------------------------
    // ---------- FLOAT ----------
    // ---------------------------

    /// A bezier style smoothing function for lerps (values between 0f and 1f).
    public static float GetSmoothed(this float t)
    {
        return Mathf.Clamp(t * t * t * (t * (6f * t - 15f) + 10f), 0f, 1f);
    }

    public static float GetSmoothed(this float t, int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            t = Mathf.SmoothStep(0f, 1f, t);
        }
        return t;
    }

    public static float ExponentialLerp(float l, int pow)
    {
        return Mathf.Pow(Mathf.Clamp(l, 0f, 1f), pow);
    }

    public static float MiddleLerp_01(float l)
    {
        l = Mathf.Clamp(l, 0f, 1f);
        return 1f - Mathf.Abs(1f - l * 2f);
    }

    public static float MiddleLerp_02(float l)
    {
        l = Mathf.Clamp(l, 0f, 1f);
        if (l < 0.5f)
        {
            return l * 2f;
        }
        else
        {
            return 2f - (l * 2f);
        }
    }

    public static float MiddleLerp_03(float l)
    {
        // 0f		0f
        // 0.25f	0.5f
        // 0.5f		1f
        // 0.75f	0.5f
        // 1f		0f
        if (l < 0.5f)
        {
            return l * 2f;
        }
        else
        {
            return 2f - (l * 2f);
        }
    }

    // scale calculates new range of game
    public static float MapFloat(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    // transpose 0f...1f to -1f...1f
    public static float NegativeNormalize(float f)
    {
        f = Mathf.Clamp(f, 0f, 1f);
        return f * 2f - 1f;
    }



    // ---------------------------
    // ---------- LIST -----------
    // ---------------------------

    /// Shuffle any List so the items are ordered randomly.
    public static List<T> Shuffle<T>(List<T> list)
    {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
        return list;
    }



    // ---------------------------
    // ----------- BOOL ----------
    // ---------------------------

    public static bool[] GetRandomPattern(int bitCount, int bitsTrue)
    {
        //Debug.Log(bitCount);
        //Debug.Log(bitsTrue);
        List<bool[]> patterns = GetAllPatterns(bitCount, bitsTrue);
        //Debug.Log(patterns.Count + " different patterns possible");
        return patterns[UnityEngine.Random.Range(0, patterns.Count)];
    }

    public static List<bool[]> GetAllPatterns(int bitCount, int bitsTrue)
    {
        // prevent logic errors
        if (bitsTrue > bitCount)
        {
            Debug.LogError("bitsTrue (" + bitsTrue + ") must never be greater than bitCount (" + bitCount + ")!");
            bitsTrue = bitCount;
        }

        int integer = (int)Math.Pow(2, bitCount); // calculate necessary max integer for bitshift
        List<bool[]> patterns = new List<bool[]>();

        for (int n = 0; n < integer; n++)
        {
            var bits = new BitArray(BitConverter.GetBytes(n));
            bool[] clampedBits = new bool[bitCount];
            int trueCount = 0;
            for (int b = 0; b < clampedBits.Length; b++)
            {
                clampedBits[b] = bits[b];

                if (bits[b])
                {
                    trueCount++;
                }
            }
            if (trueCount == bitsTrue)
            {
                patterns.Add(clampedBits);
            }
        }

        return patterns;
    }



    // ---------------------------
    // -------- GENERICS ---------
    // ---------------------------

    public static T[] CombineArrays<T>(T[] a1, T[] a2)
    {
        T[] c = new T[a1.Length + a2.Length];

        int p = 0; // pointer
        for (int i = 0; i < a1.Length; i++)
        {
            c[p] = a1[i];
            p++;
        }

        for (int i = 0; i < a2.Length; i++)
        {
            c[p] = a2[i];
            p++;
        }

        return c;
    }



    // ---------------------------
    // ------ List / Array -------
    // ---------------------------

    public static void Subscribe<T>(this List<T> l, T i)
    {
        if (l == null)
        {
            Debug.LogError("Could not Subscribe to List since it was null.");
            return;
        }

        if (!l.Contains(i))
        {
            l.Add(i);
        }
    }

    public static void Unsubscribe<T>(this List<T> l, T i)
    {
        if (l == null || l.Count == 0)
        {
            //Debug.LogError("Could not Unsubscribe from List since it was null or empty.");
            return;
        }

        if (l.Contains(i))
        {
            l.Remove(i);
        }
    }

    public static List<T> Combine<T>(params List<T>[] lists)
    {
        List<T> combined = new List<T>();
        for (int i = 0; i < lists.Length; i++)
        {
            combined.AddRange(lists[i]);
        }
        return combined;
    }

    public static bool Contains<T>(this T[] arr, T t)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].Equals(t)) return true;
        }
        return false;
    }

    /// Returns all Type T of a List<T> container that contains a List<T> entities.
    public static T[] GetContained<T>(T[] container, T[] entities)
    {
        List<T> c = new List<T>();
        for (int i = 0; i < entities.Length; i++)
        {
            if (container.Contains(entities[i]))
            {
                c.Add(entities[i]);
            }
        }
        return c.ToArray();
    }

    /// Returns all GameObjects of a group that have a Component / interface T attached to them.
    public static GameObject[] GetGameObjectsWithComponent<T>(GameObject[] group)
    {
        List<GameObject> c = new List<GameObject>();
        for (int i = 0; i < group.Length; i++)
        {
            if (group[i].GetComponent<T>() != null)
            {
                c.Add(group[i]);
            }
        }
        return c.ToArray();
    }

    [Obsolete("GetGameObjectsWithComponent<T> does the same but simpler.")]
    public static Component[] GetComponentsWithInterface<T>(GameObject[] group)
    {
        List<Component> viableComponents = new List<Component>();

        for (int i = 0; i < group.Length; i++)
        {
            Component[] components = group[i].GetComponents<Component>();
            for (int c = 0; c < components.Length; c++)
            {
                Type[] interfaces = components[c].GetType().GetInterfaces();
                for (int f = 0; f < interfaces.Length; f++)
                {
                    Debug.Log(interfaces[f].Name + " on " + group[i].name);
                    if (interfaces[f] == typeof(T))
                    {
                        viableComponents.Add(components[c]);
                        break;
                    }
                }
            }
        }

        return viableComponents.ToArray();
    }

    public static string Log<T>(this List<T> l)
    {
        string s = "";

        if (l == null || l.Count == 0)
        {
            return "null or empty";
        }

        s += l.GetType() + " List: ";

        for (int i = 0; i < l.Count; i++)
        {
            s += i + "(" + l[i] + ") ";
        }

        return s;
    }

    /// Log an array of generics in the console.
    public static void DebugLogArray<T>(T[] array, string arrayName = "")
    {
        string s = "";

        if (arrayName == "")
        {
            s += array.GetType() + " array: ";
        }
        else
        {
            s += arrayName + ": ";
        }

        for (int i = 0; i < array.Length; i++)
        {
            s += i + "(" + array[i] + ") ";
        }
        Debug.Log(s);
    }

    public static string LogListOfBoolArrays(List<bool[]> list)
    {
        string s = "";

        for (int i = 0; i < list.Count; i++)
        {
            for (int b = list[i].Length - 1; b >= 0; b--)
            {
                s += list[i][b] ? 1 : 0;
            }

            if (i != list.Count - 1)
            {
                s += Environment.NewLine;
            }
        }

        return s;
    }

    public static string Abbreviate(this int c)
    {
        if (c < 0)
        {
            return "0";
        }

        if (c > 10000000)
        {
            c /= 1000000;
            return c.ToString() + "m";
        }
        else if (c > 1000000)
        {
            c /= 100000;
            return ((float)c / 10f).ToString() + "m";
        }
        else if (c > 10000)
        {
            c /= 1000;
            return c.ToString() + "k";
        }
        else if (c > 1000)
        {
            c /= 100;
            return ((float)c / 10f).ToString() + "k";
        }
        else
        {
            return c.ToString();
        }
    }



    public static void SetIndividualColor(this Projector projector, string keyword, Color color)
    {
        var mat = new Material(projector.material);
        if (!mat.name.Contains("(Instance)"))
            mat.name += " (Instance)";

        mat.SetColor(keyword, color);

        projector.material = mat;
    }

    public static void SetIndiviualFloat(this Projector projector, string keyword, float f)
    {
        var mat = new Material(projector.material);
        if (!mat.name.Contains("(Instance)"))
            mat.name += " (Instance)";

        mat.SetFloat(keyword, f);

        projector.material = mat;
    }



    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }





    public static void FaceCameraFixedScale(this Transform t, Camera referenceCamera, float relativeLocalScale)
    {
        Vector3 screenSpace = referenceCamera.WorldToScreenPoint(t.position);
        Vector3 adjustedScreenSpace = new Vector3(screenSpace.x + 100f, screenSpace.y, screenSpace.z);
        Vector3 adjustedWorldSpace = referenceCamera.ScreenToWorldPoint(adjustedScreenSpace);
        t.localScale = Vector3.one * (t.position - adjustedWorldSpace).magnitude * relativeLocalScale;
        t.rotation = referenceCamera.transform.rotation;
    }
}