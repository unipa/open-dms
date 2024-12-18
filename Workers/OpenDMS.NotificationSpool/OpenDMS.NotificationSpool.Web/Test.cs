/*
namespace OpenDMS.NotificationManager.API
{
    public class TestsRabbitMQ
    {

        private IMessaging<string> messaging;
        [SetUp]
        public void Setup()
        {
            //creo l'istanza per usare MessageBus
            messaging = new RabbitMQMessageBus<string>("amqp://guest:guest@localhost:5672");
        }


        [Test]
        public void PushMessage_and_BasicGetSingleMessage_ReturnSamePushedMessage()
        {
            // Arrange
            //nome queue (creo un nome casuale per evitare di testare in code già usate)
            string queue = "api_demo_queue_" + Guid.NewGuid().ToString().Replace("-", "");
            //messaggio
            string PushedMessage = "ciao";
            //Act
            messaging.PushMessage(PushedMessage, queue);
            //Assert
            Assert.AreEqual(PushedMessage, result);
        }



        [Test]
        public void BasicGetSingleMessage_EmptyQueue_ReturnNull()
        {
            // Arrange
            //nome queue (creo un nome casuale per evitare di testare in code già usate)
            string queue = "api_demo_queue_" + Guid.NewGuid().ToString().Replace("-", "");
            //messaggio
            string PushedMessage = "ciao";
            //Act
            //non eseguo nessun push
            string result = messaging.BasicGetSingleMessage(queue);
            //Assert
            Assert.IsNull(result);

        }



        [Test]
        public void GetSingleMessage_ReturnCallbackProcessedTrue()
        {
            // Arrange



            //nome queue (creo un nome casuale per evitare di testare in code già usate)
            string queue = "api_demo_queue_" + Guid.NewGuid().ToString().Replace("-", "");



            //messaggio
            string PushedMessage = "ciao";



            bool CallbackProcessed = false;



            //Act



            messaging.PushMessage(PushedMessage, queue);



            string result = messaging.GetSingleMessage(queue, (message) =>
            {
                if (message.Contains(PushedMessage))
                    CallbackProcessed = true;
            });



            //Assert



            Assert.IsTrue(CallbackProcessed);



        }



        [Test]
        public void GetSingleMessage__EmptyQueue_ReturnNull()
        {
            // Arrange



            //nome queue (creo un nome casuale per evitare di testare in code già usate)
            string queue = "api_demo_queue_" + Guid.NewGuid().ToString().Replace("-", "");



            //messaggio
            string PushedMessage = "ciao";



            bool CallbackProcessed = false;



            //Act



            //non eseguo nessun push



            string result = messaging.GetSingleMessage(queue, (message) =>
            {
                if (message.Contains(PushedMessage))
                    CallbackProcessed = true;
            });



            //Assert



            Assert.IsNull(result);



        }



        [Test]
        public void Listening_WaitMultipleMessage_ReturnCollectedAllMessageTrue()
        {



            // Arrange




            //nome queue (creo un nome casuale per evitare di testare in code già usate)
            string queue = "api_demo_queue_" + Guid.NewGuid().ToString().Replace("-", "");



            //messaggio
            List<string> PushedMessages = new List<string>() { "ciao1", "ciao2", "ciao3" };



            List<string> CollectedMessage = new List<string>();
            bool CollectedAllMessage = false;



            //Act



            //resto in attesa dei messaggi registrandomi all'evento
            messaging.OnMessageReceived += (message) =>
            {
                CollectedMessage.Add(message);
                //Thread.Sleep(5000); IN QUESTO CASO FALLISCE
            };



            messaging.Listening(queue);
            foreach (string message in PushedMessages)
            {
                messaging.PushMessage(message, queue);
            }



            //Assert



            CollectedAllMessage = PushedMessages.SequenceEqual(CollectedMessage);



            Assert.IsTrue(CollectedAllMessage);



        }



        [Test]
        public void Listening_StopListening_ReturnCollectedSomeMessageTrue()
        {



            // Arrange



            IMessaging<string> messaging2 = new RabbitMQMessageBus<string>("amqp://guest:guest@localhost:5672");
            IMessaging<string> messaging1 = new RabbitMQMessageBus<string>("amqp://guest:guest@localhost:5672");




            //nome queue (creo un nome casuale per evitare di testare in code già usate)
            string queue = "api_demo_queue_" + Guid.NewGuid().ToString().Replace("-", "");



            //messaggio
            List<string> PushedMessages = new List<string>() { "ciao1", "ciao2", "ciao3" };



            List<string> CollectedMessage = new List<string>();
            bool CollectedSomeMessage = false;



            //Act



            //resto in attesa dei messaggi registrandomi all'evento
            messaging2.OnMessageReceived += (message) =>
            {
                CollectedMessage.Add(message);
            };



            messaging2.Listening(queue);



            int i = 0;
            foreach (string message in PushedMessages)
            {



                messaging1.PushMessage(message, queue);
                //if(i == 1)
                messaging2.StopListening();



                i++;
            }



            //Assert
            // la lista dei messaggi raccolti deve essere incompleta quindi : 1. diversa da PushedMessages ma con almeno il primo elemento uguale
            CollectedSomeMessage = (!PushedMessages.SequenceEqual(CollectedMessage) && PushedMessages[0].Equals(CollectedMessage[0]));



            Assert.IsTrue(CollectedSomeMessage);



        }



    }
}
*/