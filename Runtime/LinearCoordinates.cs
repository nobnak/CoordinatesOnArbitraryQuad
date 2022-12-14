using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace CoordinatesOnArbitraryQuad {

    public class LinearCoordinates {
        private const float EPSILON = 1e-2f;
        public readonly float2 vl0;
        public readonly float2 vr0;
        public readonly float2 vl1;
        public readonly float2 vr1;

        protected float a;

        public LinearCoordinates(float2 vl0, float2 vr0, float2 vl1, float2 vr1) {
            this.vl0 = vl0;
            this.vr0 = vr0;
            this.vl1 = vl1;
            this.vr1 = vr1;
        }

        public float SolveT(float2 p, float t0, float t1, int depth) {
            var t = 0.5f * (t0 + t1);
            if (depth-- <= 0) return t;

            var vl = math.lerp(vl0, vl1, t);
            var vr = math.lerp(vr0, vr1, t);
            var below = isRight(vl, vr, p);
            if (below)
                return SolveT(p, t0, t, depth);
            else
                return SolveT(p, t, t1, depth);
        }
        public float SolveS(float2 p, float t0, float t1, int depth) {
            var t = 0.5f * (t0 + t1);
            if (depth-- <= 0) return t;

            var v0 = math.lerp(vl0, vr0, t);
            var v1 = math.lerp(vl1, vr1, t);
            var below = isRight(v1, v0, p);
            if (below)
                return SolveS(p, t0, t, depth);
            else
                return SolveS(p, t, t1, depth);
        }

        public float Solve1(float2 p, int depth = 10) {
            if (!isRight(vl1, vr1, p)) return 1f;
            if (!isRight(vr0, vl0, p)) return 0f;

            return SolveT(p, 0f, 1f, depth);
        }
        public float2 Solve2(float2 p, int depth = 10) {
            float t, s;
            if (!isRight(vr0, vl0, p))
                t = 0f;
            else if (!isRight(vl1, vr1, p))
                t = 1f;
            else
                t = SolveT(p, 0f, 1f, depth);

            if (!isRight(vl0, vl1, p))
                s = 0f;
            if (!isRight(vr1, vr0, p))
                s = 1f;
            else
                s = SolveS(p, 0f, 1f, depth);
            
            return new float2(s, t);
        }

        public static bool isRight(float2 v0, float2 v1, float2 p) {
            var d = (v1.x - v0.x) * (p.y - v0.y) - (v1.y - v0.y) * (p.x - v0.x);
            return d < 0f;
        }
    }
}
