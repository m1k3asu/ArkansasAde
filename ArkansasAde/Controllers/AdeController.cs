using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProjectArkansasAde.Data;
using ProjectArkansasAde.Models;
using Microsoft.EntityFrameworkCore;
using ArkansasAde.Models;

namespace ProjectArkansasAde.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    
    public class AdeController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public AdeController(DataContext context)
        {
            _dataContext = context;
        }


        [HttpGet]
        [Route("GetDepartments")]

        // Using Action Result here insead of the IActionResult Inface so I can see the results in the swagger UI
        public async Task<ActionResult<List<Department>>> GetDepartments()
        {
            var toReturn = await _dataContext.Departments.ToListAsync();
            return Ok(toReturn);
        }

        [HttpGet]
        [Route("GetEmployees")]

        public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
        {
            var toReturn = new List<EmployeeDto>();
            var employees = await _dataContext.Employees.ToListAsync();
            var departments = await _dataContext.Departments.ToListAsync();
            employees.ForEach(employee =>
            {
                //var departmentInfo = departments.Where(w => w.Id == employee.Id).FirstOrDefault();
                var departmentInfo = departments.Where(w => w.Id == employee.DepartmentId).FirstOrDefault();
                var employeeDto = new EmployeeDto()
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Id = employee.Id,
                    DepartmentId = departmentInfo.Id,
                    DepartmentName = departmentInfo.Name,
                    Phone = employee.Phone
                };
                toReturn.Add(employeeDto);
            });

            return Ok(toReturn);
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<ActionResult<Employee>> AddEmployee(EmployeeDto dto)
        {
            //Note: Assume for sake of Time purposes the validation is done on the front and everyhing passes

            var departmentId = _dataContext.Departments.Where(w => w.Name == dto.DepartmentName).Select(s => s.Id).FirstOrDefault();
            var employee = new Employee()
            { 
                FirstName = dto.FirstName, 
                LastName = dto.LastName,
                Phone = dto.Phone,
                Email = dto.Email,
                DepartmentId = departmentId
            };
            
            _dataContext.Employees.Add(employee);
            await _dataContext.SaveChangesAsync();
            return Ok(employee);
        }


        [HttpPut]
        // Using Action Result here insead of the IActionResult Inface so I can see the results in the swagger UI
        [Route("UpdateEmployees")]
        public async Task<ActionResult<List<Employee>>> UpdateEmployees(List<EmployeeDto> dto)
        {
            dto.ForEach(emp =>
            {
                // Update First, Last, Phone and Email
                var departmentId = _dataContext.Departments.Where(w => w.Name == emp.DepartmentName).Select(s => s.Id).FirstOrDefault();
                var employee = new Employee()
                {
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Phone = emp.Phone,
                    Email = emp.Email,
                    DepartmentId = departmentId,
                    Id = emp.Id
                };
                _dataContext.Employees.Update(employee);
            });
            
            await _dataContext.SaveChangesAsync();            
            return Ok(true);
        }
    }
}
