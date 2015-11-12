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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRayTracer
{
    public class Renderer
    {
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

            //
            // NOTE: For now just support the viewport being in the xy-plane
            // This later needs to be updated to allow the viewport to move and be rotated
            //
            cameraPosition = -config.ViewportData.Position;

            // calculate the offset the camera needs to be from the viewport
            cameraPosition.Z += (config.OutputHeight / (2.0f * (float)Math.Tan((config.ViewportData.FieldOfView / 2.0f) * (float)Math.PI / 180.0f)));

            // Get the left and top side of the viewport in screen coordinates
            float viewportLeftWorldSpace = config.ViewportData.Position.X - config.ViewportData.Width / 2.0f;
            float viewportTopWorldSpace = config.ViewportData.Position.Y - config.ViewportData.Height / 2.0f;

            // Get the number of world units per pixel
            float horizontalUnitsPerPixel = config.ViewportData.Width / config.OutputWidth;
            float verticalUnitsPerPixel = config.ViewportData.Height / config.OutputHeight;

            // The z component for the viewport when creating the rays
            float zDist = config.ViewportData.Position.Z - cameraPosition.Z;

            // Start casting the rays and tracing
            for(int h = config.OutputHeight;  h >= 0; h--)
            {
                for(int w = 0; w <= config.OutputWidth; w++)
                {
                    Vector3 viewportPos = new Vector3(
                        viewportLeftWorldSpace + w * horizontalUnitsPerPixel,
                        viewportTopWorldSpace + h * verticalUnitsPerPixel,
                        zDist);

                    Vector3 direction = viewportPos - cameraPosition;

                    Ray r = new Ray(cameraPosition, direction);

                    Color c = CastRay(r);

                    output.SetPixel(w, h, c);
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
            Color returnColor = Color.Black;

            foreach(var t in scene.Triangles)
            {
                float time = float.MaxValue;

                if(r.CollidesWith(t, ref time))
                {
                    if(time < closestTime && time > ProximityTolerance)
                    {
                        closestTime = time;
                        closestTriangle = t;
                        closestPosition = r.PointAtDistance(time);
                    }
                }
            }

            // If we can still cast rays deeper and we collided with an object, then cast more rays
            if(depth < config.MaxRayDepth && closestTriangle != null)
            {
                // TODO: Cast reflection rays

                // TODO: Cast refraction rays
            }
            else if ( closestTriangle != null)
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
