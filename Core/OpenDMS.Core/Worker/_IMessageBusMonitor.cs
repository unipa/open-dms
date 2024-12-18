namespace OpenDMS.Core.Worker
{
    public  interface IMessageBusMonitor
    {
            public bool StartListenForNewMailMessages();
            public bool StopListenForNewMailMessages();
    }
}
