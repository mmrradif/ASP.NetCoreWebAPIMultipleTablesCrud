using CoreWebAPI.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ShopDbContext _db;
        public CategoriesController(ShopDbContext db)
        {
            this._db = db;
        }


        public async Task<ActionResult<IEnumerable<Category>>> GetCategories(int id)
        {
            return await _db.Categories.ToListAsync();
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Category>> GetCategoryBYId(int id)
        {
            var category = await _db.Categories.FindAsync(id);

            if (category==null)
            {
                return NotFound();
            }
            return category;
        }
        

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _db.Categories.Add(category);

            await _db.SaveChangesAsync();

            return CreatedAtAction("GetCategoryBYId", new { id = category.CategoryId },category);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> PutCategory(int id,Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            _db.Entry(category).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
                return CreatedAtAction("GetCategoryBYId", new { id = category.CategoryId }, category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound(id);
                }
                else
                {
                    return NoContent();
                }               
            }        
        }
        private bool CategoryExists(int id)
        {
            return _db.Categories.Any(e => e.CategoryId == id);
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var contact = await _db.Categories.FindAsync(id);

            if (contact != null)
            {
                _db.Remove(contact);
                await _db.SaveChangesAsync();

                return Ok(contact);
            }
            return NotFound();
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<Category>>> DeleteCategory(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            var all = await _db.Categories.ToListAsync();

            if (category==null)
            {
                return NotFound();
            }
            _db.Remove(category);

            return all;
        }

    }
}
