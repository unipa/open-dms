namespace MessageBus.Interface
{

    public class FakeMessaging<T> : IMessaging<T>
    {
        public event MessageReceived<T> OnMessageReceived;

        public T BasicGetSingleMessage(string queue)
        {
            return default!;
        }

        public void Dispose()
        {
        }

        public T GetSingleMessage(string queue, Action<T> beforeAck)
        {
            return default!;
        }

        public async Task Listening(string queue)
        {
        }

        public void PushMessage(T message, string queue)
        {
        }

        public void StopListening()
        {
        }
    }

}