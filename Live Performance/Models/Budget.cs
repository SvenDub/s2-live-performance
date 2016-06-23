using System;
using System.Linq;
using Live_Performance.Entity;

namespace Live_Performance.Models
{
    public class Budget
    {
        private const int AvailableLakes = 12;
        private const double BaseLakeCost = 100;
        private const double AdditionalLakeCost = 50;
        private const int AdditionalLakeThreshold = 5;

        public static int LakesForBudget(Rent rent, int budget)
        {
            // Amount of days, same day counts as 1
            int days = (rent.End - rent.Begin).Days + 1;

            // Costs for boats (rent only)
            int boatCosts = rent.Boats.Sum(boatRent => boatRent.Cost)*days;
            int boatAmount = rent.Boats.Count;

            // Costs for areas
            int areaCosts = rent.Areas.Sum(areaRent => areaRent.Cost)*boatAmount*days;

            // Cost for additional articles
            int articleCosts = rent.Articles.Sum(articleRent => articleRent.Cost*articleRent.Amount)*days;

            // Budget left for lakes
            int availableBudget = budget - boatCosts - areaCosts - articleCosts;

            // No budget left
            if (availableBudget <= 0)
            {
                return 0;
            }

            // Costs per lake
            double lakeCost;

            // Can afford enough to pay additional lake costs
            bool overThreshold = availableBudget >=
                                 (BaseLakeCost + AdditionalLakeCost)*(AdditionalLakeThreshold + 1)*boatAmount*days;

            if (overThreshold)
            {
                lakeCost = BaseLakeCost + AdditionalLakeCost;
            }
            else
            {
                lakeCost = BaseLakeCost;
            }

            // Divide the available budget over the lakes
            int lakes = (int) Math.Floor(availableBudget/(lakeCost*boatAmount*days));

            // Bound the amount of lakes by the maximum available and the threshold
            return Math.Min(overThreshold ? AvailableLakes : AdditionalLakeThreshold, lakes);
        }
    }
}