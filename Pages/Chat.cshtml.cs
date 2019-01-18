using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chat.Models;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace chat.Pages
{
    public class ChatModel : PageModel
    {
        [FromQuery]
        public Info Info { get; set; }
        public void OnGet()
        {


        }
    }
}
