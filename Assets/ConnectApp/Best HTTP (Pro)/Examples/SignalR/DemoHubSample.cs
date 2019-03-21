#if !BESTHTTP_DISABLE_SIGNALR

using System;

using UnityEngine;

using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using BestHTTP.SignalR.JsonEncoders;

namespace BestHTTP.Examples
{
    public sealed class DemoHubSample : MonoBehaviour
    {
        readonly Uri URI = new Uri(GUIHelper.BaseURL + "/signalr");

        /// <summary>
        /// The SignalR connection instance
        /// </summary>
        Connection signalRConnection;

        /// <summary>
        /// DemoHub client side implementation
        /// </summary>
        DemoHub demoHub;

        /// <summary>
        /// TypedDemoHub client side implementation
        /// </summary>
        TypedDemoHub typedDemoHub;

        /// <summary>
        ///  VB .NET Hub
        /// </summary>
        Hub vbDemoHub;

        /// <summary>
        /// Result of the VB demo's ReadStateValue call
        /// </summary>
        string vbReadStateResult = string.Empty;

        Vector2 scrollPos;

        void Start()
        {
            // Create the hubs
            demoHub = new DemoHub();
            typedDemoHub = new TypedDemoHub();
            vbDemoHub = new Hub("vbdemo");

            // Create the SignalR connection, passing all the three hubs to it
            signalRConnection = new Connection(URI, demoHub, typedDemoHub, vbDemoHub);

            // Switch from the default encoder to the LitJson Encoder because it can handle the complex types too.
            signalRConnection.JsonEncoder = new LitJsonEncoder();

            // Call the demo functions when we successfully connect to the server
            signalRConnection.OnConnected += (connection) =>
                {
                    var person = new { Name = "Foo", Age = 20, Address = new { Street = "One Microsoft Way", Zip = "98052" } };

                // Call the demo functions

                demoHub.AddToGroups();
                    demoHub.GetValue();
                    demoHub.TaskWithException();
                    demoHub.GenericTaskWithException();
                    demoHub.SynchronousException();
                    demoHub.DynamicTask();
                    demoHub.PassingDynamicComplex(person);
                    demoHub.SimpleArray(new int[] { 5, 5, 6 });
                    demoHub.ComplexType(person);
                    demoHub.ComplexArray(new object[] { person, person, person });
                    demoHub.ReportProgress("Long running job!");

                    demoHub.Overload();

                // set some state
                demoHub.State["name"] = "Testing state!";
                    demoHub.ReadStateValue();

                    demoHub.PlainTask();
                    demoHub.GenericTaskWithContinueWith();

                    typedDemoHub.Echo("Typed echo callback");

                // vbDemo is not wrapped in a hub class, it would contain only one function
                vbDemoHub.Call("readStateValue", (hub, msg, result) => vbReadStateResult = string.Format("Read some state from VB.NET! => {0}", result.ReturnValue == null ? "undefined" : result.ReturnValue.ToString()));
                };

            // Start opening the signalR connection
            signalRConnection.Open();
        }

        void OnDestroy()
        {
            // Close the connection when we are closing this sample
            signalRConnection.Close();
        }

        void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);
                GUILayout.BeginVertical();

                demoHub.Draw();

                typedDemoHub.Draw();

                GUILayout.Label("Read State Value");
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label(vbReadStateResult);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            });

        }
    }

    /// <summary>
    /// Wrapper class of the 'TypedDemoHub' hub
    /// </summary>
    class TypedDemoHub : Hub
    {
        string typedEchoResult = string.Empty;
        string typedEchoClientResult = string.Empty;

        public TypedDemoHub()
            : base("typeddemohub")
        {

            // Setup server-called functions
            base.On("Echo", Echo);
        }

        #region Server Called Functions

        /// <summary>
        /// Server-called, client side implementation of the Echo function
        /// </summary>
        private void Echo(Hub hub, MethodCallMessage methodCall)
        {
            typedEchoClientResult = string.Format("{0} #{1} triggered!", methodCall.Arguments[0], methodCall.Arguments[1]);
        }

        #endregion

        #region Client Called Function(s)

        /// <summary>
        /// Client-called, server side implementation of the Echo function.
        /// When the function successfully executed on the server the OnEcho_Done callback function will be called.
        /// </summary>
        public void Echo(string msg)
        {
            base.Call("echo", OnEcho_Done, msg);
        }

        /// <summary>
        /// When the function successfully executed on the server this callback function will be called.
        /// </summary>
        private void OnEcho_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
        {
            typedEchoResult = "TypedDemoHub.Echo(string message) invoked!";
        }

        #endregion

        public void Draw()
        {
            GUILayout.Label("Typed callback");

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginVertical();
            GUILayout.Label(typedEchoResult);
            GUILayout.Label(typedEchoClientResult);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
        }
    }

    /// <summary>
    /// A wrapper class for the 'DemoHub' hub.
    /// </summary>
    class DemoHub : Hub
    {
        #region Private fields

        // These fields are here to store results of the function calls

        float longRunningJobProgress = 0f;
        string longRunningJobStatus = "Not Started!";
        string fromArbitraryCodeResult = string.Empty;
        string groupAddedResult = string.Empty;
        string dynamicTaskResult = string.Empty;
        string genericTaskResult = string.Empty;
        string taskWithExceptionResult = string.Empty;
        string genericTaskWithExceptionResult = string.Empty;
        string synchronousExceptionResult = string.Empty;
        string invokingHubMethodWithDynamicResult = string.Empty;
        string simpleArrayResult = string.Empty;
        string complexTypeResult = string.Empty;
        string complexArrayResult = string.Empty;
        string voidOverloadResult = string.Empty;
        string intOverloadResult = string.Empty;
        string readStateResult = string.Empty;
        string plainTaskResult = string.Empty;
        string genericTaskWithContinueWithResult = string.Empty;
        GUIMessageList invokeResults = new GUIMessageList();

        #endregion

        public DemoHub()
            : base("demo")
        {

            // Setup server-called functions
            base.On("invoke", Invoke);
            base.On("signal", Signal);
            base.On("groupAdded", GroupAdded);
            base.On("fromArbitraryCode", FromArbitraryCode);
        }

        #region Client Called Functions

        #region ReportProgress

        public void ReportProgress(string arg)
        {
            Call("reportProgress", OnLongRunningJob_Done, null, OnLongRunningJob_Progress, arg);
        }

        public void OnLongRunningJob_Progress(Hub hub, ClientMessage originialMessage, ProgressMessage progress)
        {
            longRunningJobProgress = (float)progress.Progress;
            longRunningJobStatus = progress.Progress.ToString() + "%";
        }

        public void OnLongRunningJob_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
        {
            longRunningJobStatus = result.ReturnValue.ToString();

            MultipleCalls();
        }

        #endregion

        public void MultipleCalls()
        {
            base.Call("multipleCalls");
        }

        #region DynamicTask

        public void DynamicTask()
        {
            base.Call("dynamicTask", OnDynamicTask_Done, OnDynamicTask_Failed);
        }

        private void OnDynamicTask_Failed(Hub hub, ClientMessage originalMessage, FailureMessage result)
        {
            dynamicTaskResult = string.Format("The dynamic task failed :( {0}", result.ErrorMessage);
        }

        private void OnDynamicTask_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
        {
            dynamicTaskResult = string.Format("The dynamic task! {0}", result.ReturnValue);
        }

        #endregion

        public void AddToGroups()
        {
            base.Call("addToGroups");
        }

        public void GetValue()
        {
            base.Call("getValue", (hub, msg, result) => genericTaskResult = string.Format("The value is {0} after 5 seconds", result.ReturnValue));
        }

        public void TaskWithException()
        {
            // This method call must fail, so only error handler added
            base.Call("taskWithException", null, (Hub hub, ClientMessage msg, FailureMessage error) => taskWithExceptionResult = string.Format("Error: {0}", error.ErrorMessage));
        }

        public void GenericTaskWithException()
        {
            // This method call must fail, so only error handler added
            base.Call("genericTaskWithException", null, (Hub hub, ClientMessage msg, FailureMessage error) => genericTaskWithExceptionResult = string.Format("Error: {0}", error.ErrorMessage));
        }

        public void SynchronousException()
        {
            // This method call must fail, so only error handler added
            base.Call("synchronousException", null, (Hub hub, ClientMessage msg, FailureMessage error) => synchronousExceptionResult = string.Format("Error: {0}", error.ErrorMessage));
        }

        public void PassingDynamicComplex(object person)
        {
            base.Call("passingDynamicComplex", (hub, msg, result) => invokingHubMethodWithDynamicResult = string.Format("The person's age is {0}", result.ReturnValue), person);
        }

        public void SimpleArray(int[] array)
        {
            base.Call("simpleArray", (hub, msg, result) => simpleArrayResult = "Simple array works!", array);
        }

        public void ComplexType(object person)
        {
            base.Call("complexType", (hub, msg, result) => complexTypeResult = string.Format("Complex Type -> {0}", (this as IHub).Connection.JsonEncoder.Encode(this.State["person"])), person);
        }

        public void ComplexArray(object[] complexArray)
        {
            // We need to cast the object array to object to keep it as an array
            // http://stackoverflow.com/questions/36350/how-to-pass-a-single-object-to-a-params-object
            base.Call("ComplexArray", (hub, msg, result) => complexArrayResult = "Complex Array Works!", (object)complexArray);
        }

        #region Overloads

        public void Overload()
        {
            base.Call("Overload", OnVoidOverload_Done);
        }

        private void OnVoidOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
        {
            voidOverloadResult = "Void Overload called";

            Overload(101);
        }

        public void Overload(int number)
        {
            base.Call("Overload", OnIntOverload_Done, number);
        }

        private void OnIntOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
        {
            intOverloadResult = string.Format("Overload with return value called => {0}", result.ReturnValue.ToString());
        }

        #endregion

        public void ReadStateValue()
        {
            base.Call("readStateValue", (hub, msg, result) => readStateResult = string.Format("Read some state! => {0}", result.ReturnValue));
        }

        public void PlainTask()
        {
            base.Call("plainTask", (hub, msg, result) => plainTaskResult = "Plain Task Result");
        }

        public void GenericTaskWithContinueWith()
        {
            base.Call("genericTaskWithContinueWith", (hub, msg, result) => genericTaskWithContinueWithResult = result.ReturnValue.ToString());
        }

        #endregion

        #region Server Called Functions

        private void FromArbitraryCode(Hub hub, MethodCallMessage methodCall)
        {
            fromArbitraryCodeResult = methodCall.Arguments[0] as string;
        }

        private void GroupAdded(Hub hub, MethodCallMessage methodCall)
        {
            if (!string.IsNullOrEmpty(groupAddedResult))
                groupAddedResult = "Group Already Added!";
            else
                groupAddedResult = "Group Added!";
        }

        private void Signal(Hub hub, MethodCallMessage methodCall)
        {
            dynamicTaskResult = string.Format("The dynamic task! {0}", methodCall.Arguments[0]);
        }

        private void Invoke(Hub hub, MethodCallMessage methodCall)
        {
            invokeResults.Add(string.Format("{0} client state index -> {1}", methodCall.Arguments[0], this.State["index"]));
        }

        #endregion

        #region Draw

        /// <summary>
        /// Display the result's of the function calls.
        /// </summary>
        public void Draw()
        {
            GUILayout.Label("Arbitrary Code");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(string.Format("Sending {0} from arbitrary code without the hub itself!", fromArbitraryCodeResult));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Group Added");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(groupAddedResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Dynamic Task");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(dynamicTaskResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Report Progress");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginVertical();
            GUILayout.Label(longRunningJobStatus);
            GUILayout.HorizontalSlider(longRunningJobProgress, 0, 100);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Generic Task");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(genericTaskResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Task With Exception");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(taskWithExceptionResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Generic Task With Exception");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(genericTaskWithExceptionResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Synchronous Exception");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(synchronousExceptionResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Invoking hub method with dynamic");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(invokingHubMethodWithDynamicResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Simple Array");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(simpleArrayResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Complex Type");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(complexTypeResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Complex Array");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(complexArrayResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Overloads");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginVertical();
            GUILayout.Label(voidOverloadResult);
            GUILayout.Label(intOverloadResult);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Read State Value");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(readStateResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Plain Task");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(plainTaskResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Generic Task With ContinueWith");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(genericTaskWithContinueWithResult);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Message Pump");
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            invokeResults.Draw(Screen.width - 40, 270);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
        }

        #endregion
    }
}

#endif