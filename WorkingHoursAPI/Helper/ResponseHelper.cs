using Core.DTO;
using Microsoft.AspNetCore.Mvc;

namespace WorkingHoursAPI.Helper
{
    public static class ResponseHelper
    {
        public static IActionResult CreateResponse(this ControllerBase controllerBase, CommonDTO dto)
        {
            return dto.Status switch
            {
                CustomResponseStatus.OK => controllerBase.Ok(dto.View),
                CustomResponseStatus.NoContent => controllerBase.NoContent(),
                CustomResponseStatus.BadRequest => controllerBase.BadRequest(dto.View),
                CustomResponseStatus.Unauthorized => controllerBase.Unauthorized(),
                _ => throw new NotImplementedException()
            };
        }

        public static IActionResult CreateResponse<T>(this ControllerBase controllerBase, CommonContentDTO<T> dto)
        {
            return dto.Status switch
            {
                CustomResponseStatus.OK => controllerBase.Ok(dto.View),
                CustomResponseStatus.NoContent => controllerBase.NoContent(),
                CustomResponseStatus.BadRequest => controllerBase.BadRequest(dto.View),
                CustomResponseStatus.Unauthorized => controllerBase.Unauthorized(),
                _ => throw new NotImplementedException()
            };
        }

        public static IActionResult CreateResponse<T>(this ControllerBase controllerBase, CommonListDTO<T> dto)
        {
            return dto.Status switch
            {
                CustomResponseStatus.OK => controllerBase.Ok(dto.View),
                CustomResponseStatus.NoContent => controllerBase.NoContent(),
                CustomResponseStatus.BadRequest => controllerBase.BadRequest(dto.View),
                CustomResponseStatus.Unauthorized => controllerBase.Unauthorized(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
