using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace BaseProjectApp.Admin.Pages._Shared
{
    public class _SharedModel : PageModel
    {
        public _SharedModel()
        {

        }

        public void OnGet()
        {   
        }

        public IActionResult OnGetLoadMediaFilesForm(int recordId, int mediaId, string pid, string type , bool language = false)
        {
            return Partial(
                "MediaFilesForm",
                new Microsoft.AspNetCore.Html.HtmlString(JsonConvert.SerializeObject(new {
                    pid = pid,
                    recordId = recordId,
                    type = type,
                    mediaId = mediaId,
                    language = language
                }))
            );
        }

        public IActionResult OnGetLoadFilesDropzone(int mediaId, string pid, string endpoint)
        {
            return Partial(
                "UploadFilesDropzone",
                new Microsoft.AspNetCore.Html.HtmlString(JsonConvert.SerializeObject(new {
                    pid = pid,
                    mediaId = mediaId,
                    endpoint = endpoint
                }))
            );
        }

        
    }
}
