using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Enemy
{
	public class EnemyType: ScriptableObject
	{
		public enum Element
		{
			Normal = 0,
			Fire = 1,
			Water = 2
		}

		public Element type = Element.Normal;
		public float maxHealth = 10f, speed = 1f, strength = 1f, defense = 1f;
		public List<Element> weaknessList = new();
		public List<Element> advantageList = new();
	}
}