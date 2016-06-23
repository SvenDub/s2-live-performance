using System.Web.Mvc;
using Live_Performance.Entity;
using Live_Performance.Models;

namespace Live_Performance.Controllers
{
    public class ArticleController : EntityController<Article>
    {
        public ActionResult Index() => View(Repository.FindAll());

        [Authentication(Admin = true)]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authentication(Admin = true)]
        public ActionResult New(Article article)
        {
            article.Id = 0;
            article.Cost = 125;

            Article saved = Repository.Save(article);
            return RedirectToAction("Index", "Article");
        }

        [Authentication(Admin = true)]
        public ActionResult Delete(int id)
        {
            Repository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}