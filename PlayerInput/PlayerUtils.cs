using BB.Di;
using BB.Map;
using System;
using UnityEngine;

namespace BB
{
    public readonly struct PlayerSpawnContext
    {
        public TransformOperation? Transform { get; init; }
        public MapPoint Point { get; init; }
    }
    public static class PlayerUtils
    {
        public static Entity SpawnPlayer(in PlayerSpawnContext context)
        {
            var t = new TransformOperation
            {
                CopySource = context.Transform ?? default,
                DoNotDestroyOnLoad = true
            };
            var player = World.Require<Player>();
            if (player)
            {
                t.Apply(player);
            }
            else
            {
                var installer = World.Require<PlayerInstaller>();
                player.Value = Entity.Spawn(new SpawnEntityFromInstaller3DContext
                {
                    Installer = installer,
                    Transform = t
                });
            }
            if (context.Point)
                PlayerUtils.WarpPlayerToPoint(context.Point,player.Value.Require<Root>().ForwardFlat);

            return player;
        }
        public static IDisposable SwitchToCursorMode(in DataSourceDesc source)
        {
            return DisposableBag.GetPooled()
                .Add(World.Require<PlayerInputBlocked>().Push(new()
                {
                    Value = true,
                    Source = source
                }))
                .Add(World.Require<PlayerCameraMovementBlocked>().Push(new()
                {
                    Value = true,
                    Source = source
                }))
                .Add(World.Require<PlayerMovementBlocked>().Push(new()
                {
                    Value = true,
                    Source = source
                }))
                .Add(World.Require<MouseCursorVisible>().Push(new()
                {
                    Value = true,
                    Source = source
                }));
        }
        public static void WarpPlayerToPoint(MapPoint point, Vector3 viewDir)
        {
            var player = World.Require<Player>();
            player.Require<Root>().Position = point.transform.position;
            player.Require<PlayerCamera>().Value.transform.forward = viewDir;
            player.Require<CurrentPlayerPosition>().Value = new()
            {
                Point = point
            };
        }
    }
}