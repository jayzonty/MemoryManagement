using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public GameObject gameOverInfo;
    public GameObject compactionInfo;

	public delegate void StatsChangedDelegate( GameState source );
	public event StatsChangedDelegate StatsChanged;

	public delegate void GameOverDelegate();
	public event GameOverDelegate GameOverHandlers;

	public delegate void GameWinDelegate();
	public event GameWinDelegate GameWinHandlers;

	public Transform compactionChargesContainer;
	public GameObject compactionChargeIconPrefab;
	public Button compactionButton;

	public int finalRamSize = 1024;

    private int m_maxProcessesInList = 5;
    public int MaxProcessesInList
    {
        get
        {
            return m_maxProcessesInList;
        }
    }

	private int m_numProcessesCompleted = 0;
	public int NumProcessesCompleted
	{
		get
		{
			return m_numProcessesCompleted;
		}

		set
		{
			m_numProcessesCompleted = value;

			if( ( m_numProcessesCompleted > 0 ) && ( m_numProcessesCompleted % 10 == 0 ) )
			{
				RAMController ramController = GameObject.FindObjectOfType<RAMController>();
				if( ramController.RAMData.MaxSize == finalRamSize )
				{
					// Congratulations for now
					if( GameWinHandlers != null )
					{
						GameWinHandlers();
					}
				}
				else
				{
					ramController.RAMData.MaxSize *= 2;

                    m_numProcessesMissedLimit += 5;

                    m_maxProcessesInList++;
				}
			}

            if( StatsChanged != null )
            {
                StatsChanged( this );
            }
		}
	}

	private int m_numProcessesMissed = 0;
	public int NumProcessesMissed
	{
		get
		{
			return m_numProcessesMissed;
		}

		set
		{
			m_numProcessesMissed = value;

			if( StatsChanged != null )
			{
				StatsChanged( this );
			}

			if( m_numProcessesMissed == m_numProcessesMissedLimit )
			{
				// Game over
				if( GameOverHandlers != null )
				{
					GameOverHandlers();
				}

                if( compactionInfo != null )
                {
                    compactionInfo.SetActive( false );
                }

                if( gameOverInfo != null )
                {
                    gameOverInfo.SetActive( true );
                }
			}
		}
	}

	private int m_numCompactionCharges = 0;
	public int NumCompactionCharges
	{
		get
		{
			return m_numCompactionCharges;
		}

		set
		{
			m_numCompactionCharges = value;
			if( m_numCompactionCharges < 0 )
			{
				m_numCompactionCharges = 0;
			}
			else if( m_numCompactionCharges >= m_maxCompactionCharges )
			{
				m_numCompactionCharges = m_maxCompactionCharges;
			}

			compactionButton.interactable = ( m_numCompactionCharges > 0 );

			int childCount = compactionChargesContainer.childCount;
			if( m_numCompactionCharges < childCount )
			{
				for( int i = childCount - 1; i >= m_numCompactionCharges; i-- )
				{
					Destroy( compactionChargesContainer.GetChild( i ).gameObject );
				}
			}
			else
			{
				for( int i = childCount; i < m_numCompactionCharges; i++ )
				{
					Instantiate( compactionChargeIconPrefab, compactionChargesContainer );
				}
			}
		}
	}

	private int m_maxCompactionCharges;
	public int MaxCompactionCharges
	{
		get
		{
			return m_maxCompactionCharges;
		}

		set
		{
			m_maxCompactionCharges = value;
		}
	}

	private int m_numProcessesMissedLimit;
	public int NumProcessesMissedLimit
	{
		get
		{
			return m_numProcessesMissedLimit;
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
        m_numProcessesMissedLimit = 5;

		if( StatsChanged != null )
		{
			StatsChanged( this );
		}

		NumCompactionCharges = MaxCompactionCharges = 5;

        if( gameOverInfo != null )
        {
            gameOverInfo.SetActive( false );
        }
	}

	public void PlayGame()
	{
		SceneManager.LoadScene( "Game" );
	}
}
