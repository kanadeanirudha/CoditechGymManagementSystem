using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
namespace Coditech.Admin.Controllers
{
    public class GymBodyMeasurementTypeController : BaseController
    {
        private readonly IGymBodyMeasurementTypeAgent _gymBodyMeasurementTypeAgent;
        private const string createEdit = "~/Views/Gym/GymBodyMeasurementType/CreateEdit.cshtml";

        public GymBodyMeasurementTypeController(IGymBodyMeasurementTypeAgent gymBodyMeasurementTypeAgent)
        {
            _gymBodyMeasurementTypeAgent = gymBodyMeasurementTypeAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            GymBodyMeasurementTypeListViewModel list = _gymBodyMeasurementTypeAgent.GetGymBodyMeasurementTypeList(dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Gym/GymBodyMeasurementType/_List.cshtml", list);
            }
            return View($"~/Views/Gym/GymBodyMeasurementType/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            return View(createEdit, new GymBodyMeasurementTypeViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(GymBodyMeasurementTypeViewModel gymBodyMeasurementTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                gymBodyMeasurementTypeViewModel = _gymBodyMeasurementTypeAgent.CreateGymBodyMeasurementType(gymBodyMeasurementTypeViewModel);
                if (!gymBodyMeasurementTypeViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(gymBodyMeasurementTypeViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionModeSave, new { gymBodyMeasurementTypeId = gymBodyMeasurementTypeViewModel.GymBodyMeasurementTypeId });
                    }
                    else if (string.Equals(gymBodyMeasurementTypeViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToList);
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(gymBodyMeasurementTypeViewModel.ErrorMessage));
            return View(createEdit, gymBodyMeasurementTypeViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(short gymBodyMeasurementTypeId)
        {
            GymBodyMeasurementTypeViewModel gymBodyMeasurementTypeViewModel = _gymBodyMeasurementTypeAgent.GetGymBodyMeasurementType(gymBodyMeasurementTypeId);
            return ActionView(createEdit, gymBodyMeasurementTypeViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(GymBodyMeasurementTypeViewModel gymBodyMeasurementTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_gymBodyMeasurementTypeAgent.UpdateGymBodyMeasurementType(gymBodyMeasurementTypeViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(gymBodyMeasurementTypeViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToEdit, new { gymBodyMeasurementTypeId = gymBodyMeasurementTypeViewModel.GymBodyMeasurementTypeId });
                }
                else if (string.Equals(gymBodyMeasurementTypeViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToList);
                }
            }
            return View(createEdit, gymBodyMeasurementTypeViewModel);
        }

        public virtual ActionResult Delete(string gymBodyMeasurementTypeIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(gymBodyMeasurementTypeIds))
            {
                status = _gymBodyMeasurementTypeAgent.DeleteGymBodyMeasurementType(gymBodyMeasurementTypeIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<GymBodyMeasurementTypeController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<GymBodyMeasurementTypeController>(x => x.List(null));
        }

        #region Protected

        #endregion
    }
}