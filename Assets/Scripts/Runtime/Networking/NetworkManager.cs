using ComradeVanti.CSharpTools;
using UnityEngine;

namespace TeamShrimp.GGJ23.Networking
{
    [RequireComponent(typeof(IncomingCommandHandler))]
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private bool initFromCache;
        [SerializeField] private ITransferLayer.Type transferType;

        private IncomingCommandHandler commandHandler;
        private IOpt<Connection> connection = Opt.None<Connection>();


        private void Awake()
        {
            commandHandler = GetComponent<IncomingCommandHandler>();
            if (initFromCache) InitFromBlackboard();
        }

        public void FixedUpdate()
        {
            connection.Iter(CheckForMessagesFrom);
        }

        public void InitAsHost(ushort port)
        {
            connection = Opt.Some(Connection.AsHost(transferType, port));
        }

        public void InitAsGuest(string ip, ushort port)
        {
            connection = Opt.Some(Connection.AsGuest(transferType, ip, port));
        }

        private void InitFromBlackboard()
        {
            connection = Blackboard.EstablishedConnection;
        }

        public void CacheToBlackboard()
        {
            Blackboard.EstablishedConnection = connection;
        }


        // ReSharper disable once ParameterHidesMember
        private void CheckForMessagesFrom(Connection connection)
        {
            connection.CheckForMessages().Iter(ManageIncomingPacket);
        }

        private void ManageIncomingPacket(byte[] incoming)
        {
            commandHandler.HandleCommand(incoming);
        }

        public void SendCommand(BaseCommand baseCommand, byte channelId = 0)
        {
            connection.Match(it => it.Send(baseCommand, channelId),
                () => Debug.LogError(
                    "Cannot send, no connection initialized!"));
        }
    }
}