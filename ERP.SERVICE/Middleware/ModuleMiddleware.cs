using ERP.ENTITY.Models.Menu;
using ERP.SERVICE.IRepositories.HRM;
using ERP.SERVICE.IRepositories.Menu;
using ERP.SERVICE.Repositories.Menu;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SERVICE.Middleware
{
    public class ModuleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;


        public ModuleMiddleware(RequestDelegate next, IMemoryCache memoryCache)
        {
            _next = next;
            _memoryCache = memoryCache;
            //_menuRepository = menuRepository;
        }

        public async Task Invoke(HttpContext context, IMenuRepository _menuRepository)
        {
            string cacheKey = "ModulesItem";

            IEnumerable<ModuleInfo> moduleInfos;

            // Check if menu items are already cached
            if (!_memoryCache.TryGetValue(cacheKey, out moduleInfos))
            {
                // Menu items are not in cache, fetch them from the database
                moduleInfos = await _menuRepository.GetModules();

                // Cache the menu items with a specified cache duration
                _memoryCache.Set(cacheKey, moduleInfos, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
            }


            // Add menu items to the request context
            context.Items["ModulesItem"] = moduleInfos;

            await _next(context);
        }
    }

}
