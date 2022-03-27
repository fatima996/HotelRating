#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelRating.Data;
using HotelRating.Models;

namespace HotelRating.Controllers
{
    public class HotelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Hotel.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            HotelDetails hotelDetails = new HotelDetails();
            hotelDetails.Id = hotel.Id;
            hotelDetails.Name = hotel.Name;
            hotelDetails.IsHalal = hotel.IsHalal;
            hotelDetails.AvgRating = GetAvgRating(id);
            hotelDetails.Image = hotel.Image;
            hotelDetails.Details = hotel.Details;
            return View(hotelDetails);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Details,IsHalal")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Details,Image,IsHalal")] Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotel = await _context.Hotel.FindAsync(id);
            _context.Hotel.Remove(hotel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
            return _context.Hotel.Any(e => e.Id == id);
        }

        public ActionResult GetRatings(int HotelId)
        {
            IEnumerable<Rating> listOfRatings = (from rating in _context.Rating
                                                 where rating.HotelId == HotelId
                                                 select new Rating()
                                                 {
                                                     HotelId = rating.HotelId,
                                                     Comment = rating.Comment,
                                                     Rate = rating.Rate,
                                                     Username = Guid.NewGuid().ToString()
                                                 }
            ).ToList();
            ViewBag.HotelId = HotelId;
            return View(listOfRatings);
        }
        [HttpPost]
        public ActionResult AddReview(int HotelId, int rating, string comment)
        {
            Rating obj = new Rating();
            obj.HotelId = HotelId;
            obj.Rate = rating;
            obj.Comment = comment;
            obj.Username = Guid.NewGuid().ToString();
            _context.Rating.Add(obj);
            _context.SaveChanges();
            return RedirectToAction("GetRatings");

        }

        private double GetAvgRating(int? HotelId)
        {
            IEnumerable<Rating> listOfRatings = (from rating in _context.Rating
                                                 where rating.HotelId == HotelId
                                                 select new Rating()
                                                 {
                                                   
                                                     Rate = rating.Rate,
                                                 }
            ).ToList();
            var sum = 0;
            foreach (var rate in listOfRatings)
            {
                sum = sum + rate.Rate;

              
               
            }   if (sum == 0)
                {
                    return 3.0;
                }
            return sum / listOfRatings.Count();
        }
    }
}
