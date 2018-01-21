using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	public delegate void TimerTickDelegate( int tick );
	public event TimerTickDelegate TimerTick;

	// How much time in real life is equivalent to 1 game tick
	public float secondsPerTick = 1.0f;

	public float timeMultiplier = 1.0f;

	public float maxMultiplier = 4.0f;

	private int m_tick;
	public int CurrentGameTime
	{
		get
		{
			return m_tick;
		}
	}

	private bool m_isPaused = false;

	private float m_tickCountdown = 0.0f;

	public void StartTimer()
	{
		m_tickCountdown = secondsPerTick;

		m_isPaused = false;

		if( TimerTick != null )
		{
			TimerTick( m_tick );
		}
	}

	public void ResetTimer()
	{
		m_tick = 0;
		m_tickCountdown = secondsPerTick;

		if( TimerTick != null )
		{
			TimerTick( m_tick );
		}
	}

	public void PauseTimer()
	{
		m_isPaused = true;
	}

	public void ResumeTimer()
	{
		m_isPaused = false;
	}

    private void Awake()
    {
        GameState gameState = GameObject.FindObjectOfType<GameState>();
        if( gameState != null )
        {
            gameState.GameOverHandlers += HandleGameOver;
        }
    }

    private void HandleGameOver()
    {
        timeMultiplier = 0.0f;
    }

	private void Start()
	{
		StartTimer();
	}

	private void Update()
	{
		if( !m_isPaused )
		{
			if( m_tickCountdown > 0.0f )
			{
				m_tickCountdown -= Time.deltaTime * timeMultiplier;
			}
			else
			{
				m_tick++;
				if( TimerTick != null )
				{
					TimerTick( m_tick );
				}

				m_tickCountdown = secondsPerTick;
			}
		}
	}
}
