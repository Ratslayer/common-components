using System;
using System.Collections.Generic;

namespace BB.Serialized.Board.Conditions
{
	[Serializable]
	public abstract class HasTags<TagType> : ISerializedValueCondition
		where TagType : IBoardKey
	{
		public TagSource _source = TagSource.Source;
		public List<TagType> _tags = new();
		public bool IsValid(BoardContext context)
		{
			if (_source.HasFlag(TagSource.Source))
				return context.Tags.ContainsAll(_tags);
			if (_source.HasFlag(TagSource.Target))
				return context.TargetTags.ContainsAll(_tags);
			return false;
		}
	}
	[Flags]
	public enum TagSource
	{
		None = 0,
		Source = 1,
		Target = 2
	}
}