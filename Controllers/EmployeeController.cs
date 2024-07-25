using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCRUD.Data;
using MVCCRUD.Models;
using MVCCRUD.Models.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MVCCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MVCdemoDbContext _context;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(MVCdemoDbContext context, ILogger<EmployeeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            if (ModelState.IsValid)
            {
                
                
                    var employee = new Employee
                    {
                        Id = addEmployeeRequest.Id,
                        Name = addEmployeeRequest.Name,
                        Email = addEmployeeRequest.Email,
                        Salary = addEmployeeRequest.Salary,
                        Department = addEmployeeRequest.Department,
                        DateOfBirth = addEmployeeRequest.DateOfBirth
                    };

                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                
               
            }

            return View(addEmployeeRequest);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            var editEmployeeViewModel = new EditEmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Salary = employee.Salary,
                Department = employee.Department,
                DateOfBirth = employee.DateOfBirth
            };
            return View(editEmployeeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeViewModel editEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = await _context.Employees.FindAsync(editEmployeeViewModel.Id);
                    if (employee == null)
                    {
                        return NotFound();
                    }

                    employee.Name = editEmployeeViewModel.Name;
                    employee.Email = editEmployeeViewModel.Email;
                    employee.Salary = editEmployeeViewModel.Salary;
                    employee.Department = editEmployeeViewModel.Department;
                    employee.DateOfBirth = editEmployeeViewModel.DateOfBirth;

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error occurred while editing employee.");
                    ModelState.AddModelError(string.Empty, "Unable to save changes due to concurrency issues. Please try again.");
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Error occurred while editing employee.");
                    ModelState.AddModelError(string.Empty, "Unable to save changes. Please try again.");
                }
            }

            return View(editEmployeeViewModel);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting employee.");
                ModelState.AddModelError(string.Empty, "Unable to delete the record. Please try again.");
            }

            return RedirectToAction("Index");
        }
    }
}
