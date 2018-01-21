using System;

public class Process
{
	public enum PriorityType
	{
		Normal, Critical
	}

	private string m_name;
	public string Name
	{
		get
		{
			return m_name;
		}
	}

	private PriorityType m_priority;
	public PriorityType Priority
	{
		get
		{
			return m_priority;
		}
	}

	private int m_burstTime;
	public int BurstTime
	{
		get
		{
			return m_burstTime;
		}
	}

	private int m_memoryRequirement;
	public int MemoryRequirement
	{
		get
		{
			return m_memoryRequirement;
		}
	}

	private int m_timeCreated;
	public int TimeCreated
	{
		get
		{
			return m_timeCreated;
		}
	}

	private int m_deadline;
	public int Deadline
	{
		get
		{
			return m_deadline;
		}
	}

	public Process( string name, PriorityType priority, int burstTime, int memoryRequirement, int timeCreated )
		: this( name, priority, burstTime, memoryRequirement, timeCreated, 0 )
	{}

	public Process( string name, PriorityType priority, int burstTime, int memoryRequirement, int timeCreated, int deadline )
	{
		m_name = name;
		m_priority = priority;

		m_burstTime = burstTime;
		m_memoryRequirement = memoryRequirement;

		m_deadline = deadline;

		m_timeCreated = timeCreated;
	}

	public Process( Process process )
	{
		m_name = process.Name;
		m_priority = process.Priority;

		m_burstTime = process.BurstTime;
		m_memoryRequirement = process.MemoryRequirement;

		m_deadline = process.Deadline;

		m_timeCreated = process.TimeCreated;
	}
}
