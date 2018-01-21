using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProcessSlotController : MonoBehaviour, IDropHandler
{
	public Image progressImage;

	private Text m_slotLabel;

	private ProcessSlot m_processSlot;
	public ProcessSlot ProcessSlotData
	{
		get
		{
			return m_processSlot;
		}
	}

	private RAMController m_ramController;

	private GameState m_gameState;

	public void SetRAMController( RAMController controller )
	{
		m_ramController = controller;
	}

	public void SetProcessSlot( ProcessSlot slot )
	{
		m_processSlot = slot;

		RefreshView();
	}

	public void OnDrop( PointerEventData pointerEventData )
	{
		if( m_processSlot.process != null )
		{
			return;
		}

		ProcessController processController = pointerEventData.pointerDrag.GetComponent<ProcessController>();
		if( processController != null )
		{
			if( processController.ProcessData.MemoryRequirement <= m_processSlot.size )
			{
				m_processSlot.size -= processController.ProcessData.MemoryRequirement;

				RAM ram = m_ramController.RAMData;

				TimeManager timeManager = GameObject.FindObjectOfType<TimeManager>();
				ram.AddProcessSlot( m_processSlot.index, processController.ProcessData.MemoryRequirement, timeManager.CurrentGameTime + 1, processController.ProcessData );

				Destroy( pointerEventData.pointerDrag );
			}
		}
	}

	private void Awake()
	{
		m_slotLabel = GetComponentInChildren<Text>();

		TimeManager timeManager = GameObject.FindObjectOfType<TimeManager>();
		timeManager.TimerTick += HandleTimerTick;

		m_gameState = GameObject.FindObjectOfType<GameState>();
	}

	private void HandleTimerTick (int tick)
	{
		RefreshView();

		if( m_processSlot.process != null )
		{
			int timePassed = tick - m_processSlot.timeCreated;
			if( timePassed < 0 )
			{
				timePassed = 0;
			}

			/*float ratio = timePassed * 1.0f / m_processSlot.process.BurstTime;

			progressImage.transform.localScale = new Vector3( ratio, 1.0f, 1.0f );*/

			if( timePassed > m_processSlot.process.BurstTime )
			{
				m_ramController.RAMData.RemoveProcessAt( m_processSlot.index );

				m_gameState.NumProcessesCompleted += 1;

				Destroy( gameObject );
			}
		}
	}

	private void Start()
	{
	}

	private void RefreshView()
	{
		TimeManager timeManager = GameObject.FindObjectOfType<TimeManager>();

		if( m_processSlot.process != null )
		{
			int timePassed = timeManager.CurrentGameTime - m_processSlot.timeCreated;
			if( timePassed < 0 )
			{
				timePassed = 0;
			}

			float ratio = timePassed * 1.0f / m_processSlot.process.BurstTime;

			progressImage.transform.localScale = new Vector3( ratio, 1.0f, 1.0f );
		}
		else
		{
			progressImage.transform.localScale = new Vector3( 0.0f, 1.0f, 1.0f );
		}

		if( m_processSlot.process == null )
		{
            m_slotLabel.text = "Free Space (" + m_processSlot.size + "MB)";
		}
		else
		{
			int timePassed = timeManager.CurrentGameTime - m_processSlot.timeCreated;
			if( timePassed < 0 )
			{
				timePassed = 0;
			}

			int timeRemaining = m_processSlot.process.BurstTime - timePassed + 1;
            m_slotLabel.text = m_processSlot.process.Name + " (" + m_processSlot.process.MemoryRequirement + " / " + m_processSlot.size + "MB)\nTime until completion: " + timeRemaining;
		}
	}

	private void OnDestroy()
	{
		TimeManager timeManager = GameObject.FindObjectOfType<TimeManager>();
		if( timeManager != null )
		{
			timeManager.TimerTick -= HandleTimerTick;
		}
	}
}
