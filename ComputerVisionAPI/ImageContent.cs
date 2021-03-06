﻿using ByteConverter;
using ConnectionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ComputerVisionAPI
{
    public class ImageContent
    {
        private string finalContent = "";  //this string will hold a JSON returned from the API
        private bool isFinished = false;
        /// <summary>
        /// Does the heavy lifting by formatting the image and endpoint for broadcast
        /// </summary>
        /// <param name="endpointInformation"></param>
        /// <param name="collectedFilepath"></param>
        public string RequestImageAnalysis(ConnectContainer endpointInformation, string collectedFilepath)
        {
            HttpClient newClient = new HttpClient(); //Create a new client            
            //add the key, and the tag associated with the key
            newClient.DefaultRequestHeaders.Add(endpointInformation.getKeyTag(), endpointInformation.getSubKey());
            //Convert the image from the file path to an array of bytes using the Byte Converter library 
            byte[] imageData = ImageConversion.ImageToByteArray(collectedFilepath);
            //Hand the relevant data to the async broadcast function 
            BroadcastByteArray(imageData, newClient, endpointInformation);
            //hold while async function waits for response from API
            while (!isFinished)
            {
            }
            return finalContent;
        }
        /// <summary>
        /// Broadcasts byte array to endpoint, and saves JSON response 
        /// TODO: check API documentation. This function may be able to work for all APIs
        /// TODO: In above case, break function out into the Connection Manager Library 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="client"></param>
        /// <param name="endpointInformation"></param>
        private async void BroadcastByteArray(byte[] image, HttpClient client, ConnectContainer endpointInformation)
        {
            //provide http content using the byte array from the image             
            ByteArrayContent imageContent = new ByteArrayContent(image);
            HttpResponseMessage messageResponse; //This will be used to receive and translate data from the endpoint 
            //here a the contents of the image is defined as arbitrary binary data, which allows for a wider range of configurations to be used 
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //use async to post the url and full, properly formatted image content 
            messageResponse = await client.PostAsync(endpointInformation.getFullUrl(), imageContent);
            //assign the JSON received from the API to the private string of this class once the API responds 
            this.finalContent = await messageResponse.Content.ReadAsStringAsync();
            this.isFinished = true;
        }
        //accessor for JSON string 
        public string GetFinalContent()
        {
            return this.finalContent;
        }
        //accessor used to determine if the async function has finished 
        public bool GetIsFinished()
        {
            return this.isFinished;
        }
    }
}
