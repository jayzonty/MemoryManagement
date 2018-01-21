using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessSlot
{
	public int index;
	public int size;
	public int timeCreated;

	public Process process;

	public ProcessSlot()
	{
		index = -1;
		size = 0;

		process = null;
	}

	public ProcessSlot( ProcessSlot slot )
	{
		index = slot.index;
		size = slot.size;

		process = slot.process;
	}
}
