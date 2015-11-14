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
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRayTracer
{
    public class Renderer
    {
        /// <summary>
        /// Private class to contain information passed to tasks
        /// </summary>
        private class TaskData
        {
            /// <summary>
            /// The current pixel x coordinate
            /// </summary>
            public int x;

            /// <summary>
            /// The current pixel y coordinate
            /// </summary>
            public int y;
        }

        /// <summary>
        /// The closest an object can be to a ray origin before it is ignored
        /// </summary>
        private const float ProximityTolerance = 0.5f;

        /// <summary>
        /// The configuration for rendering
        /// </summary>
        private Configuration config;

        /// <summary>
        /// The scene to render
        /// </summary>
        private Scene scene;

        /// <summary>
        /// The position of the camera being rendered from
        /// </summary>
        private Vector3 cameraPosition;

        /// <summary>
        /// Initialize the renderer
        /// </summary>
        /// <param name="c">The configuration for rendering</param>
        /// <param name="s">The scene to render</param>
        public Renderer(Configuration c, Scene s)
        {
            config = c;
            scene = s;
        }

        /// <summary>
        /// Renders the scene
        /// </summary>
        /// <returns>The constructed bitmap</returns>
        public Bitmap Render()
        {
            Bitmap output = new Bitmap(config.OutputWidth, config.OutputHeight);

            cameraPosition = new Vector3(0, 0, 0);

            //
            // NOTE: For now just support the viewport being in the xy-plane
            // This later needs to be updated to allow the viewport to move and be rotated
            //
            cameraPosition.Z = (config.OutputHeight / (2.0f * (float)Math.Tan((config.ViewportData.FieldOfView / 2.0f) * (float)Math.PI / 180.0f)));
            cameraPosition += config.ViewportData.Position;
            
            Vector3 right = new Vector3(1, 0, 0);
            Vector3 up = new Vector3(0, 1, 0);

            // Get the left and top side of the viewport in screen coordinates
            Vector3 viewportLeftWorldSpace = -(config.ViewportData.Width / 2.0f) * right;
            Vector3 viewportTopWorldSpace = +(config.ViewportData.Height / 2.0f) * up;

            // Get the number of world units per pixel
            float horizontalUnitsPerPixel = config.ViewportData.Width / config.OutputWidth;
            float verticalUnitsPerPixel = config.ViewportData.Height / config.OutputHeight;

            // Keep track of all the tasks we start (one for each ray emitted by camera
            Task[] tasks = new Task[config.OutputWidth * config.OutputHeight];

            // Keep track of the final color of each pixel.  Bitmap doesnt allow concurrent access
            Color[,] colors = new Color[config.OutputWidth, config.OutputHeight];

            // Start casting the rays and tracing
            for (int h = 0; h < config.OutputHeight; h++)
            {
                for (int w = 0; w < config.OutputWidth; w++)
                {
                    // Store current loop state to pass to the task
                    TaskData d = new TaskData();
                    d.x = w;
                    d.y = h;

                    tasks[h * config.OutputWidth + w] = Task.Factory.StartNew((td) =>
                    {
                        TaskData data = (TaskData)td;
                        Vector3 viewportPos =
                            right * data.x * horizontalUnitsPerPixel + viewportLeftWorldSpace
                            - up * data.y * verticalUnitsPerPixel + viewportTopWorldSpace
                            + config.ViewportData.Position;

                        Vector3 direction = viewportPos - cameraPosition;

                        Ray r = new Ray(cameraPosition, direction);

                        colors[data.x, data.y] = CastRay(r);
                    }, (object)d);
                }
            }

            Task.WaitAll(tasks);

            for (int x = 0; x < config.OutputWidth; x++)
            {
                for (int y = 0; y < config.OutputHeight; y++)
                {
                    output.SetPixel(x, y, colors[x, y]);
                }
            }

            // Do anti-aliasing and other post processing effects here

            return output;
        }

        /// <summary>
        /// Casts a ray and gets the resultant color
        /// </summary>
        /// <param name="r">The ray to cast</param>
        /// <param name="depth">The current depth of the ray into the scene</param>
        /// <param name="sourceTriangle">The source of the ray if it was reflected/refracted</param>
        /// <returns>The resultant color of the ray</returns>
        public Color CastRay(Ray r, int depth = 0, Triangle sourceTriangle = null)
        {
            float closestTime = float.MaxValue;
            Triangle closestTriangle = null;
            Vector3 closestPosition = null;
            Color returnColor = Color.FromArgb(0, 0, 0);

            foreach (var t in scene.Triangles)
            {
                float time = float.MaxValue;

                if (r.CollidesWith(t, ref time))
                {
                    if (time < closestTime && time > ProximityTolerance)
                    {
                        closestTime = time;
                        closestTriangle = t;
                        closestPosition = r.PointAtDistance(time);
                    }
                }
            }

            // If we can still cast rays deeper and we collided with an object, then cast more rays
            if (depth < config.MaxRayDepth && closestTriangle != null)
            {
                // TODO: Cast reflection rays

                // TODO: Cast refraction rays

                // TODO: Add the color combinations from reflection and refraction
                returnColor = closestTriangle.GetColor(closestPosition);
            }
            else if (closestTriangle != null)
            {
                returnColor = closestTriangle.GetColor(closestPosition);
            }

            return returnColor;
        }

        public Color GetColor(Triangle t, Vector3 pos)
        {
            Color c = new Color();

            c = t.GetColor(pos);

            Vector3 pointToCam = (cameraPosition - pos).Normalized;

            // TODO: Iterate over all lights in scene for phong shading


            return c;
        }
    }
}
