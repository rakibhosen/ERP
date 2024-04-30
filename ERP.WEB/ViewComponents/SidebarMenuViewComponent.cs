using ERP.ENTITY.Models.Menu;
using ERP.SERVICE.IRepositories.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ERP.WEB.ViewComponents
{
    public class SidebarMenuViewComponent : ViewComponent
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMemoryCache _cache;
        public SidebarMenuViewComponent(IMenuRepository menuRepository, IMemoryCache cache)
        {
            _menuRepository = menuRepository;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheKey = "CACHE_MODULE_DATA";
            IEnumerable<ModuleInfo> moduleInfos;

            if (!_cache.TryGetValue(cacheKey, out moduleInfos))
            {
                moduleInfos = await _menuRepository.GetModules();
                _cache.Set(cacheKey, moduleInfos, TimeSpan.FromMinutes(200)); // Cache until session expires (200 minutes)
            }

            return View(moduleInfos);
        }


    }
}
