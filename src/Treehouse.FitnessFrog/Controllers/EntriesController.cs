using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Data;
using Treehouse.FitnessFrog.Models;

namespace Treehouse.FitnessFrog.Controllers
{
    public class EntriesController : Controller
    {
        private EntriesRepository _entriesRepository = null;

        public EntriesController()
        {
            _entriesRepository = new EntriesRepository();
        }

        public ActionResult Index()
        {
            List<Entry> entries = _entriesRepository.GetEntries();

            // Calculate the total activity.
            double totalActivity = entries
                .Where(e => e.Exclude == false)
                .Sum(e => e.Duration);

            // Determine the number of days that have entries.
            int numberOfActiveDays = entries
                .Select(e => e.Date)
                .Distinct()
                .Count();

            ViewBag.TotalActivity = totalActivity;
            ViewBag.AverageDailyActivity = (totalActivity / (double)numberOfActiveDays);

            return View(entries);
        }

        public ActionResult Add()
        {
            var entry = new Entry()
            {
                Date = DateTime.Today,
                //ActivityId = 2
            };

            SetupActivitieSelectListItems();

            return View(entry);
        }

        

        [HttpPost]
        public ActionResult Add(Entry entry)//(DateTime ? date, int ? activityId, double ? duration, Entry.IntensityLevel ? intensity, bool ? exclude, string notes)
        {
            //ModelState.AddModelError("", "This is a global message.");

            ValidateEntry(entry);

            if (ModelState.IsValid)
            {
                _entriesRepository.AddEntry(entry);

                TempData["Message"] = "Your entry was successfuly added!";

                return RedirectToAction("Index");
                //TODO Display the Entries list page
            }

            SetupActivitieSelectListItems();

            //string date = Request.Form["Date"]; //ActiityID, Duration, Intensity, Exlude, Notes
            //ViewBag.Date = ModelState["Date"].Value.AttemptedValue;
            //ViewBag.ActivityId = ModelState["ActivityId"].Value.AttemptedValue;
            //ViewBag.Duration = ModelState["Duration"].Value.AttemptedValue;
            //ViewBag.Intensity = ModelState["Intensity"].Value.AttemptedValue;
            //ViewBag.Exclude = ModelState["Exclude"].Value.AttemptedValue;
            //ViewBag.Notes = ModelState["Notes"].Value.AttemptedValue;

            return View(entry);
        }
      

        public ActionResult Edit(int? id) // ? means nullable, allows entries to be successfully passed action methods
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //TODO Get the requested entry from the repo
            Entry entry = _entriesRepository.GetEntry((int)id);

            //TODO Return a status of "not found" if the entry wasn't found
            if (entry == null)
            {
                return HttpNotFound();
            }

            //TODO Populate the activities select list items Viewbag property
            SetupActivitieSelectListItems();

            //TODO Pass the entry into the view
            return View(entry);
        }

        [HttpPost]
        public ActionResult Edit(Entry entry)
        {
            //TODO Validate the entry
            ValidateEntry(entry);

            //TODO if the entry is valid...
            //1. Use the repo to update the entry
            //2. Redirect the user to the "Entrie" list page
            if (ModelState.IsValid)
            {
                _entriesRepository.UpdateEntry(entry);

                TempData["Message"] = "Your entry was successfully updated!";

                return RedirectToAction("Index");
            }

            //TODO Populate the activites select list items ViewBag
            SetupActivitieSelectListItems();

            return View(entry);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //TODO Retreive entry for the provided if parameter value.
            Entry entry = _entriesRepository.GetEntry((int)id);

            //TODO Return "not found" if an entry wasn't found.
            if (entry == null)
            {

                return HttpNotFound();
            }

            //TODO Pass the entry to the view.
            return View(entry);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            //TODO Delete the entry
            _entriesRepository.DeleteEntry(id);

            TempData["Message"] = "Your entry was successfully deleted!";

            //Redirect to the "Entries" list page
            return RedirectToAction("Index");
        }

        private void ValidateEntry(Entry entry)
        {
            //If there aren't any "Duration" field validation erros, then make sure that the duration is grater than "0".
            if (ModelState.IsValidField("Duration") && entry.Duration <= 0)
            {
                ModelState.AddModelError("Duration", "The Duration field value must be greater than '0'.");

            }
        }

        private void SetupActivitieSelectListItems()
        {
            ViewBag.ActSelListItems = new SelectList(Data.Data.Activities, "Id", "Name");
        }

    } //END class
} //END namespace