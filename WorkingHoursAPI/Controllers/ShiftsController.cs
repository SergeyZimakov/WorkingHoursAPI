using BLL.Manager;
using Core.View.Shift;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkingHoursAPI.Helper;

namespace WorkingHoursAPI.Controllers
{
    [ApiController]
    [Route("shift")]
    [Authorize]
    public class ShiftsController : ControllerBase
    {
        private readonly ShiftsManager _manager;
        public ShiftsController(ShiftsManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [Route("daily")]
        public async Task<IActionResult> GetDailyDataAsync([FromQuery] DateOnly date)
        {
            var user = this.GetUser();
            var dto = await _manager.GetDailyDataAsync(user.UserID, date);
            return this.CreateResponse(dto);
        }

        [HttpGet]
        [Route("balance")]
        public async Task<IActionResult> GetCurrentBalanceAsync([FromQuery] int year, [FromQuery] int month)
        {
            var user = this.GetUser();
            var dto = await _manager.GetCurrentBalanceAsync(user.UserID, year, month);
            return this.CreateResponse(dto);
        }

        [HttpGet]
        [Route("monthly")]
        public async Task<IActionResult> GetMonthlyDataAsync([FromQuery]  int year, [FromQuery]  int month)
        {
            var user = this.GetUser();
            var dto = await _manager.GetMonthlyDataAsync(user.UserID, year, month);
            return this.CreateResponse(dto);
        }

        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> StartShiftAsync(StartShiftView requestView)
        {
            var user = this.GetUser();
            var dto = await _manager.StartShiftAsync(user.UserID, requestView);
            return this.CreateResponse(dto);
        }

        [HttpPost]
        [Route("{shiftID}/stop")]
        public async Task<IActionResult> StopShiftAsync(long shiftID, DateTimeView requestView)
        {
            var user = this.GetUser();
            var dto = await _manager.StopShiftAsync(user.UserID, shiftID, requestView);
            return this.CreateResponse(dto);
        }

        [HttpPost]
        [Route("{shiftID}/pause/start")]
        public async Task<IActionResult> StartPauseAsync(long shiftID, DateTimeView requestView)
        {
            var user = this.GetUser();
            var dto = await _manager.StartPauseAsync(user.UserID, shiftID, requestView);
            return this.CreateResponse(dto);
        }

        [HttpPost]
        [Route("{shiftID}/pause/stop")]
        public async Task<IActionResult> StopPauseAsync(long shiftID, DateTimeView requestView)
        {
            var user = this.GetUser();
            var dto = await _manager.StopPauseAsync(user.UserID, shiftID, requestView);
            return this.CreateResponse(dto);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> UpdateAsync(ShiftView requestView)
        {
            var user = this.GetUser();
            var dto = await _manager.CreateAsync(user.UserID, requestView);
            return this.CreateResponse(dto);
        }

        [HttpPut]
        [Route("{shiftID}")]
        public async Task<IActionResult> UpdateAsync(long shiftID, ShiftView requestView)
        {
            var user = this.GetUser();
            var dto = await _manager.UpdateAsync(user.UserID, shiftID, requestView);
            return this.CreateResponse(dto);
        }

        [HttpDelete]
        [Route("{shiftID}")]
        public async Task<IActionResult> DeleteAsync(long shiftID)
        {
            var user = this.GetUser();
            var dto = await _manager.DeleteAsync(user.UserID, shiftID);
            return this.CreateResponse(dto);
        }
    }
}
