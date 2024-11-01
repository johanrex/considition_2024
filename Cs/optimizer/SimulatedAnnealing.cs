using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer
{
    internal class SimulatedAnnealing
    {
        public static (double, int) Optimize(
            Func<double, int, double> costFunction,
            double x0,
            int y0,
            double initialTemperature,
            double coolingRate,
            int maxIterations,
            double xMin,
            double xMax,
            int yMin,
            int yMax)
        {
            Random rand = new Random();
            double x = x0;
            int y = y0;
            double bestX = x;
            int bestY = y;
            double bestCost = costFunction(x, y);
            double temperature = initialTemperature;

            for (int i = 0; i < maxIterations; i++)
            {
                // Perturb the variables
                double newX = x + (rand.NextDouble() - 0.5);
                int newY = y + (rand.Next(3) - 1); // Perturb y by -1, 0, or 1

                // Clamp the variables within their bounds
                newX = Math.Max(xMin, Math.Min(xMax, newX));
                newY = Math.Max(yMin, Math.Min(yMax, newY));

                double newCost = costFunction(newX, newY);

                // Accept the new variables based on probability
                if (newCost < bestCost || Math.Exp((bestCost - newCost) / temperature) > rand.NextDouble())
                {
                    x = newX;
                    y = newY;
                    bestCost = newCost;
                    bestX = x;
                    bestY = y;
                }

                // Decrease the temperature
                temperature *= coolingRate;
            }

            return (bestX, bestY);
        }
    }
}
