using ERP.ENTITY.Models.Menu;
using ERP.SERVICE.IRepositories.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ERP.WEB.Controllers
{
    public class MenuController : Controller
    {

        private readonly IMenuRepository _menuRepository;
        private readonly IMemoryCache _cache;

        public MenuController(IMenuRepository menuRepository, IMemoryCache cache)
        {
            _menuRepository = menuRepository;
            _cache = cache;
        }


        public IActionResult Index()
        {
 
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> GetMenuData(int moduleid)
        {
            string userid = "3101001";

            // Construct cache key based on moduleid and userid
            var cacheKey = $"CACHE_MENU_DATA_{moduleid}_{userid}";

            // Attempt to retrieve data from cache
            var menuData = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                // Configure cache expiration
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(200)); // Cache for 200 minutes
                return await _menuRepository.GetMenus(moduleid, userid);
            });

            return Json(menuData);
        }


    }
}
