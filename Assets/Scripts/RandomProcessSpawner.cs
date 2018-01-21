using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomProcessSpawner : MonoBehaviour
{
	public List<string> processNames;

    public int initialNumProcesses = 2;

	public int minSpawnTimeOffset = 5;
	public int maxSpawnTimeOffset = 10;

	public int minProcessesPerSpawn = 1;
	public int maxProcessesPerSpawn = 2;

	public int minMemoryRequirement = 20;
	public int maxMemoryRequirement = 50;

	public int minBurstTime = 10;
	public int maxBurstTime = 20;

	public int minDeadlineOffset = 20;
	public int maxDeadlineOffset = 30;

	public int initialCooldown = 5;

	private int m_nextSpawnTime;

	private ProcessListController m_processListController;
    private GameState m_gameState;

	public void SetRandomConstraints( RandomProcessSpawnerConstraints constraints )
	{
		minSpawnTimeOffset = constraints.minSpawnTimeOffset;
		maxSpawnTimeOffset = constraints.maxSpawnTimeOffset;

		minProcessesPerSpawn = constraints.minProcessesPerSpawn;
		maxProcessesPerSpawn = constraints.maxProcessesPerSpawn;

		minMemoryRequirement = constraints.minMemoryRequirement;
		maxMemoryRequirement = constraints.maxMemoryRequirement;

		minBurstTime = constraints.minBurstTime;
		maxBurstTime = constraints.maxBurstTime;

		minDeadlineOffset = constraints.minDeadlineOffset;
		maxDeadlineOffset = constraints.maxDeadlineOffset;

		TimeManager timeManager = GameObject.FindObjectOfType<TimeManager>();
		// Recalculate next spawn time
		m_nextSpawnTime = timeManager.CurrentGameTime + Random.Range( minSpawnTimeOffset, maxSpawnTimeOffset ) + 1;
	}

	private void Awake()
	{
		m_processListController = GameObject.FindObjectOfType<ProcessListController>();

		TimeManager timeManager = GameObject.FindObjectOfType<TimeManager>();
		timeManager.TimerTick += HandleTimerTick;

        m_gameState = GameObject.FindObjectOfType<GameState>();
	}

	private void Start()
	{
		m_nextSpawnTime = initialCooldown;

        SpawnProcesses( initialNumProcesses );
	}

	private void HandleTimerTick( int tick )
	{
		if( m_nextSpawnTime == tick )
		{
            if( m_processListController.NumProcesses >= m_gameState.MaxProcessesInList )
            {
                m_nextSpawnTime++;

                return;
            }

            SpawnProcesses();

            m_nextSpawnTime = tick + Random.Range( minSpawnTimeOffset, maxSpawnTimeOffset + 1 );
		}
	}

    private void SpawnProcesses()
    {
        int numProcessesToSpawn = Random.Range( minProcessesPerSpawn, maxProcessesPerSpawn + 1 );
        if( m_processListController.NumProcesses + numProcessesToSpawn > m_gameState.MaxProcessesInList )
        {
            numProcessesToSpawn = m_gameState.MaxProcessesInList - m_processListController.NumProcesses;
        }
             
        SpawnProcesses( numProcessesToSpawn );
    }

    private void SpawnProcesses( int numProcesses )
    {
        TimeManager timeManager = GameObject.FindObjectOfType<TimeManager>();

        for( int i = 0; i < numProcesses; i++ )
        {
            string name = processNames[Random.Range( 0, processNames.Count )];

            int burstTime = Random.Range( minBurstTime, maxBurstTime + 1 );
            int memoryReq = Random.Range( minMemoryRequirement, maxMemoryRequirement + 1 );

            int deadlineRoll = Random.Range( 0, 2 );
            int deadlineOffset = 0;
            if( deadlineRoll == 0 )
            {
                deadlineOffset = Random.Range( minDeadlineOffset, maxDeadlineOffset );

                Process p = new Process( name, Process.PriorityType.Normal, burstTime, memoryReq, timeManager.CurrentGameTime, timeManager.CurrentGameTime + deadlineOffset );
                m_processListController.CreateProcessCard( p );
            }
            else
            {
                Process p = new Process( name, Process.PriorityType.Normal, burstTime, memoryReq, timeManager.CurrentGameTime );
                m_processListController.CreateProcessCard( p );
            }
        }
    }
}
