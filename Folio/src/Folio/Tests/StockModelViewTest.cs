using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Folio.Tests
{
    public class StockModelViewTest
    {
        TestBetweenStocks tbs = new TestBetweenStocks(new Models.Stock(), 10);

        void ExpectedRetrunTester(decimal testDec, decimal expectedDec, TestBetweenStocks testBetween)
        {
            decimal actualReturn = testBetween.TestExpectedReturn(testDec);
            Assert.Equal(actualReturn, expectedDec);
        }
       // [Fact]
        void RunExpectedRetrunTest()
        {
            List<decimal> testList = new List<decimal> { 10, 5, 2, 1.003m, 1, .05m, 0, -.04m, -0.9m, -1, -1.55m, -2, -5, -10 };
            List<decimal> expectedList = new List<decimal> { .391m, .209m, .0635m,  .0634m, .0452m, .027m, .0124m, -.0094m, -.0294m, -.0458m, -.155m, -.337m };
            if (testList.Count == expectedList.Count)
                for (int i = 0; i < testList.Count; i++)
                    ExpectedRetrunTester(testList[i], expectedList[i], tbs);
        }
        void AddStockTester(int testNumber, int expectedNumber, TestBetweenStocks testBetween)
        {
            testBetween.testStockAdd(testNumber);
            int resultShares = testBetween.SharesOwned;
            Assert.Equal(resultShares, expectedNumber);
        }
       // [Fact]
        void RunAddStockTester()
        {
            List<int> testList = new List<int> { 0, 1, 5, 10, 1000000, 42202032 };
            int startShares = tbs.SharesOwned;
            for(int i = 0; i < testList.Count; i++)
            {
                int expectedResult = startShares;
                expectedResult = startShares + testList[i];
                AddStockTester(testList[i], expectedResult, tbs);
            }
         }
        void TestTester(int x, int y)
        { 
            Assert.Equal(x, y);
        }
        [Fact]
        void RunTestTester()
        {
            List<int> testList = new List<int> { 0, 1, 5, 10, 1000000, 42202032 };
            foreach (int i in testList)
                TestTester(testList[i], testList[i]);
        }
    }
}
