using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Negi
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Renderer))]
    public class Outline : MonoBehaviour
    {
        public Renderer Renderer { get; private set; }

        public int _color;
        public bool _eraseRenderer;

        [HideInInspector]
        public int _originalLayer;
        [HideInInspector]
        public Material[] _originalMaterials;

        private void Awake()
        {
            Renderer = GetComponent<Renderer>();
        }

        void OnEnable()
        {
			IEnumerable<OutlineEffect> effects = Camera.allCameras.AsEnumerable()
				.Select(c => c.GetComponent<OutlineEffect>())
				.Where(e => e != null);

			foreach (OutlineEffect effect in effects)
            {
                effect.AddOutline(this);
            }
        }

        void OnDisable()
        {
			IEnumerable<OutlineEffect> effects = Camera.allCameras.AsEnumerable()
				.Select(c => c.GetComponent<OutlineEffect>())
				.Where(e => e != null);

			foreach (OutlineEffect effect in effects)
            {
                effect.RemoveOutline(this);
            }
        }
    }
}