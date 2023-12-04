using HouseholdOnlineStore.Data;
using HouseholdOnlineStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HouseholdOnlineStore.Controllers
{
	public class Lab6Controller : Controller
	{
		private readonly AppDBContext _context;
		private readonly Stopwatch watch;
		private readonly Random rand;

		public Lab6Controller(AppDBContext context)
        {
			rand = new();
			_context = context;
			watch = new();
		}

		public async Task<IActionResult> Index()
		{
			List<Lab6> labs;

			watch.Start();
			labs = _context.Lab6s.Take(20).ToList();
			watch.Stop();
			ViewBag.SelectSync20 = watch.Elapsed;
			watch.Reset();

			watch.Start();
			labs = await _context.Lab6s.Take(20).ToListAsync();
			watch.Stop();
			ViewBag.SelectAsync20 = watch.Elapsed;
			watch.Reset();

			watch.Start();
			Parallel.Invoke(() => labs = _context.Lab6s.Take(20).ToList());
			watch.Stop();
			ViewBag.SelectParallel20 = watch.Elapsed;
			watch.Reset();

			

			watch.Start();
			labs = _context.Lab6s.ToList();
			watch.Stop();
			ViewBag.SelectSync100000 = watch.Elapsed;
			watch.Reset();

			watch.Start();
			labs = await _context.Lab6s.ToListAsync();
			watch.Stop();
			ViewBag.SelectAsync100000 = watch.Elapsed;
			watch.Reset();

			watch.Start();
			Parallel.Invoke(() => labs = _context.Lab6s.ToList());
			watch.Stop();
			ViewBag.SelectParallel100000 = watch.Elapsed;
			watch.Reset();

			return View();
		}

		public async Task<IActionResult> Insert()
		{
			_context.Database.ExecuteSqlRaw("TRUNCATE TABLE Lab6s");

			Lab6[] labs = new Lab6[20];

			for (int i = 0; i < labs.Length; i++)
			{
				labs[i] = new()
				{
					Name = $"Item {i}",
					Number = rand.Next(100)
				};
			}

			watch.Start();
			_context.Lab6s.AddRange(labs);
			_context.SaveChanges();
			watch.Stop();
			ViewBag.Sync20 = watch.Elapsed;
			watch.Reset();

			//_context.Database.ExecuteSqlRaw("TRUNCATE TABLE Lab6s");

			for (int i = 0; i < labs.Length; i++)
			{
				labs[i] = new()
				{
					Name = $"Item {i}",
					Number = rand.Next(100)

				};
			}

			watch.Start();
			await _context.Lab6s.AddRangeAsync(labs);
			await _context.SaveChangesAsync();
			watch.Stop();
			ViewBag.Async20 = watch.Elapsed;
			watch.Reset();

			//_context.Database.ExecuteSqlRaw("TRUNCATE TABLE Lab6s");

			for (int i = 0; i < labs.Length; i++)
			{
				labs[i] = new()
				{
					Name = $"Item {i}",
					Number = rand.Next(100)
				};
			}

			watch.Start();
			Parallel.Invoke(() => _context.Lab6s.AddRange(labs));
			Parallel.Invoke(() => _context.SaveChanges());
			watch.Stop();
			ViewBag.Parallel20 = watch.Elapsed;
			watch.Reset();

			List<Lab6> list;

			watch.Start();
			list = _context.Lab6s.Take(20).ToList();
			watch.Stop();
			ViewBag.SelectSync20 = watch.Elapsed;
			watch.Reset();

			watch.Start();
			list = await _context.Lab6s.Take(20).ToListAsync();
			watch.Stop();
			ViewBag.SelectAsync20 = watch.Elapsed;
			watch.Reset();

			watch.Start();
			Parallel.Invoke(() => list = _context.Lab6s.Take(20).ToList());
			watch.Stop();
			ViewBag.SelectParallel20 = watch.Elapsed;
			watch.Reset();

			labs = new Lab6[100000];

			for (int i = 0; i < labs.Length; i++)
			{
				labs[i] = new()
				{
					Name = $"Item {i}",
					Number = rand.Next(100)
				};
			}

			watch.Start();
			_context.Lab6s.AddRange(labs);
			_context.SaveChanges();
			watch.Stop();
			ViewBag.Sync100000 = watch.Elapsed;
			watch.Reset();

			for (int i = 0; i < labs.Length; i++)
			{
				labs[i] = new()
				{
					Name = $"Item {i}",
					Number = rand.Next(100)
				};
			}

			watch.Start();
			await _context.Lab6s.AddRangeAsync(labs);
			await _context.SaveChangesAsync();
			watch.Stop();
			ViewBag.Async100000 = watch.Elapsed;
			watch.Reset();

			for (int i = 0; i < labs.Length; i++)
			{
				labs[i] = new()
				{
					Name = $"Item {i}",
					Number = rand.Next(100)
				};
			}

			watch.Start();
			Parallel.Invoke(() => _context.Lab6s.AddRange(labs));
			Parallel.Invoke(() => _context.SaveChanges());
			watch.Stop();
			ViewBag.Parallel100000 = watch.Elapsed;
			watch.Reset();

			watch.Start();
			list = _context.Lab6s.Take(100000).ToList();
			watch.Stop();
			ViewBag.SelectSync100000 = watch.Elapsed;
			watch.Reset();

			watch.Start();
			list = await _context.Lab6s.Take(100000).ToListAsync();
			watch.Stop();
			ViewBag.SelectAsync100000 = watch.Elapsed;
			watch.Reset();

			watch.Start();
			Parallel.Invoke(() => list = _context.Lab6s.Take(100000).ToList());
			watch.Stop();
			ViewBag.SelectParallel100000 = watch.Elapsed;
			watch.Reset();

			_context.Database.ExecuteSqlRaw("TRUNCATE TABLE Lab6s");
			return View();
		}

		public async Task<IActionResult> Insert100000()
		{
			_context.Database.ExecuteSqlRaw("TRUNCATE TABLE Lab6s");

			Lab6[] labs = new Lab6[100000];

			for (int i = 0; i < labs.Length; i++)
			{
				labs[i] = new()
				{
					Name = $"Item {i}",
					Number = rand.Next(100)
				};
			}

			await _context.Lab6s.AddRangeAsync(labs);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}
	}
}
