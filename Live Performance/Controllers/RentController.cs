using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Inject;
using Live_Performance.Entity;
using Live_Performance.Models;
using Live_Performance.Persistence;
using Util;

namespace Live_Performance.Controllers
{
    [Authentication]
    public class RentController : EntityController<Rent>
    {
        private readonly IRepository<Area> _areaRepository = Injector.Resolve<IRepository<Area>>();
        private readonly IRepository<Article> _articleRepository = Injector.Resolve<IRepository<Article>>();
        private readonly IRepository<ArticleRent> _articleRentRepository = Injector.Resolve<IRepository<ArticleRent>>();
        private readonly IRepository<Boat> _boatRepository = Injector.Resolve<IRepository<Boat>>();

        public ActionResult Index()
        {
            User user = (User) Session[SessionVars.User];
            if (user.Admin)
            {
                return View(Repository.FindAll());
            }

            return View(Repository.FindAllWhere(rent => rent.User == user));
        }

        public ActionResult Details(int id)
        {
            User user = (User) Session[SessionVars.User];
            Rent rent = Repository.FindOne(id);
            rent.Articles = _articleRentRepository.FindAllWhere(articleRent => articleRent.Rent == id);

            if (user.Admin || rent.User == user)
            {
                return View(rent);
            }

            return View();
        }

        public ActionResult New()
        {
            ViewBag.AvailableArticles = new SelectList(new List<Article> {new Article {Id = -1, Name = "Geen"}}.Concat(_articleRepository.FindAll()), "Id", "Name");
            ViewBag.AvailableBoats = new SelectList(new List<Boat> {new Boat {Id = -1, Name = "Geen"} }.Concat(_boatRepository.FindAll()));

            return View();
        }

        [HttpPost]
        public ActionResult New(Rent rent)
        {
            User user = (User) Session[SessionVars.User];
            rent.User = user;
            rent.Id = 0;

            rent.Articles
                .RemoveAll(articleRent => articleRent.Article.Id == -1 || articleRent.Amount <= 0);
            rent.Articles
                .ForEach(articleRent =>
                {
                    articleRent.Article = _articleRepository.FindOne(articleRent.Article.Id);
                    articleRent.Cost = articleRent.Article.Cost;
                });

            Rent saved = Repository.Save(rent);
            return RedirectToAction("Details", new {id = saved.Id});
        }
    }
}