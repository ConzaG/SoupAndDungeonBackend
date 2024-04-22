using SoupAndDungeon.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Http.Cors;

namespace SoupAndDungeon.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class PlayerController : ApiController
    {
        private readonly SoupAndDungeonDBContext _dbContext = new SoupAndDungeonDBContext();
        private readonly string BearerToken; // Token bearer per l'autenticazione

        public PlayerController()
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
                    var token = authToken.Substring(7);
                    if (token == BearerToken)
                    {
                        return true; // Il token è valido
                    }
                }
            }
            return false; // Il token non è valido
        }

        // GET api/player
        public IHttpActionResult GetPlayers()
        {
            // Verifica l'autorizzazione
            if (!IsAuthorized(Request))
            {
                return Unauthorized(); // Ritorna 401 Unauthorized se il token non è valido
            }

            var players = _dbContext.Players.ToList();
            return Json(players);
        }

        // GET api/player/{id}
        public IHttpActionResult GetPlayerById(int id)
        {
            // Verifica l'autorizzazione
            if (!IsAuthorized(Request))
            {
                return Unauthorized(); // Ritorna 401 Unauthorized se il token non è valido
            }

            var player = _dbContext.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }
            return Json(player);
        }

        // POST api/player
        public IHttpActionResult PostPlayer(Players player)
        {
            // Verifica l'autorizzazione
            if (!IsAuthorized(Request))
            {
                return Unauthorized(); // Ritorna 401 Unauthorized se il token non è valido
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.Players.Add(player);
            _dbContext.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = player.Id }, player);
        }

        // PUT api/player/{id}

        public IHttpActionResult PutPlayer(int id, Players player)
        {
            // Verifica l'autorizzazione
            if (!IsAuthorized(Request))
            {
                return Unauthorized(); // Ritorna 401 Unauthorized se il token non è valido
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != player.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(player).State = EntityState.Modified;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Players.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE api/player/{id}
        public IHttpActionResult DeletePlayer(int id)
        {
            // Verifica l'autorizzazione
            if (!IsAuthorized(Request))
            {
                return Unauthorized(); // Ritorna 401 Unauthorized se il token non è valido
            }

            var player = _dbContext.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }

            _dbContext.Players.Remove(player);
            _dbContext.SaveChanges();

            return Ok(player);
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