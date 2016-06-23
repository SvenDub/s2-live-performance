using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Inject;
using Live_Performance.Entity;
using Live_Performance.Persistence;

namespace Live_Performance.Controllers
{
    [RoutePrefix("api/Rent")]
    public class RentApiController : ApiController
    {
        private readonly IRepository<Rent> _repository = Injector.Resolve<IRepository<Rent>>();
        private readonly IRepository<AreaRent> _areaRentRepository = Injector.Resolve<IRepository<AreaRent>>();
        private readonly IRepository<ArticleRent> _articleRentRepository = Injector.Resolve<IRepository<ArticleRent>>();
        private readonly IRepository<BoatRent> _boatRentRepository = Injector.Resolve<IRepository<BoatRent>>();

        [Route("")]
        [HttpGet]
        public List<Rent> Index()
        {
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment");

            List<Rent> rents = _repository.FindAll();
            rents.ForEach(rent =>
            {
                rent.Articles = _articleRentRepository.FindAllWhere(articleRent => articleRent.Rent == rent.Id);
                rent.Boats = _boatRentRepository.FindAllWhere(boatRent => boatRent.Rent == rent.Id);
                rent.Areas = _areaRentRepository.FindAllWhere(areaRent => areaRent.Rent == rent.Id);
            });
            
            return rents;
        }

        [Route("{id:int}")]
        [HttpGet]
        public Rent Get(int id)
        {
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment");

            Rent rent = _repository.FindOne(id);
            rent.Articles = _articleRentRepository.FindAllWhere(articleRent => articleRent.Rent == rent.Id);
            rent.Boats = _boatRentRepository.FindAllWhere(boatRent => boatRent.Rent == rent.Id);
            rent.Areas = _areaRentRepository.FindAllWhere(areaRent => areaRent.Rent == rent.Id);

            return rent;
        }
    }
}
