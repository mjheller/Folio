using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Services.MonteCarlo
{
    public static class HowToRun
    {
        //Stock YHOO = new Stock("YHOO", 28M, 300);
        //Stock GOOGL = new Stock("GOOGL", 599M, 300);
        //List<Stock> listy = new List<Stock>() { YHOO, GOOGL };
        //Portfolio Portfolio = new Portfolio(listy, 5000);



        //int nSteps = 40;
        //int nPaths = 1000000;
        //int initialPortfolioValue = 20000; //20000 is hardcode, will grab this from portfolio object
        //int annualContributions = 5000; //will grab this from portfolio object
        //int incomeDraw = 0; //will grab from portfolio/user

        ////define the creator function 
        //Func<int, PortfolioPath> creator = x => new PortfolioPath(nSteps, (double)Portfolio.expectedReturn, (double)Portfolio.variance, initialPortfolioValue, annualContributions, incomeDraw);

        //PortfolioPath[] simulationPaths = MonteCarlo.RunSimulation(nSteps, nPaths, creator);
        
    }
}
