// namespace BB
// {
//     public sealed class AddBoardContextOnDispose : ProtectedPooledObject<AddBoardContextOnDispose>
//     {
//         AddBoardContext _context;
//         public static AddBoardContextOnDispose GetPooled(in AddBoardContext context)
//         {
//             var result = GetPooledInternal();
//             result._context = context;
//             return result;
//         }
//         public override void Dispose()
//         {
//             _context.Add();
//             _context = default;
//             base.Dispose();
//         }
//     }
// }