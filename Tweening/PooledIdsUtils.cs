using System.Collections.Generic;

public static class PooledIdsUtils
{
	static readonly Dictionary<ulong, object> _ids = new();
	static ulong _lastId = 0;
	public static ulong AddToPool(object obj)
	{
		_ids.Add(_lastId, obj);
		_lastId++;
		return _lastId - 1;
	}
	public static bool IsValid(object obj, ulong id)
		=> _ids.TryGetValue(id, out var value) && obj == value;
	public static bool RemoveFromPool(object obj, ulong id)
	{
		if (IsValid(obj, id))
		{
			_ids.Remove(id);
			return true;
		}
		return false;
	}
}
