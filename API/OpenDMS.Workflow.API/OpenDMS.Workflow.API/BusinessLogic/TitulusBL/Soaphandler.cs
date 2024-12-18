using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace OpenDMS.Workflow.API.BusinessLogic.TitulusBL
{
    public class Soaphandler
    {

        public static string SoapCall(string xmlBody, string url, string username, string password)
        {

            // Crea un'istanza di CustomBinding
            var binding = new CustomBinding(
                new TextMessageEncodingBindingElement(MessageVersion.Soap11, System.Text.Encoding.UTF8),
                new HttpsTransportBindingElement()
                {
                    AuthenticationScheme = AuthenticationSchemes.Basic,
                    MaxReceivedMessageSize = 314572800
                });

            // Crea un'istanza di ChannelFactory senza specificare il tipo generico
            var factory = new ChannelFactory<IRequestChannel>(binding, new EndpointAddress(url));

            try
            {
                // Imposta le credenziali dell'utente per l'autenticazione
                factory.Credentials.UserName.UserName = username;
                factory.Credentials.UserName.Password = password;

                // Crea il canale di comunicazione
                IRequestChannel channel = factory.CreateChannel();

                // Crea un oggetto XmlReader utilizzando il corpo XML fornito
                XmlReader xmlReader = XmlReader.Create(new StringReader(xmlBody));

                // Imposta l'operazione da chiamare e i parametri se necessario
                var request = Message.CreateMessage(xmlReader, int.MaxValue, MessageVersion.Soap11);
                request.Headers.Action = "";

                // Invia la richiesta al servizio SOAP e ottieni la risposta
                Message response = channel.Request(request);
                // Controlla lo stato della risposta
                if (response.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                {
                    var httpResponse = (HttpResponseMessageProperty)response.Properties[HttpResponseMessageProperty.Name];
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        // La chiamata è andata a buon fine, restituisci la risposta come stringa
                        return response.ToString();
                    }
                    else
                    {
                        // La chiamata non è andata a buon fine, lancia un'eccezione con il messaggio di errore
                        throw new Exception($"{response}");
                    }
                }
                else
                {
                    // La risposta non contiene lo stato HTTP, lancia un'eccezione generica
                    throw new Exception("La chiamata SOAP non ha restituito lo stato HTTP");
                }
            }
            catch (Exception ex)
            {
                // Gestisci eventuali errori
                Console.WriteLine("Si è verificato un errore: " + ex.Message);
                throw new Exception("Si è verificato un errore: " + ex.Message);
            }
            finally
            {
                // Chiudi la factory e il canale
                factory.Close();
            }
        }
    }
}
