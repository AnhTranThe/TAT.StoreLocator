using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Store;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Core.Models.Response.User;
using TAT.StoreLocator.Infrastructure.Services;

namespace TAT.StoreLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        //Depency Injection
        private readonly IStoreService _storeService;
        //=>  private readonly IStoreService _storeService : bảo đảm rằng _storeService chỉ có thể đc gán 1 lần, và sau đó k 
        // thể thay đổi giá trị của nó 

        // + readonly : là một modifier(bộ điều chỉnh) , nó chỉ ra rằng giá trị của trường chỉ có thể được gán một lần
        //và chỉ có thể được thiết lập trong constructor hoặc khi khai báo trường đó

        // + IStoreService : là một giao diện(interface) định nghĩa các phương thức và thuộc tính để thực hiện các hoạt động

        //Constructor
        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        //=> Constructor này nhận một đối tượng IStoreService thông qua dependency injection
        //  và lưu trữ nó trong một trường _storeService để sử dụng trong các phương thức khác của StoreController

        [HttpPost("create")]
        //        //=> Đây là attribute để đánh dấu action method như một endpoint của API được truy cập bằng phương thức HTTP POST.
        //        //create/{storeId}: là phần của đường dẫn URL, nơi {storeId} là một tham số đường dẫn tùy chọn mà  có thể truy cập từ trong phương thức.
        public async Task<IActionResult> CreateStore([FromBody] CreateStoreRequestModel request)
        //// => Khai báo của action method CreateStore, nhận vào 1 đối tượng CreateStoreRequestModel từ phần thân yêu cầu ( request body)
        ////   public async Task<IActionResult>: chỉ rằng method sẽ trả về IActionResult, cho phép trả về kết quả khác nhau như Ok,BadRequest,NotFound

        {
            try
            {
                var response = await _storeService.CreateStoreAsync(request);
                //        //                //=>Trong khối try, action gọi phương thức CreateStoreAsync của service _storeService,
                //        //                    //  truyền vào đối tượng request để tạo một cửa hàng mới.
                //        //                     //   Phương thức này có thể thực hiện các thao tác như tạo một cửa hàng mới trong cơ sở dữ liệu
                //        //                //=> được sử dụng để trả về một phản hồi HTTP với mã trạng thái 201 (Created), cùng với thông tin chi tiết về cửa hàng được tạo ra.
                //        //                //+ CreatedAtAction:  là một phương thức trợ giúp của ASP.NET Core MVC,được sử dụng để tạo một phản hồi HTTP 
                //        //                    // với mã trạng thái 201 và cung cấp một địa chỉ URL tương ứng với một action method khác

                //        //                //+ nameof(GetStoreById): nameof: được sử dụng để lấy tên của phương thức GetStoreById,đảm bảo rằng tên 
                //        //                    // của phương thức không bị mã hóa ngầm trong chuỗi văn bản, giúp việc refactor dễ dàng hơn.

                //        //                //+ new { id = response.StoreId }: là một đối tượng kiểu anonymous object được sử dụng để truyền các tham số
                //        //                    // đường dẫn đến action method GetStoreById.Trong trường hợp này, chúng ta chỉ truyền
                //        //                        // một tham số là id với giá trị là response.StoreId.

                //        //                //+ response : Đối tượng response chứa thông tin chi tiết về cửa hàng đã được tạo ra, 
                //        //                    //    và nó sẽ được đính kèm vào phản hồi HTTP.
                if (response == null)
                {
                    return StatusCode(500, "Failed to create store");
                }

                return CreatedAtAction(nameof(GetAllStore), new { storeid = response.Id }, response.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            //    //            //=> Trong trường hợp có lỗi xảy ra trong quá trình thực thi action, nó sẽ bắt ngoại lệ
            //    //                //và trả về một phản hồi HTTP với status code 500(Internal Server Error), chứa thông điệp lỗi từ ngoại lệ.
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllStore()
        {
            try
            {
                var response = await _storeService.GetAllStoreAsync();
                if (response != null && response.Success)
                {
                    return Ok(response.Data);
                }
                return StatusCode(500, "Failed to get stores");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getDetail/{storeId}")]
        public async Task<IActionResult> GetDetailStore(string storeId)
        {
            try
            {
                var request = new GetDetailStoreRequestModel { Id = storeId };
                var response = await _storeService.GetDetailStoreAsync(storeId);
                if (response.Success)
                {
                    return Ok(response.Data);
                }
                else
                {
                    return NotFound(response.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update/{storeId}")]
        public async Task<IActionResult>UpdateStore ( string storeId,[FromBody]UpdateStoreRequestModel request)
        {
            try
            {
                var response = await _storeService.UpdateStoreAsync(storeId, request);
                if(response.Success)
                {
                    return Ok(response.Data);
                }
                else
                {
                    return StatusCode(500,response.Message);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("delete/{storeId}")]
        public async Task<IActionResult> DeleteStore(string storeId)
        {
            try
            {
                var response = await _storeService.DeleteStoreAsync(storeId);
                if (response.Success)
                {
                    return Ok("Store deleted successfully");
                }
                else
                {
                    return StatusCode(500, response.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
