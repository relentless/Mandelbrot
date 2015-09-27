using System;

namespace CSharpExample {
    class Program {

        static float xMandelbrotMin = -2.5f;
        static float xMandelbrotMax = 1.0f;

        static float yMandelbrotMin = -1.0f;
        static float yMandelbrotMax = 1.0f;

        static float xOutputCharacters = 80.0f;
        static float yOutputCharacters = 25.0f;

        static int max_iterations = 1000;

        static private float scale(float number, float inputMax, float outputMin, float outputMax) {
            var sourcePosition = number/inputMax;
            return sourcePosition * (outputMax - outputMin) + outputMin;
        }

        static private bool withinDistance(float x, float y, float distance) {
            return x*x + y*y < distance*distance;
        }

        static void Main(string[] args) {

            for(float yPixel=0f; yPixel < yOutputCharacters; yPixel++){
                for(float xPixel=0f; xPixel < xOutputCharacters; xPixel++){

                    var xScaled = scale(xPixel, xOutputCharacters, xMandelbrotMin, xMandelbrotMax);
                    var yScaled = scale(yPixel, yOutputCharacters, yMandelbrotMin, yMandelbrotMax);

                    var x = 0.0f;
                    var y = 0.0f;

                    var iteration = 0;

                    while (withinDistance(x, y, 2f) && iteration < max_iterations) {
              
                        var nextX = x*x - y*y + xScaled;
                        y = 2.0f*x*y + yScaled;
                        x = nextX;
                        iteration = iteration + 1;
                    }

                    Console.Write(iteration == 1000 ? "*" : " ");
                }
            }

            System.Console.ReadLine();
        }
    }
}
