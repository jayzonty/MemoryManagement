using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TimerLabel : MonoBehaviour
{
	private TimeManager m_timeManager;

	private Text m_text;

	private void Awake()
	{
		m_timeManager = GameObject.FindObjectOfType<TimeManager>();
		m_timeManager.TimerTick += HandleTimerTick;

		m_text = GetComponent<Text>();
	}

	private void Start()
	{
	}

	private void HandleTimerTick( int tick )
	{
		//m_text.text = "Time: " + tick;
	}

	private void Update()
	{
		m_text.text = "Time: " + m_timeManager.CurrentGameTime + " (x" + (int)m_timeManager.timeMultiplier + ")";
	}
}
