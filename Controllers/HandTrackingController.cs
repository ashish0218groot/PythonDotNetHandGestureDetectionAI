using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PythonIntegrated.Services;
using System.Diagnostics;

namespace PythonIntegrated.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HandTrackingController : ControllerBase
    {
        private readonly HandTrackingService _handTrackingService;

        public HandTrackingController(HandTrackingService handTrackingService)
        {
            this._handTrackingService = handTrackingService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var handPosition = await _handTrackingService.GetHandPositionAsync();
            return Ok(handPosition);
        }

        [HttpGet("start-python-script")]
        public IActionResult StartPythonScript()
        {
            var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "lib", "hand_tracking.py");
            var start = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = scriptPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(start))
            {
                var result = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }

                return Ok(result);
            }
        }
    }
}
