using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using chat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace chat.Pages
{
    public class IndexModel : PageModel
    {

        public string Message { get; private set; } = "PageModel in C#";

        public void OnGet()
        {

        }
        [BindProperty]
        public Info info { get; set; }
        public IActionResult OnPost(Info i)
        {


            if(i.RoomName == null)
            {
                var r = new Random();
                i.RoomName =  r.Next(1000,9999).ToString();
            }

            


            return Redirect($"/Chat?UserName={i.UserName}&RoomName={i.RoomName}");
        }
    }
}
