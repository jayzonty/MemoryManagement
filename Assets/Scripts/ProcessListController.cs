using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ProcessListController : MonoBehaviour
{
	public GameObject processCardPrefab;

	private LayoutGroup m_layoutGroup;

	private void Awake()
	{
		m_layoutGroup = GetComponent<LayoutGroup>();
	}

	private void Start()
	{
	}

	public int NumProcesses
	{
		get
		{
			return m_layoutGroup.transform.childCount;
		}
	}

	public void CreateProcessCard( Process process )
	{
		GameObject processCard = Instantiate( processCardPrefab, m_layoutGroup.transform );

		ProcessController processController = processCard.GetComponent<ProcessController>();
		processController.SetProcessData( process );
	}
}
