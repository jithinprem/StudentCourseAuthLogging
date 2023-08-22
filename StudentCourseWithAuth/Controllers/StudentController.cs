using Microsoft.AspNetCore.Mvc;
using StudentCourseWithAuth.Logger;
using StudentCourseWithAuth.Models;

namespace StudentCourseWithAuth.Controllers
{
    public class StudentsController : Controller
    {
        private IRepository<Student> _repository;
        private readonly IMyLogger _logger;
        public StudentsController(IRepository<Student> repository, IMyLogger logger)
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
        public IActionResult Add(Student student)
        {
            try
            {
                _repository.Add(student);
                _logger.LogInfo($"Successfully added student {student.StudentId}");
                return View("Index", _repository.GetAll());
            }
            catch {
                _logger.LogError($"Failed adding student {student.StudentId}");
            }
            return Redirect("/Menu/Index");
            
        }

        [HttpGet]
        public IActionResult Update(int id)
        {

            try
            {
                var student = _repository.GetById(id);
                return View(student);
            }
            catch {
                _logger.LogError($"Failed student update {id}");
            }
            return Redirect("/Menu/Index");
            
        }

        [HttpPost]
        public IActionResult Update(Student student)
        {
            try
            {
                _repository.Update(student);
                _logger.LogInfo($"Successful update student {student.StudentId}");
                return View("Index", _repository.GetAll());
            }
            catch {
                _logger.LogError($"Failed update student {student.StudentId}");
            }
            return Redirect("/Menu/Index");
            
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                _logger.LogInfo($"Successfully deleted student {id}");
                return View("Index", _repository.GetAll());
            }
            catch { 
                _logger.LogError($"Failed deleting student {id}");
            }
            return Redirect("/Menu/Index");
            
        }


    }
}
