using BLL.Manager;
using Core.View.DayType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkingHoursAPI.Helper;

namespace WorkingHoursAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("day-type")]
    public class DayTypeController : ControllerBase
    {
        private readonly DayTypeManager _manager;
        public DayTypeController(DayTypeManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAsync()
        {
            var user = this.GetUser();
            var dto = await _manager.GetByUserAsync(user.UserID);
            return this.CreateResponse(dto);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(DayTypeView requestView)
        {
            var user = this.GetUser();
            var dto = await _manager.CreateAsync(user.UserID, requestView);
            return this.CreateResponse(dto);
        }

        [HttpPut]
        [Route("{dayTypeID}")]
        public async Task<IActionResult> UpdateAsync(long dayTypeID, DayTypeView requestView)
        {
            var user = this.GetUser();
            var dto = await _manager.UpdateAsync(user.UserID, dayTypeID, requestView);
            return this.CreateResponse(dto);
        }

        [HttpDelete]
        [Route("{dayTypeID}")]
        public async Task<IActionResult> DeleteAsync(long dayTypeID)
        {
            var dto = await _manager.DeleteAsync(dayTypeID);
            return this.CreateResponse(dto);
        }
    }
}
