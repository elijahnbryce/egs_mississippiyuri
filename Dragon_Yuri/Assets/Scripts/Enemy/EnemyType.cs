using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Enemy
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Custom/EnemyType")]
    public class EnemyType: ScriptableObject
	{
		public enum Element
		{
			Normal = 0,
			Fire = 1,
			Water = 2
		}

        [Header("Elemental")]
        public Element type = Element.Normal;
		public bool critical = false;

		[Tooltip("Type Chart: an elements occurrences in this list stack multiplier [water, water] => 4x multiplier")]
		public List<Element> weaknessList = new(), advantageList = new();
		
		[Tooltip("State Transition: the next stage in elemental transition")]
        public EnemyType wetList;
        public EnemyType hotList; //new in scriptable object error

        [Header("Stats")]
        public float maxHealth = 10f, speed = 1f, strength = 1f, defense = 1f;

		[Header("Characteristics")]
		public int sprite = 0;
		public Color colour = Color.ghostWhite;
    }
}