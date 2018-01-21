using System.Collections;
using System.Collections.Generic;

public class RAM
{
	public delegate void ProcessSlotsChangedDelegate();
	public event ProcessSlotsChangedDelegate ProcessSlotsChanged;

	private List<ProcessSlot> m_processSlots;

	private int m_maxSize;
	public int MaxSize
	{
		get
		{
			return m_maxSize;
		}

		set
		{
			int expandedMemory = value - m_maxSize;

			AddProcessSlot( m_processSlots.Count, expandedMemory, 0, null );

			m_maxSize = value;

			if( ProcessSlotsChanged != null )
			{
				ProcessSlotsChanged();
			}
		}
	}

	public RAM( int maxSize )
	{
		m_maxSize = maxSize;

		Init();
	}

	public void Init()
	{
		m_processSlots = new List<ProcessSlot>();

		ProcessSlot initialSlot = new ProcessSlot();
		initialSlot.index = 0;
		initialSlot.size = m_maxSize;
		initialSlot.process = null;

		m_processSlots.Add( initialSlot );

		if( ProcessSlotsChanged != null )
		{
			ProcessSlotsChanged();
		}
	}

	public void AddProcessSlot( int index, int size, int timeCreated, Process process )
	{
		ProcessSlot slot = new ProcessSlot();
		slot.index = index;
		slot.size = size;
		slot.timeCreated = timeCreated;
			
		slot.process = process;

		m_processSlots.Insert( index, slot );

		for( int i = 0; i < m_processSlots.Count; i++ )
		{
			m_processSlots[i].index = i;
		}

		if( ProcessSlotsChanged != null )
		{
			ProcessSlotsChanged();
		}
	}

	public void RemoveProcessAt( int index )
	{
		m_processSlots[index].process = null;

		for( int i = m_processSlots.Count - 1; i > 0; i-- )
		{
			if( m_processSlots[i].process == null )
			{
				if( m_processSlots[i - 1].process == null )
				{
					m_processSlots[i - 1].size += m_processSlots[i].size;

					m_processSlots.RemoveAt( i );
				}
			}
		}

		for( int i = 0; i < m_processSlots.Count; i++ )
		{
			m_processSlots[i].index = i;
		}

		if( ProcessSlotsChanged != null )
		{
			ProcessSlotsChanged();
		}
	}

	public void Compact()
	{
		int totalFreeMemory = 0;
		for( int i = m_processSlots.Count - 1; i >= 0; i-- )
		{
			if( m_processSlots[i].process == null )
			{
				totalFreeMemory += m_processSlots[i].size;

				m_processSlots.RemoveAt( i );
			}
		}

		int mid = (int)( ( m_processSlots.Count + 1 ) / 2 );

		ProcessSlot freeSlot = new ProcessSlot();
		freeSlot.size = totalFreeMemory;

		m_processSlots.Insert( mid, freeSlot );

		for( int i = 0; i < m_processSlots.Count; i++ )
		{
			m_processSlots[i].index = i;
		}

		if( ProcessSlotsChanged != null )
		{
			ProcessSlotsChanged();
		}
	}

	public List<ProcessSlot> GetProcessSlots()
	{
		return m_processSlots;
	}
}
