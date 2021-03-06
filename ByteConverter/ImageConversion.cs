﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteConverter
{
    public class ImageConversion
    {
        /// <summary>
        /// Converts an image from the filepath into an array of bytes 
        /// Returns array of bytes 
        /// </summary>
        /// <param name="imageLocation"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(string imageLocation)
        {
            //Create a file stream from the recieved address, and having read only access 
            FileStream imageStream = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            //Read the file as stream of bytes 
            BinaryReader byteImage = new BinaryReader(imageStream);          
            //read through all the bytes from the image, assigning them to an array of bytes
            byte[] byteArray = byteImage.ReadBytes((int)imageStream.Length);
            //Close file stream to the image before function termination 
            imageStream.Close();
            //return byte array
            return byteArray;
        }
    }
}
