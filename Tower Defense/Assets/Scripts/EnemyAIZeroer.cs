using UnityEngine;
using System.Collections;

public class EnemyAIZeroer : EnemyAIConfound {	

	protected override void ConfoundWeights(double[] weights, int index){
		weights [index] = 0;
	}
}
