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
        /// Used for some generating some variance in rays being cast
        /// </summary>
        private static Random rand = new Random(DateTime.Now.Millisecond);

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

            Matrix view = Matrix.LookAt(config.ViewportData.Up, config.ViewportData.Position, config.ViewportData.Target);

            // calcualte the camera position using the view matrix
            cameraPosition = new Vector3(0, 0, 0);
            cameraPosition.Z = (config.OutputHeight / (2.0f * (float)Math.Tan((config.ViewportData.FieldOfView / 2.0f) * (float)Math.PI / 180.0f)));
            cameraPosition = view * cameraPosition;

            Vector3 right = new Vector3(1, 0, 0);
            Vector3 up = new Vector3(0, 1, 0);

            // Get the left and top side of the viewport in screen coordinates
            Vector3 viewportLeftWorldSpace = (-(config.ViewportData.Width / 2.0f) * right);
            Vector3 viewportTopWorldSpace = ((config.ViewportData.Height / 2.0f) * up);

            // Get the number of world units per pixel
            float horizontalUnitsPerPixel = config.ViewportData.Width / config.OutputWidth;
            float verticalUnitsPerPixel = config.ViewportData.Height / config.OutputHeight;

            // Keep track of all the tasks we start (one for each ray emitted by camera
            Task[] tasks = new Task[config.OutputWidth * config.OutputHeight];

            // Keep track of the final color of each pixel.  Bitmap doesnt allow concurrent access
            Vector4[,] colors = new Vector4[config.OutputWidth, config.OutputHeight];

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

                        // Get the position of the ray target in the viewport 
                        Vector3 viewportPos =
                            right * data.x * horizontalUnitsPerPixel + viewportLeftWorldSpace
                            - up * data.y * verticalUnitsPerPixel + viewportTopWorldSpace;

                        // Calculate the viewport position using the view matrix
                        viewportPos = view * viewportPos;

                        // Calculate the direction of the ray
                        Vector3 direction = viewportPos - cameraPosition;

                        // Create the ray
                        Ray r = new Ray(cameraPosition, direction);

                        // Cast the ray into the scene and store the resultant color
                        colors[data.x, data.y] = CastRay(r);

                    }, (object)d);
                }
            }

            Task.WaitAll(tasks);

            for (int x = 0; x < config.OutputWidth; x++)
            {
                for (int y = 0; y < config.OutputHeight; y++)
                {
                    ScaleColor(ref colors[x, y]);
                    colors[x, y] *= 255;
                    output.SetPixel(x, y,
                        Color.FromArgb(
                            (int)colors[x, y].W,
                            (int)colors[x, y].X,
                            (int)colors[x, y].Y,
                            (int)colors[x, y].Z));
                }
            }

            // Do anti-aliasing and other post processing effects here

            return output;
        }

        /// <summary>
        /// Scales the color so that no component is greater than 1.0f
        /// </summary>
        /// <param name="color">The color to scale</param>
        private void ScaleColor(ref Vector4 color)
        {
            // Find the biggest component (except w) or 1.0f if they are all smaller
            float max = Math.Max(Math.Max(Math.Max(color.X, color.Y), color.Z), 1.0f);

            // Scale all componenets (except w) so that 1.0 is the biggest number
            color.X /= max;
            color.Y /= max;
            color.Z /= max;

            if (color.W < 0) color.W = 0;
            if (color.W > 1.0f) color.W = 1.0f;

            if (color.X < 0) color.X = 0;
            if (color.Y < 0) color.Y = 0;
            if (color.Z < 0) color.Z = 0;
        }

        /// <summary>
        /// Casts a ray and gets the resultant color
        /// </summary>
        /// <param name="r">The ray to cast</param>
        /// <param name="depth">The current depth of the ray into the scene</param>
        /// <param name="sourceTriangle">The source of the ray if it was reflected/refracted</param>
        /// <returns>The resultant color of the ray</returns>
        public Vector4 CastRay(Ray r, int depth = 0, Triangle sourceTriangle = null)
        {
            float closestTime = float.MaxValue;
            Triangle closestTriangle = null;
            Vector3 closestPosition = Vector3.Zero;
            Vector4 returnColor = Vector4.Zero;

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
                returnColor = GetColor(closestTriangle, closestPosition);
            }
            else if (closestTriangle != null)
            {
                returnColor = GetColor(closestTriangle, closestPosition);
            }

            return returnColor;
        }

        public Vector4 GetColor(Triangle t, Vector3 pos)
        {
            Vector4 c = t.GetColor(pos);
            Vector4 retColor = new Vector4(0, 0, 0, 1);

            Vector3 viewVec = (cameraPosition - pos).Normalized;

            foreach (var light in config.Lights)
            {
                Vector3 lightVec = (light.Location - pos).Normalized;

                Vector4 avg = new Vector4();

                // Calcualte diffuse lighting

                // If this is >0 then the triangle light is somewhere in front of the triangle
                float dot = Vector3.Dot(lightVec, t.Normal);
                if (dot < 0) { dot = 0; }
                Vector4 diffuse = new Vector4(
                    c.X * light.DiffuseColor.X * dot,
                    c.Y * light.DiffuseColor.Y * dot,
                    c.Z * light.DiffuseColor.Z * dot,
                    c.W * light.DiffuseColor.W * dot);

                // Calculate specular lighting
                Vector3 reflected = Vector3.Reflection(lightVec, t.Normal).Normalized;
                dot = Vector3.Dot(reflected, viewVec);

                Vector4 specular = new Vector4(
                    (float)Math.Pow(light.SpecularColor.X * dot, t.GetSpecularCoefficient(pos)),
                    (float)Math.Pow(light.SpecularColor.Y * dot, t.GetSpecularCoefficient(pos)),
                    (float)Math.Pow(light.SpecularColor.Z * dot, t.GetSpecularCoefficient(pos)),
                    (float)Math.Pow(light.SpecularColor.W * dot, t.GetSpecularCoefficient(pos)));

                // Cast a bunch of shadow rays to determine how well lit this point is from the given light source
                for(int i = 0;i < config.ShadowRays; i++)
                {
                    if(!InShadow(pos, light))
                    {
                        // If we are lit then add the diffuse and specular colors
                        avg += diffuse;
                        avg += specular;
                    }
                }

                // Average the color by the number of shadow rays cast.
                avg /= config.ShadowRays;

                retColor.X += light.AmbientColor.X * c.X + avg.X;
                retColor.Y += light.AmbientColor.Y * c.Y + avg.Y;
                retColor.Z += light.AmbientColor.Z * c.Z + avg.Z;

                ScaleColor(ref retColor);
            }

            return retColor;
        }

        /// <summary>
        /// Determines if a point has direct line of sight to a light, or if it is shadowed by another object
        /// </summary>
        /// <param name="pos">The position being checked</param>
        /// <param name="light">The light source</param>
        /// <returns>Whether or not the point is lit by the light or in a shadow</returns>
        private bool InShadow(Vector3 pos, Light light)
        {
            // Get a vector from pos to light with some variation
            Vector3 lightVec = light.Location
                + new Vector3(
                    (float)rand.NextDouble() * 2.0f * light.Radius - light.Radius,
                    (float)rand.NextDouble() * 2.0f * light.Radius - light.Radius,
                    (float)rand.NextDouble() * 2.0f * light.Radius - light.Radius)
                - pos;

            float lightDist = lightVec.Magnitude;

            Ray shadowRay = new Ray(pos, lightVec);

            float time = -1;

            // Iterate over all the triangles in the scene to see if they block the light
            foreach(var t in scene.Triangles)
            {
                if(shadowRay.CollidesWith(t, ref time))
                {
                    if(time < lightDist && time > ProximityTolerance)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
