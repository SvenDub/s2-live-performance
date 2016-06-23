using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Inject;
using Live_Performance.Entity;
using Live_Performance.Models;
using Live_Performance.Persistence;

namespace Live_Performance.Controllers
{
    public class BoatController : EntityController<Boat>
    {
        private readonly IRepository<BoatType> _boatTypeRepository = Injector.Resolve<IRepository<BoatType>>();

        public ActionResult Index() => View(Repository.FindAll());

        [Authentication(Admin = true)]
        public ActionResult New()
        {
            ViewBag.AvailableBoatTypes = new SelectList(_boatTypeRepository.FindAll(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [Authentication(Admin = true)]
        public ActionResult New(Boat boat)
        {
            boat.Id = 0;
            boat.BoatType = _boatTypeRepository.FindOne(boat.BoatType.Id);

            Boat saved = Repository.Save(boat);
            return RedirectToAction("Index", "Boat");
        }

        [Authentication(Admin = true)]
        public ActionResult Delete(int id)
        {
            Repository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}