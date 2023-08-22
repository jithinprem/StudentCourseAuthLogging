using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCourseWithAuth.Logger;
using StudentCourseWithAuth.Models;

namespace StudentCourseWithAuth.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IRepository<Course> _repository;
        private readonly IMyLogger _logger;
        public CoursesController(IRepository<Course> repository, IMyLogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_repository.GetAll());
        }

        [Authorize(Policy = "Experience")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Course course)
        {
            try{
                _repository.Add(course);
                _logger.LogInfo($"Successful adding course {course.CourseId}");
            }
            catch {
                _logger.LogError($" Couldn't add course {course.CourseId} ");
            }
            return View("Index", _repository.GetAll());

        }



        [HttpGet]
        public IActionResult Update(int id)
        {
            try
            {
                var course = _repository.GetById(id);
                return View(course);
            }
            catch {
                _logger.LogError($"Failed update course {id}");
            }
            return Redirect("/Menu/Index");
            
            
        }

        [HttpPost]
        public IActionResult Update(Course course)
        {
            try
            {
                _repository.Update(course);
                _logger.LogInfo($"Successful update course {course.CourseId}");
                return View("Index", _repository.GetAll());
            }
            catch {
                _logger.LogError($"Failed update course {course.CourseId}");
            }

            return Redirect("/Menu/Index");
  
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                _logger.LogInfo($"Successfully deleted course {id}");
                return View("Index", _repository.GetAll());
            }
            catch { 
                _logger.LogError($"Could not delete course {id}");
            }
            return Redirect("/Menu/Index");
        }



    }
}
