using System;
using System.IO;
using System.Linq;
using interview_invitation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Interview_Invitation.Models;
using static System.Net.WebRequestMethods;
using System.Diagnostics;

public class InterviewController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public InterviewController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: Interview
    public async Task<IActionResult> Index()
    {
        var interviews = await _context.Interviews.ToListAsync(); ;
        return View(interviews);
    }

    // GET: Interview/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var interview = await _context.Interviews
            .Include(i => i.Subjects)
            .Include(i => i.Interviewees)
            .FirstOrDefaultAsync(m => m.InterviewId == id);

        if (interview == null)
        {
            return NotFound();
        }

        return View(interview);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Interview model)
    {
        if (model !=null && model.Interviewees[0].CvFile!=null)
        {
            var interviewees = new List<Interviewee>();

            foreach (var intervieweeModel in model.Interviewees)
            {
           
                    var cvFileName = Guid.NewGuid().ToString() + Path.GetExtension(intervieweeModel.CvFile.FileName);

                    var cvFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", cvFileName);

                    using (var stream = new FileStream(cvFilePath, FileMode.Create))
                    {
                        await intervieweeModel.CvFile.CopyToAsync(stream);
                    }

                    interviewees.Add(new Interviewee
                    {
                        FullName = intervieweeModel.FullName,
                        Email = intervieweeModel.Email,
                        ContactNumber = intervieweeModel.ContactNumber,
                        CvFilePath = Path.Combine("uploads", cvFileName)
                    });
                
            }

            var interview = new Interview
            {
                InterviewerName = model.InterviewerName,
                Date = model.Date,
                Duration = model.Duration,
                InterviewType = model.InterviewType,
                Subjects = model.Subjects.Select(s => new Subject
                {
                    Code = s.Code,
                    Title = s.Title,
                    Interviewer = s.Interviewer
                }).ToList(),
                Interviewees = interviewees
            };

            _context.Interviews.Add(interview);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // If ModelState is not valid, redisplay the form with validation errors
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var interview = await _context.Interviews
            .Include(i => i.Subjects)
            .Include(i => i.Interviewees)
            .FirstOrDefaultAsync(m => m.InterviewId == id);

        if (interview == null)
        {
            return NotFound();
        }

        return View(interview);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Interview model)
    {

            try
            {
            // Find the existing interview in the database
            var existingInterview = await _context.Interviews
                .Include(i => i.Subjects)  // Ensure Subjects are loaded
                .Include(i => i.Interviewees)  // Ensure Interviewees are loaded
                .FirstOrDefaultAsync(m => m.InterviewId == id);

            if (existingInterview == null)
            {
                return NotFound();
            }

            // Update properties of the existing interview
            existingInterview.InterviewerName = model.InterviewerName;
                existingInterview.Date = model.Date;
                existingInterview.Duration = model.Duration;
                existingInterview.InterviewType = model.InterviewType;

            // Check if Subjects is null, and if so, initialize it
            if (existingInterview.Subjects == null)
            {
                existingInterview.Subjects = new List<Subject>();
            }

            // Clear and update Subjects
            existingInterview.Subjects.Clear();
            existingInterview.Subjects.AddRange(model.Subjects.Select(s => new Subject
            {
                Code = s.Code,
                Title = s.Title,
                Interviewer = s.Interviewer
            }));


            // Update Interviewees
            for (int i = 0; i < existingInterview.Interviewees.Count && i < model.Interviewees.Count; i++)
                {
                    var existingInterviewee = existingInterview.Interviewees[i];
                    var intervieweeModel = model.Interviewees[i];

                    existingInterviewee.FullName = intervieweeModel.FullName;
                    existingInterviewee.Email = intervieweeModel.Email;
                    existingInterviewee.ContactNumber = intervieweeModel.ContactNumber;

                    // Check if a new file is uploaded
                    if (intervieweeModel.CvFile != null)
                    {
                        var cvFileName = Guid.NewGuid().ToString() + Path.GetExtension(intervieweeModel.CvFile.FileName);
                        var cvFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", cvFileName);

                        using (var stream = new FileStream(cvFilePath, FileMode.Create))
                        {
                            await intervieweeModel.CvFile.CopyToAsync(stream);
                        }

                        // Update the CvFilePath with the new file path
                        existingInterviewee.CvFilePath = Path.Combine("uploads", cvFileName);
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues if necessary
                return NotFound();
            }
   
    }


    // GET: Interview/Delete
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var interview = await _context.Interviews
            .Include(i => i.Subjects)
            .Include(i => i.Interviewees)
            .FirstOrDefaultAsync(m => m.InterviewId == id);

        if (interview == null)
        {
            return NotFound();
        }

        return View(interview);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var interview = await _context.Interviews
            .Include(i => i.Interviewees)  // Ensure Interviewees are loaded
            .FirstOrDefaultAsync(m => m.InterviewId == id);

        if (interview == null)
        {
            return NotFound();
        }

        // Delete CvFiles for each Interviewee
        if (interview.Interviewees != null)
        {
            foreach (var interviewee in interview.Interviewees)
            {
                if (!string.IsNullOrEmpty(interviewee.CvFilePath))
                {
                    DeleteCvFile(interviewee.CvFilePath);
                }
            }
        }

        _context.Interviews.Remove(interview);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    // Method to delete CvFile
    private void DeleteCvFile(string filePath)
    {
        var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }

}
