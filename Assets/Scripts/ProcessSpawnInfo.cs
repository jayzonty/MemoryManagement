[System.Serializable]
public class ProcessSpawnInfo
{
	public int spawnTime;

	public Process.PriorityType priority = Process.PriorityType.Normal;

	public string processName;
	public bool randomizeName = false;

	public int minBurstTime = 0;
	public int maxBurstTime = 100;
	public bool randomizeBurstTime = true;

	public int minMemoryReq = 1;
	public int maxMemoryReq = 100;
	public bool randomizeMemoryReq = true;

	public int minDeadlineOffset = 0;
	public int maxDeadlineOffset = 50;
	public bool hasDeadline = true;
	public bool randomizeDeadlineOffset = true;
	public bool randomizeHasDeadline = false;

	public bool skip = false;
}
