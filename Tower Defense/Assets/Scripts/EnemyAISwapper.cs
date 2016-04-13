﻿using UnityEngine;
using System.Collections;

public class EnemyAISwapper : EnemyAIConfound {	

	protected override void ConfoundWeights(double[] weights, int index){
		weights [index] = -weights[index];
	}
}