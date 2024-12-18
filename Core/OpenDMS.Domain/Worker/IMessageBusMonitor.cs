namespace OpenDMS.Domain.Worker
{
    public  interface IMessageBusMonitor
    {
            public bool StartListenForNewMailMessages();
            public bool StopListenForNewMailMessages();
    }
}
