using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SampleWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ReallyWeirdAsyncService _service;
        private static readonly Random _rando = new Random();

        public IndexModel(ILogger<IndexModel> logger, ReallyWeirdAsyncService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult OnGetLogMessage()
        {
            using (_logger.BeginScope("Scope"))
            {
                _logger.LogInformation(new EventId(1, "MyEvent"), "The next random value is: {rando}", _rando.Next());
            }

            return Page();
        }

        public async Task<IActionResult> OnGetGarbageCollectAsync()
        {
            var arrays = new List<byte[]>();
            for(var i = 0; i < 1000; i ++)
            {
                arrays.Add(new byte[1024]);
            }
            await Task.Delay(5000);

            GC.Collect();

            return Page();
        }

        public IActionResult OnGetAsyncRelease()
        {
            _service.Release();

            return Page();
        }

        public async Task<IActionResult> OnGetAsyncHang()
        {
            await _service.WaitAsync();

            return Page();
        }
    }
}
