using System.Web.Http;
using Inject;
using Live_Performance.Entity;
using Live_Performance.Models;
using Live_Performance.Persistence;

namespace Live_Performance.Controllers
{
    public class BudgetController : ApiController
    {
        private readonly IRepository<Area> _areaRepository = Injector.Resolve<IRepository<Area>>();
        private readonly IRepository<Article> _articleRepository = Injector.Resolve<IRepository<Article>>();
        private readonly IRepository<Boat> _boatRepository = Injector.Resolve<IRepository<Boat>>();

        [HttpPost]
        public int Index(Rent rent)
        {
            rent.Articles.RemoveAll(articleRent => articleRent.Article.Id == -1 || articleRent.Amount <= 0);
            rent.Articles.ForEach(articleRent =>
            {
                articleRent.Article = _articleRepository.FindOne(articleRent.Article.Id);
                articleRent.Cost = articleRent.Article.Cost;
            });

            rent.Boats.RemoveAll(boatRent => boatRent.Boat.Id == -1);
            rent.Boats.ForEach(boatRent =>
            {
                boatRent.Boat = _boatRepository.FindOne(boatRent.Boat.Id);
                boatRent.Cost = boatRent.Boat.BoatType.Cost;
            });

            rent.Areas.RemoveAll(areaRent => areaRent.Area.Id == -1);
            rent.Areas.ForEach(areaRent =>
            {
                areaRent.Area = _areaRepository.FindOne(areaRent.Area.Id);
                areaRent.Cost = areaRent.Area.Cost;
            });

            return Budget.LakesForBudget(rent, rent.Budget);
        }
    }
}