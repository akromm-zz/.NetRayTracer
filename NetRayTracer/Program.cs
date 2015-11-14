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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace NetRayTracer
{
    public class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("Usage: NetRayTracer {path to config file}");
                return;
            }

            Stopwatch watch = Stopwatch.StartNew();

            Console.WriteLine("{0} Reading config", watch.Elapsed);
            Configuration config = Configuration.Load(args[0]);

            Console.WriteLine("{0} Loading Obj", watch.Elapsed);
            ObjData data = ObjData.LoadFile(config.ObjFile);

            Console.WriteLine("{0} Loading Scene", watch.Elapsed);
            Scene scene = ObjToSceneConverter.Convert(data);
            
            Console.WriteLine("{0} Rendering", watch.Elapsed);
            Renderer renderer = new Renderer(config, scene);

            Bitmap output = renderer.Render();


            Console.WriteLine("{0} Saving output", watch.Elapsed);
            output.Save("output.jpeg", ImageFormat.Jpeg);

            Console.WriteLine("{0} Done!", watch.Elapsed);
            Console.ReadLine();
        }
    }
}
