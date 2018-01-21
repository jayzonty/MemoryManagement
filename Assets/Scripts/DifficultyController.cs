using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public List<RandomProcessSpawnerConstraints> milestoneEffects;

    private int m_milestoneEffectIndex = 0;

    private TimeManager m_timeManager;

    private RandomProcessSpawner m_randomProcessSpawner;

    private void Awake()
    {
        m_timeManager = GameObject.FindObjectOfType<TimeManager>();
        m_timeManager.TimerTick += HandleTimerTick;

        m_randomProcessSpawner = GameObject.FindObjectOfType<RandomProcessSpawner>();
    }

    void HandleTimerTick ( int tick )
    {
        if( m_milestoneEffectIndex >= milestoneEffects.Count )
        {
            m_timeManager.TimerTick -= HandleTimerTick;
            return;
        }

        if( milestoneEffects[m_milestoneEffectIndex].timeOffset == tick )
        {
            m_randomProcessSpawner.SetRandomConstraints( milestoneEffects[m_milestoneEffectIndex++] );
        }
    }
}
