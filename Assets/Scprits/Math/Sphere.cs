using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {

    public double x, y, z;
    double R;
    private void Start()
    {
        
    }
    //空间四点确定球心坐标(克莱姆法则)
    void get_xyz(double x1, double y1, double z1,
             double x2, double y2, double z2,
             double x3, double y3, double z3,
             double x4, double y4, double z4,
            out double x, out double y, out double z,float r)
    {
        double a11, a12, a13, a21, a22, a23, a31, a32, a33, b1, b2, b3, d, d1, d2, d3;
        a11 = 2 * (x2 - x1); a12 = 2 * (y2 - y1); a13 = 2 * (z2 - z1);
        a21 = 2 * (x3 - x2); a22 = 2 * (y3 - y2); a23 = 2 * (z3 - z2);
        a31 = 2 * (x4 - x3); a32 = 2 * (y4 - y3); a33 = 2 * (z4 - z3);
        b1 = x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1 + z2 * z2 - z1 * z1;
        b2 = x3 * x3 - x2 * x2 + y3 * y3 - y2 * y2 + z3 * z3 - z2 * z2;
        b3 = x4 * x4 - x3 * x3 + y4 * y4 - y3 * y3 + z4 * z4 - z3 * z3;
        d = a11 * a22 * a33 + a12 * a23 * a31 + a13 * a21 * a32 - a11 * a23 * a32 - a12 * a21 * a33 - a13 * a22 * a31;
        d1 = b1 * a22 * a33 + a12 * a23 * b3 + a13 * b2 * a32 - b1 * a23 * a32 - a12 * b2 * a33 - a13 * a22 * b3;
        d2 = a11 * b2 * a33 + b1 * a23 * a31 + a13 * a21 * b3 - a11 * a23 * b3 - b1 * a21 * a33 - a13 * b2 * a31;
        d3 = a11 * a22 * b3 + a12 * b2 * a31 + b1 * a21 * a32 - a11 * b2 * a32 - a12 * a21 * b3 - b1 * a22 * a31;
        x = d1 / d;
        y = d2 / d;
        z = d3 / d;
        Mathf.Pow((float)(x - x1), 2);
    }
}
