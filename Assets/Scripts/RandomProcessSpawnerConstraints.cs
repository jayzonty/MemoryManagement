[System.Serializable]
public class RandomProcessSpawnerConstraints
{
    public int timeOffset;
	public int minSpawnTimeOffset;
	public int maxSpawnTimeOffset;

	public int minProcessesPerSpawn;
	public int maxProcessesPerSpawn;

	public int minBurstTime;
	public int maxBurstTime;

	public int minMemoryRequirement;
	public int maxMemoryRequirement;

	public int minDeadlineOffset;
	public int maxDeadlineOffset;
}
