using System.Collections;

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
