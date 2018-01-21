using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RAMController : MonoBehaviour
{
	public GameObject processSlotPrefab;

	public int initialRamSize = 128;

	private LayoutGroup m_layoutGroup;
	private RectTransform m_layoutGroupRectTransform;

	private RAM m_ramData;
	public RAM RAMData
	{
		get
		{
			return m_ramData;
		}
	}

	public void Compact()
	{
		GameState gameState = GameObject.FindObjectOfType<GameState>();

		if( gameState.NumCompactionCharges > 0 )
		{
			m_ramData.Compact();

			gameState.NumCompactionCharges -= 1;
		}
	}

	private void Awake()
	{
		m_layoutGroup = GetComponentInChildren<LayoutGroup>();
		m_layoutGroupRectTransform = m_layoutGroup.GetComponent<RectTransform>();
	}

	private void Start()
	{
		m_ramData = new RAM( initialRamSize );
		m_ramData.ProcessSlotsChanged += RAMProcessSlotsChanged;

		m_ramData.Init();
	}

	private void RAMProcessSlotsChanged()
	{
		foreach ( Transform child in m_layoutGroup.transform )
		{
			Destroy( child.gameObject );
		}

		List<ProcessSlot> processSlots = m_ramData.GetProcessSlots();
		foreach ( ProcessSlot processSlot in processSlots )
		{
			if( processSlot.size == 0 )
			{
				continue;
			}

			GameObject processSlotInstance = Instantiate( processSlotPrefab, m_layoutGroup.transform );

			ProcessSlotController slotController = processSlotInstance.GetComponent<ProcessSlotController>();
			slotController.SetProcessSlot( processSlot );
			slotController.SetRAMController( this );

			float sizeRatio = slotController.ProcessSlotData.size * 1.0f / m_ramData.MaxSize;

			RectTransform rectTransform = processSlotInstance.GetComponent<RectTransform>();
			Vector2 sizeDelta = rectTransform.sizeDelta;
			sizeDelta.x = m_layoutGroupRectTransform.rect.width;
			sizeDelta.y = m_layoutGroupRectTransform.rect.height * sizeRatio;
			rectTransform.sizeDelta = sizeDelta;
		}
	}
}
