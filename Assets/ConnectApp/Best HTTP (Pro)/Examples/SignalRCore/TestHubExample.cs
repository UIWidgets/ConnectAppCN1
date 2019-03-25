#if !BESTHTTP_DISABLE_SIGNALR_CORE

using System;
using UnityEngine;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;

namespace BestHTTP.Examples
{
    public class TestHubExample : MonoBehaviour
    {
        // Server uri to connect to
        readonly Uri URI = new Uri(GUIHelper.BaseURL + "/TestHub");

        // Instance of the HubConnection
        HubConnection hub;

        Vector2 scrollPos;
        string uiText;

        void Start()
        {
            // Server side of this example can be found here:
            // https://github.com/Benedicht/BestHTTP_DemoSite/blob/master/BestHTTP_DemoSite/Hubs/TestHub.cs

            // Set up optional options
            HubOptions options = new HubOptions();
            options.SkipNegotiation = false;

            // Crete the HubConnection
            hub = new HubConnection(URI, new JsonProtocol(new LitJsonEncoder()), options);

            // Optionally add an authenticator
            //hub.AuthenticationProvider = new BestHTTP.SignalRCore.Authentication.HeaderAuthenticator("<generated jwt token goes here>");

            // Subscribe to hub events
            hub.OnConnected += Hub_OnConnected;
            hub.OnError += Hub_OnError;
            hub.OnClosed += Hub_OnClosed;

            hub.OnMessage += Hub_OnMessage;

            // Set up server callable functions
            hub.On("Send", (string arg) => uiText += string.Format(" On Send: {0}\n", arg));
            hub.On<Person>("Person", (person) => uiText += string.Format(" On Person: {0}\n", person));
            hub.On<Person, Person>("TwoPersons", (person1, person2) => uiText += string.Format(" On TwoPersons: {0}, {1}\n", person1, person2));

            // And finally start to connect to the server
            hub.StartConnect();

            uiText = "StartConnect called\n";
        }

        void OnDestroy()
        {
            if (hub != null)
                hub.StartClose();
        }

        // Draw the text stored in the 'uiText' field
        void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);
                GUILayout.BeginVertical();

                GUILayout.Label(uiText);

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            });

        }

        /// <summary>
        /// This callback is called when the plugin is connected to the server successfully. Messages can be sent to the server after this point.
        /// </summary>
        private void Hub_OnConnected(HubConnection hub)
        {
            uiText += "Hub Connected\n";

            // Call a server function with a string param. We expect no return value.
            hub.Send("Send", "my message");

            // Call a parameterless function. We expect a string return value.
            hub.Invoke<string>("NoParam")
                .OnSuccess(ret => uiText += string.Format(" 'NoParam' returned: {0}\n", ret));

            // Call a function on the server to add two numbers. OnSuccess will be called with the result and OnError if there's an error.
            hub.Invoke<int>("Add", 10, 20)
                .OnSuccess(result => uiText += string.Format(" 'Add(10, 20)' returned: {0}\n", result))
                .OnError(error => uiText += string.Format(" 'Add(10, 20)' error: {0}\n", error));

            // Call a function that will return a Person object constructed from the function's parameters.
            hub.Invoke<Person>("GetPerson", "Mr. Smith", 26)
                .OnSuccess(result => uiText += string.Format(" 'GetPerson(\"Mr. Smith\", 26)' returned: {0}\n", result))
                .OnError(error => uiText += string.Format(" 'GetPerson(\"Mr. Smith\", 26)' error: {0}\n", error));

            // To test errors/exceptions this call always throws an exception on the server side resulting in an OnError call.
            // OnError expected here!
            hub.Invoke<int>("SingleResultFailure", 10, 20)
                .OnSuccess(result => uiText += string.Format(" 'SingleResultFailure(10, 20)' returned: {0}\n", result))
                .OnError(error => uiText += string.Format(" 'SingleResultFailure(10, 20)' error: {0}\n", error));

            // This call demonstrates IEnumerable<> functions, result will be the yielded numbers.
            hub.Invoke<int[]>("Batched", 10)
                .OnSuccess(result => uiText += string.Format(" 'Batched(10)' returned items: {0}\n", result.Length))
                .OnError(error => uiText += string.Format(" 'Batched(10)' error: {0}\n", error));

            // OnItem is called for a streaming request for every items returned by the server. OnSuccess will still be called with all the items.
            hub.Stream<int>("ObservableCounter", 10, 1000)
                .OnItem(result => uiText += string.Format(" 'ObservableCounter(10, 1000)' OnItem: {0}\n", result.LastAdded))
                .OnSuccess(result => uiText += string.Format(" 'ObservableCounter(10, 1000)' OnSuccess. Final count: {0}\n", result.Items.Count))
                .OnError(error => uiText += string.Format(" 'ObservableCounter(10, 1000)' error: {0}\n", error));

            // A stream request can be cancelled any time.
            var container = hub.Stream<int>("ChannelCounter", 10, 1000)
                .OnItem(result => uiText += string.Format(" 'ChannelCounter(10, 1000)' OnItem: {0}\n", result.LastAdded))
                .OnSuccess(result => uiText += string.Format(" 'ChannelCounter(10, 1000)' OnSuccess. Final count: {0}\n", result.Items.Count))
                .OnError(error => uiText += string.Format(" 'ChannelCounter(10, 1000)' error: {0}\n", error)).value;

            // a stream can be cancelled by calling CancelStream
            hub.CancelStream(container);

            // This call will stream strongly typed objects
            hub.Stream<Person>("GetRandomPersons", 20, 2000)
                .OnItem(result => uiText += string.Format(" 'GetRandomPersons(20, 1000)' OnItem: {0}\n", result.LastAdded))
                .OnSuccess(result => uiText += string.Format(" 'GetRandomPersons(20, 1000)' OnSuccess. Final count: {0}\n", result.Items.Count));
        }

        /// <summary>
        /// This callback is called for every hub message. If false is returned, the plugin will cancel any further processing of the message.
        /// </summary>
        private bool Hub_OnMessage(HubConnection hub, BestHTTP.SignalRCore.Messages.Message message)
        {
            //UnityEngine.Debug.Log("Message Arrived: " + message.ToString());

            return true;
        }

        /// <summary>
        /// This is called when the hub is closed after a StartClose() call.
        /// </summary>
        private void Hub_OnClosed(HubConnection hub)
        {
            uiText += "Hub Closed\n";
        }

        /// <summary>
        /// Called when an unrecoverable error happen. After this event the hub will not send or receive any messages.
        /// </summary>
        private void Hub_OnError(HubConnection hub, string error)
        {
            uiText += "Hub Error: " + error + "\n";
        }

        /// <summary>
        /// Helper class to demonstrate strongly typed callbacks
        /// </summary>
        sealed class Person
        {
            public string Name { get; set; }
            public long Age { get; set; }

            public override string ToString()
            {
                return string.Format("[Person Name: '{0}', Age: {1}]", this.Name, this.Age.ToString());
            }
        }
    }
}

#endif