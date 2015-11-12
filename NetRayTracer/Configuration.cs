/// Copyright (c) 2015 Adam Kromm
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace NetRayTracer
{
    /// <summary>
    /// Represents the configuration for the ray tracer
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Gets the amount of jitter/variation to add to the rays when creating jitter rays
        /// </summary>
        public float Jitter { get; set; }

        /// <summary>
        /// Gets or sets the amount of jitter rays to cast when calculating reflection and refraction rays
        /// </summary>
        public float JitterRays { get; set; }

        /// <summary>
        /// Gets or sets the number of rays to cast for shadows
        /// </summary>
        public int ShadowRays { get; set; }

        /// <summary>
        /// Gets or sets the number of bounces/refractions to follow
        /// </summary>
        public int MaxRayDepth { get; set; }

        /// <summary>
        /// Gets or sets the path to the .obj file containing the data
        /// </summary>
        public string ObjFile { get; set; }
        
        /// <summary>
        /// Gets or sets the viewport data
        /// </summary>
        public Viewport ViewportData { get; set; }

        /// <summary>
        /// Gets or sets the desired width of the output image
        /// </summary>
        public int OutputWidth { get; set; }

        /// <summary>
        /// Gets or sets the desired height of the output image
        /// </summary>
        public int OutputHeight { get; set; }

        /// <summary>
        /// Gets or sets the lights in the scene
        /// </summary>
        public List<Light> Lights { get; set; } = new List<Light>();

        /// <summary>
        /// Load a JSON config file
        /// </summary>
        /// <param name="filename">The path to the configuration file</param>
        /// <returns>The configuration that was loaded</returns>
        public static Configuration Load(string filename)
        {
            JsonSerializer serializer = new JsonSerializer();

            Configuration config = null;

            using (StreamReader sReader = new StreamReader(filename))
            using (JsonReader reader = new JsonTextReader(sReader))
            {
                config = serializer.Deserialize<Configuration>(reader);
            }
            
            if (string.IsNullOrEmpty(config.ObjFile))
            {
                throw new ArgumentException("Missing path to .obj file");
            }
            
            return config;
        }
    }
}
