using DataFlowAPI.DTOs;
using DataFlowAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataFlowAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataflowController : ControllerBase
    {
        private readonly ILogger<DataflowController> _logger;
        private readonly IDataflowService _service;

        public DataflowController(ILogger<DataflowController> logger, IDataflowService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> MergeCsvByIdColumn([FromQuery] RequestDto dto)
        {
            if(dto == null)
            {
                _logger.LogWarning("Null argument of type {arg}", typeof(RequestDto));
                return BadRequest("Please provide the filepath for the datasource and the delimeter type");
            }
            if (string.IsNullOrEmpty(dto.Filepath))
            {
                _logger.LogWarning("Null or empty property {prop} in the query of type {type}", nameof(dto.Filepath), typeof(RequestDto));
                return BadRequest("Please provide the filepath for the datasource");
            }
            if (dto.Delimeter == default(char))
            {
                _logger.LogWarning("Default property {prop} in the query of type {type}", nameof(dto.Delimeter), typeof(RequestDto));
                return BadRequest("Please provide the delimeter type for the data");
            }

            var resultPath = await _service.ProcessDataAsync(dto.Filepath, dto.Delimeter);

            if (resultPath == null)
            {
                _logger.LogInformation("No resources have been found, pathfile: {path}", dto.Filepath);
                return NotFound($"No resources found in the file path: {dto.Filepath}");
            }
            return Ok(resultPath);
        }
    }
}