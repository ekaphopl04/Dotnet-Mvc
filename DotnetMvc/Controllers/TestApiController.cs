using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DotnetMvc.Controllers
{
    /// <summary>
    /// Test API Controller สำหรับทดสอบฟีเจอร์ต่างๆ ของ ASP.NET Core Web API
    /// This controller demonstrates various API features including action return types,
    /// JSON Patch handling, response formatting, error handling, and custom formatters.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TestApiController : ControllerBase
    {
        private static List<TestModel> _testData = new List<TestModel>
        {
            new TestModel { Id = 1, Name = "Test Item 1", Description = "รายการทดสอบที่ 1", IsActive = true, CreatedDate = DateTime.Now.AddDays(-10) },
            new TestModel { Id = 2, Name = "Test Item 2", Description = "รายการทดสอบที่ 2", IsActive = false, CreatedDate = DateTime.Now.AddDays(-5) },
            new TestModel { Id = 3, Name = "Test Item 3", Description = "รายการทดสอบที่ 3", IsActive = true, CreatedDate = DateTime.Now.AddDays(-1) }
        };

        /// <summary>
        /// 1. Action Return Types - ตัวอย่างการใช้ Return Types ต่างๆ
        /// Demonstrates different action return types in ASP.NET Core Web API
        /// </summary>
        [HttpGet("return-types")]
        public ActionResult<ApiResponse<List<TestModel>>> GetReturnTypesExample()
        {
            try
            {
                // ActionResult<T> - ให้ความยืดหยุ่นในการ return ทั้ง T หรือ ActionResult
                // Allows returning either T directly or ActionResult for more control
                var response = new ApiResponse<List<TestModel>>
                {
                    Success = true,
                    Message = "ตัวอย่าง Action Return Types - สามารถ return ได้หลายแบบ",
                    Data = _testData,
                    Timestamp = DateTime.Now
                };

                return Ok(response); // Returns ActionResult with 200 status
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<TestModel>>
                {
                    Success = false,
                    Message = $"เกิดข้อผิดพลาด: {ex.Message}",
                    Data = null,
                    Timestamp = DateTime.Now
                });
            }
        }

        /// <summary>
        /// 2. Handle JSON Patch Requests - การจัดการ JSON Patch
        /// Demonstrates how to handle JSON Patch requests for partial updates
        /// </summary>
        [HttpPatch("json-patch/{id}")]
        public ActionResult<ApiResponse<TestModel>> HandleJsonPatch(int id, [FromBody] JsonPatchDocument<TestModel> patchDoc)
        {
            try
            {
                // ค้นหา item ที่ต้องการแก้ไข
                var item = _testData.FirstOrDefault(x => x.Id == id);
                if (item == null)
                {
                    return NotFound(new ApiResponse<TestModel>
                    {
                        Success = false,
                        Message = $"ไม่พบข้อมูลที่มี ID: {id}",
                        Data = null,
                        Timestamp = DateTime.Now
                    });
                }

                // สร้าง copy เพื่อใช้ในการ patch
                var itemToPatch = new TestModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    IsActive = item.IsActive,
                    CreatedDate = item.CreatedDate
                };

                // ใช้ JSON Patch Document เพื่ออัพเดทข้อมูล
                // Apply JSON Patch operations to the model
                patchDoc.ApplyTo(itemToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<TestModel>
                    {
                        Success = false,
                        Message = "ข้อมูลที่ส่งมาไม่ถูกต้อง",
                        Data = null,
                        Timestamp = DateTime.Now,
                        Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                    });
                }

                // อัพเดทข้อมูลจริง
                var index = _testData.FindIndex(x => x.Id == id);
                _testData[index] = itemToPatch;

                return Ok(new ApiResponse<TestModel>
                {
                    Success = true,
                    Message = "อัพเดทข้อมูลสำเร็จด้วย JSON Patch",
                    Data = itemToPatch,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<TestModel>
                {
                    Success = false,
                    Message = $"เกิดข้อผิดพลาดภายในเซิร์ฟเวอร์: {ex.Message}",
                    Data = null,
                    Timestamp = DateTime.Now
                });
            }
        }

        /// <summary>
        /// 3. Format Response Data - การจัดรูปแบบข้อมูล Response
        /// Demonstrates different response formatting options
        /// </summary>
        [HttpGet("formatted-response")]
        public ActionResult<object> GetFormattedResponse([FromQuery] string format = "standard")
        {
            try
            {
                var data = _testData.Take(2).ToList();

                switch (format.ToLower())
                {
                    case "minimal":
                        // รูปแบบข้อมูลแบบย่อ - Minimal data format
                        return Ok(data.Select(x => new { x.Id, x.Name }));

                    case "detailed":
                        // รูปแบบข้อมูลแบบละเอียด - Detailed data format
                        return Ok(new
                        {
                            TotalCount = data.Count,
                            Items = data,
                            Metadata = new
                            {
                                GeneratedAt = DateTime.Now,
                                Version = "1.0",
                                Format = "detailed"
                            }
                        });

                    case "xml":
                        // รูปแบบ XML - XML format response
                        return new ObjectResult(data)
                        {
                            ContentTypes = { "application/xml" }
                        };

                    default:
                        // รูปแบบมาตรฐาน - Standard format
                        return Ok(new ApiResponse<List<TestModel>>
                        {
                            Success = true,
                            Message = "ข้อมูลในรูปแบบมาตรฐาน",
                            Data = data,
                            Timestamp = DateTime.Now
                        });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"เกิดข้อผิดพลาดในการจัดรูปแบบข้อมูล: {ex.Message}",
                    Data = null,
                    Timestamp = DateTime.Now
                });
            }
        }

        /// <summary>
        /// 4. Handle Errors - การจัดการข้อผิดพลาด
        /// Demonstrates comprehensive error handling strategies
        /// </summary>
        [HttpGet("error-handling/{type}")]
        public ActionResult<ApiResponse<object>> DemonstrateErrorHandling(string type)
        {
            try
            {
                switch (type.ToLower())
                {
                    case "validation":
                        // ข้อผิดพลาดจากการตรวจสอบข้อมูล - Validation error
                        ModelState.AddModelError("TestField", "ค่าที่ส่งมาไม่ถูกต้อง");
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = "ข้อมูลไม่ถูกต้อง",
                            Data = null,
                            Timestamp = DateTime.Now,
                            Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                        });

                    case "notfound":
                        // ข้อผิดพลาดไม่พบข้อมูล - Not found error
                        return NotFound(new ApiResponse<object>
                        {
                            Success = false,
                            Message = "ไม่พบข้อมูลที่ต้องการ",
                            Data = null,
                            Timestamp = DateTime.Now
                        });

                    case "unauthorized":
                        // ข้อผิดพลาดไม่มีสิทธิ์ - Unauthorized error
                        return Unauthorized(new ApiResponse<object>
                        {
                            Success = false,
                            Message = "ไม่มีสิทธิ์ในการเข้าถึงข้อมูลนี้",
                            Data = null,
                            Timestamp = DateTime.Now
                        });

                    case "exception":
                        // จำลองข้อผิดพลาดที่ไม่คาดคิด - Simulate unexpected exception
                        throw new InvalidOperationException("ข้อผิดพลาดที่จำลองขึ้น");

                    default:
                        return Ok(new ApiResponse<object>
                        {
                            Success = true,
                            Message = "ไม่มีข้อผิดพลาด - ระบบทำงานปกติ",
                            Data = new { Status = "OK", AvailableErrorTypes = new[] { "validation", "notfound", "unauthorized", "exception" } },
                            Timestamp = DateTime.Now
                        });
                }
            }
            catch (Exception ex)
            {
                // การจัดการข้อผิดพลาดที่ไม่คาดคิด - Unexpected error handling
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "เกิดข้อผิดพลาดภายในเซิร์ฟเวอร์",
                    Data = null,
                    Timestamp = DateTime.Now,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// 5. Custom Formatters - การใช้ Custom Formatters
        /// Demonstrates custom response formatting
        /// </summary>
        [HttpGet("custom-format")]
        public ActionResult GetCustomFormattedData()
        {
            var data = _testData.First();
            
            // ส่งข้อมูลในรูปแบบที่กำหนดเอง - Custom formatted response
            return new ObjectResult(new
            {
                // รูปแบบข้อมูลแบบกำหนดเอง - Custom data structure
                ItemInfo = new
                {
                    Identifier = data.Id,
                    Title = data.Name,
                    Details = data.Description,
                    Status = data.IsActive ? "เปิดใช้งาน" : "ปิดใช้งาน",
                    Created = data.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss")
                },
                ResponseMeta = new
                {
                    ApiVersion = "1.0",
                    ResponseTime = DateTime.Now,
                    CustomFormatter = true,
                    Description = "ข้อมูลที่จัดรูปแบบด้วย Custom Formatter"
                }
            });
        }

        /// <summary>
        /// 6. Analyzers and Conventions - การใช้ Analyzers และ Conventions
        /// This method demonstrates proper API conventions and analyzer-friendly code
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<TestModel>>> CreateItem([FromBody] CreateTestModelRequest request)
        {
            // การตรวจสอบข้อมูลด้วย Data Annotations - Validation with Data Annotations
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<TestModel>
                {
                    Success = false,
                    Message = "ข้อมูลที่ส่งมาไม่ถูกต้อง",
                    Data = null,
                    Timestamp = DateTime.Now,
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            try
            {
                // สร้างข้อมูลใหม่ - Create new item
                var newItem = new TestModel
                {
                    Id = _testData.Max(x => x.Id) + 1,
                    Name = request.Name,
                    Description = request.Description,
                    IsActive = request.IsActive ?? true,
                    CreatedDate = DateTime.Now
                };

                _testData.Add(newItem);

                // ส่งกลับ 201 Created พร้อมข้อมูลที่สร้าง - Return 201 Created with created item
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = newItem.Id },
                    new ApiResponse<TestModel>
                    {
                        Success = true,
                        Message = "สร้างข้อมูลสำเร็จ",
                        Data = newItem,
                        Timestamp = DateTime.Now
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<TestModel>
                {
                    Success = false,
                    Message = $"เกิดข้อผิดพลาดในการสร้างข้อมูล: {ex.Message}",
                    Data = null,
                    Timestamp = DateTime.Now
                });
            }
        }

        /// <summary>
        /// Get item by ID - สำหรับใช้กับ CreatedAtAction
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<TestModel>> GetById(int id)
        {
            var item = _testData.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound(new ApiResponse<TestModel>
                {
                    Success = false,
                    Message = $"ไม่พบข้อมูลที่มี ID: {id}",
                    Data = null,
                    Timestamp = DateTime.Now
                });
            }

            return Ok(new ApiResponse<TestModel>
            {
                Success = true,
                Message = "ค้นหาข้อมูลสำเร็จ",
                Data = item,
                Timestamp = DateTime.Now
            });
        }

        /// <summary>
        /// Get all items with pagination - รายการทั้งหมดพร้อม Pagination
        /// </summary>
        [HttpGet]
        public ActionResult<ApiResponse<PagedResult<TestModel>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var totalItems = _testData.Count;
                var items = _testData
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var pagedResult = new PagedResult<TestModel>
                {
                    Items = items,
                    TotalItems = totalItems,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
                };

                return Ok(new ApiResponse<PagedResult<TestModel>>
                {
                    Success = true,
                    Message = "ดึงข้อมูลสำเร็จ",
                    Data = pagedResult,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PagedResult<TestModel>>
                {
                    Success = false,
                    Message = $"เกิดข้อผิดพลาดในการดึงข้อมูล: {ex.Message}",
                    Data = null,
                    Timestamp = DateTime.Now
                });
            }
        }
    }

    /// <summary>
    /// Test Model สำหรับทดสอบ API
    /// </summary>
    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// Request model สำหรับการสร้างข้อมูลใหม่
    /// </summary>
    public class CreateTestModelRequest
    {
        [Required(ErrorMessage = "ชื่อเป็นข้อมูลที่จำเป็น")]
        [StringLength(100, ErrorMessage = "ชื่อต้องมีความยาวไม่เกิน 100 ตัวอักษร")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "คำอธิบายต้องมีความยาวไม่เกิน 500 ตัวอักษร")]
        public string Description { get; set; } = string.Empty;

        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// Generic API Response wrapper
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; }
        public List<string>? Errors { get; set; }
    }

    /// <summary>
    /// Paged result wrapper
    /// </summary>
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
