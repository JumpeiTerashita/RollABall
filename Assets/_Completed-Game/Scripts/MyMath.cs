using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KTB
{
    /// <summary>
    /// 数学的なやつらをまとめたライブラリ
    /// </summary>
    public class MyMath
    {
        //-----------------------------------------------------------------------
        //! 力pで距離distに物体を投げるときのVec (retがnullの場合は届かない）
        //-----------------------------------------------------------------------
        public static Vector3[] ParabolicVec(float _p, Vector3 _dist)
        {
            float _g = Physics.gravity.y;
            Vector3[] ret = null;
            Vector3 distXZ = new Vector3(_dist.x, 0f, _dist.z);
            float dist_x = distXZ.magnitude;
            float dist_y = _dist.y;
            float a = (_g * dist_x * dist_x) / (2f * _p * _p);
            float b = dist_x / a;
            float c = (a - dist_y) / a;
            float ts = (b * b / 4f) - c;
            if (ts >= 0.0)
            {
                float rt = Mathf.Sqrt(-c + (b * b) / 4f);
                float[] ang = new float[2] { Mathf.Atan((-b / 2f) - rt), Mathf.Atan((-b / 2f) + rt) };
                ret = new Vector3[2];
                for (int i = 0; i < 2; i++)
                {
                    ret[i] = distXZ.normalized * _p * Mathf.Cos(ang[i]);
                    ret[i].y = _p * Mathf.Sin(ang[i]);
                }
            }
            return ret;
        }

        /// <summary>
        /// 第一引数 ＜＝　第二引数　ならtrue
        /// </summary>
        public static bool IsShortLength(float _shorter, float _target)
        {
            if (_shorter * _shorter <= _target * _target) return true;
            else return false;
        }
        /// <summary>
        /// Vector3の長さ ＜＝　float ならtrue
        /// </summary>
        public static bool IsShortLength(Vector3 _shorterVec, float _target)
        {
            if (_shorterVec.sqrMagnitude <= _target * _target) return true;
            else return false;
        }

    }
}