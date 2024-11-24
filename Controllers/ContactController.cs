using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Api.AppData;
using Web_Api.DbModels;


namespace Web_Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	
	public class ContactsController : ControllerBase
	{
		private readonly AppDbContext _context;

		public ContactsController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/Contacts
		[HttpGet]
		public async Task<ActionResult<IEnumerable<PhoneBook>>> GetPhoneBooks()
		{
			var phoneBooks = await _context.PhoneBooks
										   .Where(p => !p.Deleted)
										   .ToListAsync();
			if (phoneBooks == null || !phoneBooks.Any())
			{
				return NotFound("!مخاطبی یافت نشد");
			}

			return Ok(phoneBooks);
		}
		// GET: api/Contacts/5
		[HttpGet("{id}")]
		public async Task<ActionResult<PhoneBook>> GetPhoneBook(int id)
		{
			var phoneBook = await _context.PhoneBooks.FindAsync(id);


			if (phoneBook == null || phoneBook.Deleted)
			{
				return NotFound(".کاربر مورد نظر یافت نشد");
			}

			return Ok(phoneBook);
		}

		// PUT: api/Contacts/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PhoneBookDTO(int id, [FromBody] PhoneBookDTO PhoneBookDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var phoneBook = await _context.PhoneBooks.FindAsync(id);
			if (phoneBook == null || phoneBook.Deleted)
			{
				return NotFound(".کاربر مورد نظر یافت نشد");
			}

			// به‌روزرسانی مقادیر بدون تغییر ID
			phoneBook.FirstName = PhoneBookDTO.FirstName;
			phoneBook.LastName = PhoneBookDTO.LastName;
			phoneBook.PhoneNumber = PhoneBookDTO.PhoneNumber;

			_context.PhoneBooks.Update(phoneBook);
			await _context.SaveChangesAsync();

			return Ok("اطلاعات مخاطب با موفقیت ویرایش شد.");
		}

		// POST: api/Contacts
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<PhoneBook>> PostPhoneBook(PhoneBookDTO PhoneBookDTO)
		{
			var phoneBook = new PhoneBook
			{
				FirstName = PhoneBookDTO.FirstName,
				LastName = PhoneBookDTO.LastName,
				PhoneNumber = PhoneBookDTO.PhoneNumber,
				Deleted = false
			};

			_context.PhoneBooks.Add(phoneBook);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetPhoneBook", new { id = phoneBook.ID }, phoneBook);
		}

		// DELETE: api/Contacts/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePhoneBook(int id)
		{
			var phoneBook = await _context.PhoneBooks.FindAsync(id);
			if (phoneBook == null || phoneBook.Deleted)
			{
				return NotFound("!کابر یافت نشد");
			}

			phoneBook.Deleted = true;

			_context.PhoneBooks.Update(phoneBook);
			await _context.SaveChangesAsync();

			// پس از حذف رکورد، آیدی‌ها را مجدداً تنظیم می‌کنیم.
			await ResetIds();

			// دستور DBCC CHECKIDENT برای ریست کردن مقدار آیدی
			var maxId = await _context.PhoneBooks.MaxAsync(c => (int?)c.ID) ?? 0;
			await _context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('PhoneBooks', RESEED, {maxId})");

			return Ok("مخاطب با موفقیت حذف شد.");
		}

		private async Task ResetIds()
		{
			var activeRecords = await _context.PhoneBooks
											   .Where(r => !r.Deleted)
											   .OrderBy(r => r.ID)
											   .ToListAsync();
			int id = 1;  

			foreach (var record in activeRecords)
			{
				record.ID = id++;  
			}

			await _context.SaveChangesAsync();
		}

		private bool PhoneBookExists(int id)
		{
			return _context.PhoneBooks.Any(e => e.ID == id);
		}
	}
}
