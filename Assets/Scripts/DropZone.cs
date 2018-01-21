using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	private LayoutGroup m_layoutGroup;

	private void Awake()
	{
		m_layoutGroup = GetComponentInChildren<LayoutGroup>();
	}

	public void OnPointerEnter( PointerEventData pointerEventData )
	{
		Debug.Log( "OnPointerEnter: " + gameObject.name );
	}

	public void OnPointerExit( PointerEventData pointerEventData )
	{
		Debug.Log( "OnPointerExit: " + gameObject.name );
	}

	public void OnDrop( PointerEventData pointerEventData )
	{
		Debug.Log( "OnDrop" );
		Draggable draggable = pointerEventData.pointerDrag.GetComponent<Draggable>();
		if( draggable != null )
		{
			draggable.parentToReturnTo = m_layoutGroup.transform;
		}
	}
}
