﻿namespace TeamShrimp.GGJ23.Runtime.Networking
{
    public interface ITransferLayer
    {
        public void SetServer(bool isServer);
        public void SetConnectionInfo(string ip, ushort port);
        public byte[] NetUpdate();
        public void CreateClient();
        public void CreateServer();
        public void StopNetworkConnection();

        public void Send(BaseCommand baseCommand, byte channelId = 0);

    }
}