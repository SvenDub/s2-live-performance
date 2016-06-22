using System.Web.Http;
using Live_Performance.Entity;
using Live_Performance.Models;

namespace Live_Performance.Controllers
{
    public class BudgetController : ApiController
    {
        public int Lakes(Rent rent, int budget) => Budget.LakesForBudget(rent, budget);
    }
}