using System;
using System.Collections.Generic;

namespace BB
{
    public static class IBoardValuesProviderExtensions
    {
        public static void Add(this IBoardValuesProvider provider, Entity entity, double value = 1)
        {
            if (entity.Has(out IBoard board))
                provider.Add(board, value);
        }
        public static void Add(this IBoardValuesProvider provider, IBoard board, double value = 1)
        {
            provider.Add(new()
            {
                Board = board,
                Value = value
            });
        }
        public static IDisposable AddTemp(this IBoardValuesProvider provider, IBoard board, double value = 1)
        {
            provider.Add(board, value);
            return ApplyBoardValueProvidersOnDispose
                .GetPooled()
                .WithContext(new AddBoardContext
                {
                    Board = board,
                    Value = -value
                })
                .WithProvider(provider);
        }
        public static IDisposable AddTemp(this IBoardValuesProvider provider, Entity entity, double value = 1)
        {
            if (entity.Has(out IBoard board))
                return provider.AddTemp(board, value);
            return null;
        }
        //public static bool CanSpend(this IBoardValuesProvider provider, BoardContext context, List<IBoardKey> errorKeys)
        //{
        //    var newContext = context
        //        .GetPooledCopy()
        //        .WithValue(-context.Value);
        //    var result = provider.CanAdd(newContext, errorKeys);
        //    newContext.Dispose();
        //    return result;
        //}
    }
}