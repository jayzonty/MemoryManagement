using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Canvas m_gameCanvas;
	private CanvasGroup m_canvasGroup;

	public Transform parentToReturnTo = null;

	private void Awake()
	{
		m_gameCanvas = GameObject.FindObjectOfType<Canvas>();
		m_canvasGroup = GetComponent<CanvasGroup>();
	}

	public void OnBeginDrag( PointerEventData pointerEventData )
	{
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

		m_canvasGroup.blocksRaycasts = true;
	}
}
