﻿using NebulaModel.Networking;
using NebulaModel.Packets.Universe;
using NebulaModel.Logger;
using NebulaModel.Packets.Processors;
using NebulaWorld.Universe;
using NebulaModel.Attributes;

namespace NebulaClient.PacketProcessors.Universe
{
    [RegisterPacketProcessor]
    class DysonSphereAddFrameProcessor : IPacketProcessor<DysonSphereAddFramePacket>
    {
        public void ProcessPacket(DysonSphereAddFramePacket packet, NebulaConnection conn)
        {
            Log.Info($"Processing DysonSphere Add Frame notification for system {GameMain.data.galaxy.stars[packet.StarIndex].name} (Index: {GameMain.data.galaxy.stars[packet.StarIndex].index})");
            DysonSphere_Manager.IncomingDysonSpherePacket = true;
            DysonSphereLayer dsl = GameMain.data.dysonSpheres[packet.StarIndex]?.GetLayer(packet.LayerId);
            //Check if target nodes exists (if not, assume that AddNode packet is on the way)
            if (DysonSphere_Manager.CanCreateFrame(packet.NodeAId, packet.NodeBId, dsl)) {
                dsl.NewDysonFrame(packet.ProtoId, packet.NodeAId, packet.NodeBId, packet.Euler);
            }
            else
            {
               DysonSphere_Manager.QueuedAddFramePackets.Add(packet);
            }
            DysonSphere_Manager.IncomingDysonSpherePacket = false;
        }
    }
}
