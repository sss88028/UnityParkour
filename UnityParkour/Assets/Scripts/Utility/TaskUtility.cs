using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class TaskUtility
{
	public static IEnumerator AwaitFrames(int frameCount = 1)
	{
		for (var i = 0; i < frameCount; i++)
		{
			yield return frameCount;
		}
	}
}
