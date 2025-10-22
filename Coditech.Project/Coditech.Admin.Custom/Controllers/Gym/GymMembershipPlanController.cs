using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;

using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class GymMembershipPlanController : BaseController
    {
        private readonly IGymMembershipPlanAgent _gymMembershipPlanAgent;
        private const string createEdit = "~/Views/Gym/GymMembershipPlan/CreateEdit.cshtml";

        public GymMembershipPlanController(IGymMembershipPlanAgent gymMembershipPlanAgent)
        {
            _gymMembershipPlanAgent = gymMembershipPlanAgent;
        }

        #region GymMembershipPlan
        public ActionResult List(DataTableViewModel dataTableModel)
        {
            GymMembershipPlanListViewModel list = new GymMembershipPlanListViewModel();
            GetListOnlyIfSingleCentre(dataTableModel);
            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode))
            {
                list = _gymMembershipPlanAgent.GetGymMembershipPlanList(dataTableModel);
            }
            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;

            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Gym/GymMembershipPlan/_List.cshtml", list);
            }

            return View("~/Views/Gym/GymMembershipPlan/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult CreateGymMembershipPlan()
        {
            GymMembershipPlanViewModel viewModel = new GymMembershipPlanViewModel();
            viewModel.AllGeneralServices = _gymMembershipPlanAgent.AllGeneralServices();
            return View(createEdit, viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult CreateGymMembershipPlan(GymMembershipPlanViewModel gymMembershipPlanViewModel)
        {
            if (ModelState.IsValid)
            {
                gymMembershipPlanViewModel = _gymMembershipPlanAgent.CreateGymMembershipPlan(gymMembershipPlanViewModel);
                if (!gymMembershipPlanViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(gymMembershipPlanViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("UpdateGymMembershipPlan", new { gymMembershipPlanId = gymMembershipPlanViewModel.GymMembershipPlanId });
                    }
                    else if (string.Equals(gymMembershipPlanViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToList, new DataTableViewModel { SelectedCentreCode = gymMembershipPlanViewModel.CentreCode });
                    }
                }
            }
			gymMembershipPlanViewModel.AllGeneralServices = _gymMembershipPlanAgent.AllGeneralServices();
			SetNotificationMessage(GetErrorNotificationMessage(gymMembershipPlanViewModel.ErrorMessage));
            return View(createEdit, gymMembershipPlanViewModel);
        }

        [HttpGet]
        public virtual ActionResult UpdateGymMembershipPlan(int gymMembershipPlanId)
        {
            GymMembershipPlanViewModel gymMembershipPlanViewModel = _gymMembershipPlanAgent.GetGymMembershipPlan(gymMembershipPlanId);
			gymMembershipPlanViewModel.AllGeneralServices = _gymMembershipPlanAgent.AllGeneralServices();
			return ActionView(createEdit, gymMembershipPlanViewModel);
        }

        [HttpPost]
        public virtual ActionResult UpdateGymMembershipPlan(GymMembershipPlanViewModel gymMembershipPlanViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_gymMembershipPlanAgent.UpdateGymMembershipPlan(gymMembershipPlanViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(gymMembershipPlanViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("UpdateGymMembershipPlan", new { gymMembershipPlanId = gymMembershipPlanViewModel.GymMembershipPlanId });
                }
                else if (string.Equals(gymMembershipPlanViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToList, new DataTableViewModel { SelectedCentreCode = gymMembershipPlanViewModel.CentreCode });
                }
            }
			gymMembershipPlanViewModel.AllGeneralServices = _gymMembershipPlanAgent.AllGeneralServices();
			return View(createEdit, gymMembershipPlanViewModel);
        }

        public virtual ActionResult Delete(string gymMembershipPlanIds, string selectedCentreCode)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(gymMembershipPlanIds))
            {
                status = _gymMembershipPlanAgent.DeleteGymMembershipPlan(gymMembershipPlanIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                // Redirect to the List action with the selectedCentreCode
                return RedirectToAction("List", new { selectedCentreCode });
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            // Redirect to the List action with the selectedCentreCode
            return RedirectToAction("List", new { selectedCentreCode });
        }
        public virtual ActionResult Cancel(string SelectedCentreCode)
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel() { SelectedCentreCode = SelectedCentreCode };
            return RedirectToAction("List", dataTableViewModel);
        }
        #endregion
    }
}