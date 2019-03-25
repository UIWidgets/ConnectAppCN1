using System;
using System.Collections.Generic;

using UnityEngine;
using BestHTTP;

namespace BestHTTP.Examples
{
    public sealed class TextureDownloadSample : MonoBehaviour
    {
        /// <summary>
        /// The URL of the server that will serve the image resources
        /// </summary>
        string BaseURL = GUIHelper.BaseURL + "/images/Demo/";

        #region Private Fields

        /// <summary>
        /// The downloadable images
        /// </summary>
        string[] Images = new string[9] { "One.png", "Two.png", "Three.png", "Four.png", "Five.png", "Six.png", "Seven.png", "Eight.png", "Nine.png" };

        /// <summary>
        /// The downloaded images will be stored as textures in this array
        /// </summary>
        Texture2D[] Textures = new Texture2D[9];

#if !BESTHTTP_DISABLE_CACHING
        /// <summary>
        /// True if all images are loaded from the local cache instead of the server
        /// </summary>
        bool allDownloadedFromLocalCache;
#endif

        /// <summary>
        /// How many sent requests are finished
        /// </summary>
        int finishedCount;

        /// <summary>
        /// GUI scroll position
        /// </summary>
        Vector2 scrollPos;

        #endregion

        #region Unity Events

        void Awake()
        {
            // Set a well observable value
            // This is how many concurrent requests can be made to a server
            HTTPManager.MaxConnectionPerServer = 1;

            // Create placeholder textures
            for (int i = 0; i < Images.Length; ++i)
                Textures[i] = new Texture2D(100, 150);
        }

        void OnDestroy()
        {
            // Set back to its defualt value.
            HTTPManager.MaxConnectionPerServer = 4;
        }

        void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
                {
                    scrollPos = GUILayout.BeginScrollView(scrollPos);

                // Draw out the textures
                GUILayout.SelectionGrid(0, Textures, 3);

#if !BESTHTTP_DISABLE_CACHING
                    if (finishedCount == Images.Length && allDownloadedFromLocalCache)
                        GUIHelper.DrawCenteredText("All images loaded from the local cache!");
#endif

                    GUILayout.FlexibleSpace();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Max Connection/Server: ", GUILayout.Width(150));
                    GUILayout.Label(HTTPManager.MaxConnectionPerServer.ToString(), GUILayout.Width(20));
                    HTTPManager.MaxConnectionPerServer = (byte)GUILayout.HorizontalSlider(HTTPManager.MaxConnectionPerServer, 1, 10);
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Start Download"))
                        DownloadImages();

                    GUILayout.EndScrollView();
                });
        }

        #endregion

        #region Private Helper Functions

        void DownloadImages()
        {
            // Set these metadatas to its initial values
#if !BESTHTTP_DISABLE_CACHING
            allDownloadedFromLocalCache = true;
#endif
            finishedCount = 0;

            for (int i = 0; i < Images.Length; ++i)
            {
                // Set a blank placeholder texture, overriding previously downloaded texture
                Textures[i] = new Texture2D(100, 150);

                // Construct the request
                var request = new HTTPRequest(new Uri(BaseURL + Images[i]), ImageDownloaded);

                // Set the Tag property, we can use it as a general storage bound to the request
                request.Tag = Textures[i];

                // Send out the request
                request.Send();
            }
        }

        /// <summary>
        /// Callback function of the image download http requests
        /// </summary>
        void ImageDownloaded(HTTPRequest req, HTTPResponse resp)
        {
            // Increase the finished count regardless of the state of our request
            finishedCount++;

            switch (req.State)
            {
                // The request finished without any problem.
                case HTTPRequestStates.Finished:
                    if (resp.IsSuccess)
                    {
                        // Get the Texture from the Tag property
                        Texture2D tex = req.Tag as Texture2D;

                        // Load the texture
                        tex.LoadImage(resp.Data);

#if !BESTHTTP_DISABLE_CACHING
                        // Update the cache-info variable
                        allDownloadedFromLocalCache = allDownloadedFromLocalCache && resp.IsFromCache;
#endif
                    }
                    else
                    {
                        Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                        resp.StatusCode,
                                                        resp.Message,
                                                        resp.DataAsText));
                    }
                    break;

                // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
                case HTTPRequestStates.Error:
                    Debug.LogError("Request Finished with Error! " + (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
                    break;

                // The request aborted, initiated by the user.
                case HTTPRequestStates.Aborted:
                    Debug.LogWarning("Request Aborted!");
                    break;

                // Connecting to the server is timed out.
                case HTTPRequestStates.ConnectionTimedOut:
                    Debug.LogError("Connection Timed Out!");
                    break;

                // The request didn't finished in the given time.
                case HTTPRequestStates.TimedOut:
                    Debug.LogError("Processing the request Timed Out!");
                    break;
            }
        }

        #endregion
    }
}