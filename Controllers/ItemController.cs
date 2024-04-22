using SoupAndDungeon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SoupAndDungeon.Controllers
{
    public class ItemController : ApiController
    {
        private readonly SoupAndDungeonDBContext _dbContext = new SoupAndDungeonDBContext();
        private readonly string BearerToken;

        public ItemController()
        {
            BearerToken = Token.BToken;
        }

        // Metodo per verificare il token nell'intestazione della richiesta
        private bool IsAuthorized(HttpRequestMessage request)
        {
            IEnumerable<string> headers;
            if (request.Headers.TryGetValues("Authorization", out headers))
            {
                var authToken = headers.FirstOrDefault();
                if (authToken != null && authToken.StartsWith("Bearer "))
                {
                    var token = authToken.Substring(7); // Rimuovi il prefisso "Bearer " dal token
                    if (token == BearerToken)
                    {
                        return true; // Il token è valido
                    }
                }
            }
            return false; // Il token non è valido
        }

        // GET api/item
        public IHttpActionResult GetItems()
        {
            // Verifica l'autorizzazione
            if (!IsAuthorized(Request))
            {
                return Unauthorized(); // Ritorna 401 Unauthorized se il token non è valido
            }

            var items = _dbContext.Items.ToList();
            return Json(items);
        }

        // GET api/item/{id}
        public IHttpActionResult GetItemById(int id)
        {
            // Verifica l'autorizzazione
            if (!IsAuthorized(Request))
            {
                return Unauthorized(); // Ritorna 401 Unauthorized se il token non è valido
            }

            var item = _dbContext.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}