using System;
using System.Collections.Generic;
using Live_Performance.Entity;
using Live_Performance.Models;
using NUnit.Framework;

namespace Live_Performance.Tests
{
    public class BudgetTest
    {
        [Test]
        public void TestOneMotorized()
        {
            Rent rent = new Rent
            {
                Begin = DateTime.Today,
                End = DateTime.Today,
                Boats = new List<BoatRent>
                {
                    new BoatRent
                    {
                        Boat = new Boat
                        {
                            BoatType = new MotorBoat()
                        },
                        Cost = 1500
                    }
                },
                Articles = new List<ArticleRent>(),
                Areas = new List<AreaRent>()
            };

            Assert.AreEqual(12, Budget.LakesForBudget(rent, 5000));
            Assert.AreEqual(12, Budget.LakesForBudget(rent, 3300));
            Assert.AreEqual(11, Budget.LakesForBudget(rent, 3299));
            Assert.AreEqual(0, Budget.LakesForBudget(rent, 1500));
            Assert.AreEqual(0, Budget.LakesForBudget(rent, 1501));
            Assert.AreEqual(1, Budget.LakesForBudget(rent, 1600));
            Assert.AreEqual(5, Budget.LakesForBudget(rent, 2000));
            Assert.AreEqual(5, Budget.LakesForBudget(rent, 2150));
            Assert.AreEqual(6, Budget.LakesForBudget(rent, 2400));
            Assert.AreEqual(7, Budget.LakesForBudget(rent, 2550));
        }

        [Test]
        public void TestTwoMotorized()
        {
            Rent rent = new Rent
            {
                Begin = DateTime.Today,
                End = DateTime.Today,
                Boats = new List<BoatRent>
                {
                    new BoatRent
                    {
                        Boat = new Boat
                        {
                            BoatType = new MotorBoat()
                        },
                        Cost = 1500
                    },
                    new BoatRent
                    {
                        Boat = new Boat
                        {
                            BoatType = new MotorBoat()
                        },
                        Cost = 1500
                    }
                },
                Articles = new List<ArticleRent>(),
                Areas = new List<AreaRent>()
            };

            Assert.AreEqual(12, Budget.LakesForBudget(rent, 2*5000));
            Assert.AreEqual(12, Budget.LakesForBudget(rent, 2*3300));
            Assert.AreEqual(11, Budget.LakesForBudget(rent, 2*3299));
            Assert.AreEqual(0, Budget.LakesForBudget(rent, 2*1500));
            Assert.AreEqual(0, Budget.LakesForBudget(rent, 2*1501));
            Assert.AreEqual(1, Budget.LakesForBudget(rent, 2*1600));
            Assert.AreEqual(5, Budget.LakesForBudget(rent, 2*2000));
            Assert.AreEqual(5, Budget.LakesForBudget(rent, 2*2150));
            Assert.AreEqual(6, Budget.LakesForBudget(rent, 2*2400));
            Assert.AreEqual(7, Budget.LakesForBudget(rent, 2*2550));
        }

        [Test]
        public void TestOneArea()
        {
            Rent rent = new Rent
            {
                Begin = DateTime.Today,
                End = DateTime.Today,
                Boats = new List<BoatRent>
                {
                    new BoatRent
                    {
                        Boat = new Boat
                        {
                            BoatType = new MotorBoat()
                        },
                        Cost = 1500
                    }
                },
                Articles = new List<ArticleRent>(),
                Areas = new List<AreaRent>
                {
                    new AreaRent
                    {
                        Cost = 200
                    }
                }
            };

            Assert.AreEqual(12, Budget.LakesForBudget(rent, 5000 + 200));
            Assert.AreEqual(12, Budget.LakesForBudget(rent, 3300 + 200));
            Assert.AreEqual(11, Budget.LakesForBudget(rent, 3299 + 200));
            Assert.AreEqual(0, Budget.LakesForBudget(rent, 1500 + 200));
            Assert.AreEqual(0, Budget.LakesForBudget(rent, 1501 + 200));
            Assert.AreEqual(1, Budget.LakesForBudget(rent, 1600 + 200));
            Assert.AreEqual(5, Budget.LakesForBudget(rent, 2000 + 200));
            Assert.AreEqual(5, Budget.LakesForBudget(rent, 2150 + 200));
            Assert.AreEqual(6, Budget.LakesForBudget(rent, 2400 + 200));
            Assert.AreEqual(7, Budget.LakesForBudget(rent, 2550 + 200));
        }

        [Test]
        public void TestTwoDays()
        {
            Rent rent = new Rent
            {
                Begin = DateTime.Today,
                End = DateTime.Today.AddDays(1),
                Boats = new List<BoatRent>
                {
                    new BoatRent
                    {
                        Boat = new Boat
                        {
                            BoatType = new MotorBoat()
                        },
                        Cost = 1500
                    }
                },
                Articles = new List<ArticleRent>(),
                Areas = new List<AreaRent>()
            };

            Assert.AreEqual(12, Budget.LakesForBudget(rent, 2*5000));
            Assert.AreEqual(12, Budget.LakesForBudget(rent, 2*3300));
            Assert.AreEqual(11, Budget.LakesForBudget(rent, 2*3299));
            Assert.AreEqual(0, Budget.LakesForBudget(rent, 2*1500));
            Assert.AreEqual(0, Budget.LakesForBudget(rent, 2*1501));
            Assert.AreEqual(1, Budget.LakesForBudget(rent, 2*1600));
            Assert.AreEqual(5, Budget.LakesForBudget(rent, 2*2000));
            Assert.AreEqual(5, Budget.LakesForBudget(rent, 2*2150));
            Assert.AreEqual(6, Budget.LakesForBudget(rent, 2*2400));
            Assert.AreEqual(7, Budget.LakesForBudget(rent, 2*2550));
        }

        [Test]
        public void TestArticles()
        {
            Rent rent = new Rent
            {
                Begin = DateTime.Today,
                End = DateTime.Today,
                Boats = new List<BoatRent>
                {
                    new BoatRent
                    {
                        Boat = new Boat
                        {
                            BoatType = new MotorBoat()
                        },
                        Cost = 1500
                    }
                },
                Articles = new List<ArticleRent>
                {
                    new ArticleRent
                    {
                        Amount = 3,
                        Cost = 125
                    },
                    new ArticleRent
                    {
                        Amount = 4,
                        Cost = 125
                    }
                },
                Areas = new List<AreaRent>()
            };

            Assert.AreEqual(12, Budget.LakesForBudget(rent, 5000 + 875));
            Assert.AreEqual(12, Budget.LakesForBudget(rent, 3300 + 875));
            Assert.AreEqual(11, Budget.LakesForBudget(rent, 3299 + 875));
            Assert.AreEqual(0, Budget.LakesForBudget(rent, 1500 + 875));
            Assert.AreEqual(0, Budget.LakesForBudget(rent, 1501 + 875));
            Assert.AreEqual(1, Budget.LakesForBudget(rent, 1600 + 875));
            Assert.AreEqual(5, Budget.LakesForBudget(rent, 2000 + 875));
            Assert.AreEqual(5, Budget.LakesForBudget(rent, 2150 + 875));
            Assert.AreEqual(6, Budget.LakesForBudget(rent, 2400 + 875));
            Assert.AreEqual(7, Budget.LakesForBudget(rent, 2550 + 875));
        }

        private class MotorBoat : BoatType
        {
            public MotorBoat()
            {
                Motorized = true;
                Cost = 1500;
            }
        }
    }
}