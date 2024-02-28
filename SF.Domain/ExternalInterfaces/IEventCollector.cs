using System;
using System.Collections.Generic;

public interface IEventCollector
{
	void CollectEvent(string eventName, DateTime timestamp, Dictionary<string, List<string>> param);
}