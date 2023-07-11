using BaseProjectApp.Library.DbSpsResult;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace BaseProjectApp.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUnitofWork _repositories = null;

        [BindProperty]
        public string DataObj { get; set; }
  
        public IndexModel(IUnitofWork repositories,IConfiguration configuration, ILogger<IndexModel> logger)
        {
            _repositories = repositories;
            _configuration = configuration;
            _logger = logger;
        }

        public void OnGet()
        {
            ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
             
            //var Data = _repositories.globalRepo.GetDashboardData(); 
            //DataObj = Data != null ? JsonConvert.SerializeObject(Data) : JsonConvert.SerializeObject(new DashboardChartsDataResult());
        }
    }
}