using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessSpawner : MonoBehaviour
{
	public List<string> processNameOptions;

	public List<ProcessSpawnInfo> spawnInfo;

	private ProcessListController m_processListController;

	private void Awake()
	{
		TimeManager timeManager = GameObject.FindObjectOfType<TimeManager>();
		timeManager.TimerTick += HandleTimerTick;

		m_processListController = GameObject.FindObjectOfType<ProcessListController>();

		if( processNameOptions.Count == 0 )
		{
			processNameOptions.Add( "Steam" );
			processNameOptions.Add( "Firefox" );
			processNameOptions.Add( "Chrome" );
			processNameOptions.Add( "Adobe Photoshop" );
			processNameOptions.Add( "Microsoft Word" );
			processNameOptions.Add( "Microsoft Excel" );
			processNameOptions.Add( "Terminal" );
			processNameOptions.Add( "Control Panel" );
			processNameOptions.Add( "Hello World" );
			processNameOptions.Add( "Tr0jan" );
		}
	}

	private void Start()
	{
	}

	private void HandleTimerTick ( int tick )
	{
		for( int i = 0; i < spawnInfo.Count; i++ )
		{
			if( !spawnInfo[i].skip && ( spawnInfo[i].spawnTime == tick ) )
			{
				string processName = spawnInfo[i].processName;
				if( spawnInfo[i].randomizeName )
				{
					int index = Random.Range( 0, processNameOptions.Count );
					processName = processNameOptions[index];
				}

				int burstTime = spawnInfo[i].minBurstTime;
				if( spawnInfo[i].randomizeBurstTime )
				{
					burstTime = Random.Range( spawnInfo[i].minBurstTime, spawnInfo[i].maxBurstTime + 1 );
				}

				int memoryReq = spawnInfo[i].minMemoryReq;
				if( spawnInfo[i].randomizeMemoryReq )
				{
					memoryReq = Random.Range( spawnInfo[i].minMemoryReq, spawnInfo[i].maxMemoryReq + 1 );
				}

				int deadline = 0;
				if( spawnInfo[i].hasDeadline )
				{
					int deadlineOffset = spawnInfo[i].minDeadlineOffset;
					if( spawnInfo[i].randomizeDeadlineOffset )
					{
						deadlineOffset = Random.Range( spawnInfo[i].minDeadlineOffset, spawnInfo[i].maxDeadlineOffset + 1 );
					}

					deadline = tick + deadlineOffset;
				}
				else if( spawnInfo[i].randomizeHasDeadline )
				{
					int roll = Random.Range( 0, 1 );
					if( roll == 1 )
					{
						int deadlineOffset = spawnInfo[i].minDeadlineOffset;
						if( spawnInfo[i].randomizeDeadlineOffset )
						{
							deadlineOffset = Random.Range( spawnInfo[i].minDeadlineOffset, spawnInfo[i].maxDeadlineOffset + 1 );
						}

						deadline = tick + deadlineOffset;
					}
				}

				Process process = new Process( processName, spawnInfo[i].priority, burstTime, memoryReq, tick, deadline );
				m_processListController.CreateProcessCard( process );
			}
		}
	}
}
