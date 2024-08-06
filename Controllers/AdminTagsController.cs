using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;

        //ctor
        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        //kiem tra tag co ton tai trong db hay khong

        //create new tag
        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName

            };

            await bloggieDbContext.Tags.AddAsync(tag);

            await bloggieDbContext.SaveChangesAsync();

            return View("Add");
        }

        //get full tag
        [HttpGet]
        public async Task<IActionResult> ListTag()
        {
            var tag = await bloggieDbContext.Tags.ToListAsync();

            return View(tag);
        }


        //edit tag
        [HttpGet]
        public async Task<IActionResult> Edit (Guid id)
        {
            var tag = await bloggieDbContext.Tags.FindAsync(id);
            
            return View(tag);
        }

        //sua doi thong tin cua tag
        [HttpPost]
        public async Task<IActionResult> Edit (Tag viewModel)
        {
            var tag = await bloggieDbContext.Tags.FindAsync(viewModel.Id);

            if (tag is not null)
            {
                tag.Name = viewModel.Name;
                tag.DisplayName = viewModel.DisplayName;

                await bloggieDbContext.SaveChangesAsync();
            }

            return RedirectToAction("ListTag", "AdminTags");
        }

        //lay thong tin id cua tag de xoa
        [HttpPost]
        public async Task<IActionResult> Delete (Tag obj)
        {
            //tim tag bang id tuong ung
            var tag = await bloggieDbContext.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == obj.Id);

            if (tag is not null)
            {
                bloggieDbContext.Tags.Remove(tag);
                await bloggieDbContext.SaveChangesAsync();
            }

            return RedirectToAction("ListTag", "AdminTags");
        }


    }

}
