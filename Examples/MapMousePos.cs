using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace CoordinatesOnArbitraryQuad.Examples {

    [ExecuteAlways]
    public class MapMousePos : MonoBehaviour {

        public Dependency dep = new Dependency();

        protected LinearMap map;

        #region unity
        private void OnEnable() {
            if (dep.inputs.Length < 3) {
                enabled = false;
                return;
            }

            var x0 = (float3)dep.inputs[0].position;
            var x1 = (float3)dep.inputs[1].position;
            var x2 = (float3)dep.inputs[2].position;
            var x3 = (float3)dep.inputs[3].position;
            map = new LinearMap(x0.xy, x3.xy, x1.xy, x2.xy);
        }
        private void Update() {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (dep.surface.Raycast(ray, out var hit, float.MaxValue)) {
                var p = ((float3)hit.point).xy;
                var t = map.Solve2(p);
                if (dep.output != null) dep.output.localPosition = new Vector3(t.x-0.5f, t.y-0.5f, 0f);
            }
        }
        #endregion

        #region declarations
        [System.Serializable]
        public class Dependency {
            public Collider surface;
            public Transform[] inputs;
            public Transform output;
        }
        #endregion
    }
}
