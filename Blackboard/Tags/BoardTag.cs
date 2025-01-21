using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BB
{
	public sealed class BoardTag : BaseBoardKey
	{
		[SerializeField]
		TagType[] _types = { };

		public IReadOnlyList<TagType> Types => _types;
		public override IBoardValueContainer CreateValue(IBoard board)
		{
			var result = base.CreateValue(board);
			if (board.Entity.Has(out BoardTags tags))
				tags.Add(result);
			return result;
		}
	}

	public static class BoardTagExtensions
	{
		public static bool HasType(this BoardTag tag, TagType type)
			=> tag && tag.Types.Contains(type);

		public static PooledList<BoardTag> GetActiveTags(this IEnumerable<TagType> tags, in Entity entity)
		{
			if (entity.Has(out BoardTags container))
				return container.GetRelevantTagsFromContainer(tags);
			return PooledList<BoardTag>.GetPooled();
		}

		static PooledList<BoardTag> GetRelevantTagsFromContainer(this BoardTags tagsContainer, IEnumerable<TagType> relevantTags)
		{
			var tags = PooledList<BoardTag>.GetPooled();

			foreach (var type in relevantTags)
				foreach (var container in tagsContainer.Elements)
				{
					if (container.Key is not BoardTag tag)
						continue;

					if (!tag.Types.Contains(type))
						continue;

					if (container.GetValue() >= 1)
						tags.Add(tag);
				}

			return tags;
		}
	}
}