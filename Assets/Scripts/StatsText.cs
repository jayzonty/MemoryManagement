using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour
{
	private Text m_text;

	private void Awake()
	{
		m_text = GetComponent<Text>();

		GameState gameState = GameObject.FindObjectOfType<GameState>();
		gameState.StatsChanged += StatsChanged;
	}

	private void StatsChanged ( GameState source )
	{
        m_text.text = string.Format( "Processes Completed: {0}\nProcesses Missed: {1}/{2}", source.NumProcessesCompleted, source.NumProcessesMissed, source.NumProcessesMissedLimit );
	}
}
