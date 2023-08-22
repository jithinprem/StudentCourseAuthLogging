using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCourseWithAuth.Logger;
using StudentCourseWithAuth.Models;

namespace StudentCourseWithAuth.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class EnrollmentsController : Controller
    {
        private IRepository<Enrollment> _repository;
        private readonly IMyLogger _logger;
        public EnrollmentsController(IRepository<Enrollment> repository, IMyLogger logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View(_repository.GetAll());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Enrollment enrollment)
        {
            try
            {
                _repository.Add(enrollment);
                _logger.LogInfo($"Successfully added enrollment {enrollment.EnrollId}");
                return View("Index", _repository.GetAll());
            }
            catch {
                _logger.LogError($"Failed adding enrollment {enrollment.EnrollId}");
            }
            return Redirect("/Menu/Index");
            
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            try
            {
                var student = _repository.GetById(id);
                _logger.LogInfo($"Successful update enrollment {id}");
                return View(student);
            }
            catch {
                _logger.LogError($"Failed update enrollment {id}");
            }
            return Redirect("/Menu/Index");
            
        }

        [HttpPost]
        public IActionResult Update(Enrollment enrollment)
        {
            try
            {
                _repository.Update(enrollment);
                _logger.LogInfo($"Successful update enrollment {enrollment.EnrollId}");
                return View("Index", _repository.GetAll());
            }
            catch {
                _logger.LogError($"Failed update enrollment {enrollment.EnrollId}");
            }
            return Redirect("/Menu/Index");
          
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                _logger.LogInfo($"Successful Delete enrollment {id}");
                return View("Index", _repository.GetAll());
            }
            catch {
                _logger.LogError($"Failed delete enrollment {id}");
            }
            return Redirect("/Menu/Index");
            
        }
    }
}
