using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProcessController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Image deadlineImage;

	public Text processNameText;
	public Text processDetailsText;
	public Text processMemoryText;

	private Process m_processData;
	public Process ProcessData
	{
		get
		{
			return m_processData;
		}
	}

	private Canvas m_gameCanvas;
	private CanvasGroup m_canvasGroup;

	public Transform parentToReturnTo = null;

	private GameState m_gameState;

	private GameObject m_placeHolder = null;

	public void SetProcessData( Process processData )
	{
		m_processData = processData;

		//deadlineImage.transform.localScale = new Vector3( 0.0f, 1.0f, 1.0f );

		UpdateUI();
	}

	public void OnBeginDrag( PointerEventData pointerEventData )
	{
		int siblingIndex = this.transform.GetSiblingIndex();

		m_placeHolder = new GameObject();
		m_placeHolder.transform.SetParent( this.transform.parent );
		m_placeHolder.transform.SetSiblingIndex( siblingIndex );

		RectTransform thisRect = GetComponent<RectTransform>();

		RectTransform rect = m_placeHolder.AddComponent<RectTransform>();
		rect.sizeDelta = thisRect.sizeDelta;

		parentToReturnTo = this.transform.parent;

		this.transform.SetParent( m_gameCanvas.transform );

		m_canvasGroup.blocksRaycasts = false;
	}

	public void OnDrag( PointerEventData pointerEventData )
	{
		this.transform.position = pointerEventData.position;
	}

	public void OnEndDrag( PointerEventData pointerEventData )
	{
		this.transform.SetParent( parentToReturnTo );
		this.transform.SetSiblingIndex( m_placeHolder.transform.GetSiblingIndex() );

		Destroy( m_placeHolder );
		m_placeHolder = null;

		m_canvasGroup.blocksRaycasts = true;
	}

	private void Awake()
	{
		m_gameCanvas = GameObject.FindObjectOfType<Canvas>();
		m_canvasGroup = GetComponent<CanvasGroup>();

		TimeManager timeManager = GameObject.FindObjectOfType<TimeManager>();
		timeManager.TimerTick += HandleTimerTick;

		m_gameState = GameObject.FindObjectOfType<GameState>();
	}

	private void Start()
	{
	}

	private void HandleTimerTick (int tick)
	{
		UpdateUI();

		if( m_processData.Deadline > 0 )
		{
			/*int timePassed = tick - m_processData.TimeCreated;
			if( timePassed < 0 )
			{
				timePassed = 0;
			}

			float ratio = timePassed * 1.0f / ( m_processData.Deadline - m_processData.TimeCreated );

			deadlineImage.transform.localScale = new Vector3( ratio, 1.0f, 1.0f );*/

			if( tick > m_processData.Deadline )
			{
				m_gameState.NumProcessesMissed += 1;

				Destroy( gameObject );
			}
		}
	}

	private void OnDestroy()
	{
		TimeManager timeManager = GameObject.FindObjectOfType<TimeManager>();
		if( timeManager != null )
		{
			timeManager.TimerTick -= HandleTimerTick;
		}

		if( m_placeHolder != null )
		{
			Destroy( m_placeHolder );
		}
	}

	private void UpdateUI()
	{
		TimeManager timeManager = GameObject.FindObjectOfType<TimeManager>();

		if( m_processData.Deadline > 0 )
		{
			int timePassed = timeManager.CurrentGameTime - m_processData.TimeCreated;
			if( timePassed < 0 )
			{
				timePassed = 0;
			}

			float ratio = timePassed * 1.0f / ( m_processData.Deadline - m_processData.TimeCreated );

			deadlineImage.transform.localScale = new Vector3( ratio, 1.0f, 1.0f );
		}
		else
		{
			deadlineImage.transform.localScale = new Vector3( 0.0f, 1.0f, 1.0f );
		}

		processNameText.text = m_processData.Name;
		processMemoryText.text = m_processData.MemoryRequirement + "\nMB";

		if( m_processData.Deadline > 0 )
		{
			int expireTime = m_processData.Deadline - timeManager.CurrentGameTime + 1;
			processDetailsText.text = string.Format( "Processing time:\n{0}\nExpires in:\n{1}", m_processData.BurstTime, expireTime );
		}
		else
		{
			processDetailsText.text = string.Format( "Processing time:\n{0}", m_processData.BurstTime );
		}
	}
}
