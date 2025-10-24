using Coditech.Admin.Agents;
using Coditech.Admin.Helpers;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
namespace Coditech.Admin.Controllers
{
    public class GymMemberDetailsController : BaseController
    {
        private readonly IGymMemberDetailsAgent _gymMemberDetailsAgent;
        private readonly IGymMemberBodyMeasurementAgent _gymMemberBodyMeasurementAgent;
        private readonly IUserAgent _userAgent;
        private const string createEditGymMember = "~/Views/Gym/GymMemberDetails/CreateEditGymMember.cshtml";

        public GymMemberDetailsController(IGymMemberDetailsAgent gymMemberDetailsAgent, IGymMemberBodyMeasurementAgent gymMemberBodyMeasurementAgent, IUserAgent userAgent)
        {
            _gymMemberDetailsAgent = gymMemberDetailsAgent;
            _gymMemberBodyMeasurementAgent = gymMemberBodyMeasurementAgent;
            _userAgent = userAgent;
        }

        #region GymMemberDetails
        public ActionResult List(DataTableViewModel dataTableViewModel)
        {
            GymMemberDetailsListViewModel list = new GymMemberDetailsListViewModel();
            GetListOnlyIfSingleCentre(dataTableViewModel);
            if (!string.IsNullOrEmpty(dataTableViewModel.SelectedCentreCode))
            {
                list = _gymMemberDetailsAgent.GetGymMemberDetailsList(dataTableViewModel);
            }
            list.SelectedCentreCode = dataTableViewModel.SelectedCentreCode;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Gym/GymMemberDetails/_List.cshtml", list);
            }
            return View($"~/Views/Gym/GymMemberDetails/List.cshtml", list);
        }

        public ActionResult ActiveMemberList(DataTableViewModel dataTableViewModel)
        {
            GymMemberDetailsListViewModel list = new GymMemberDetailsListViewModel();
            GetListOnlyIfSingleCentre(dataTableViewModel);
            if (!string.IsNullOrEmpty(dataTableViewModel.SelectedCentreCode))
            {
                list = _gymMemberDetailsAgent.GetGymMemberDetailsList(dataTableViewModel, "Active");
            }
            list.SelectedCentreCode = dataTableViewModel.SelectedCentreCode;
            list.ListType = "Active";
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Gym/GymMemberDetails/_List.cshtml", list);
            }
            return View($"~/Views/Gym/GymMemberDetails/List.cshtml", list);
        }

        public ActionResult InActiveMemberList(DataTableViewModel dataTableViewModel)
        {
            GymMemberDetailsListViewModel list = new GymMemberDetailsListViewModel();
            GetListOnlyIfSingleCentre(dataTableViewModel);
            if (!string.IsNullOrEmpty(dataTableViewModel.SelectedCentreCode))
            {
                list = _gymMemberDetailsAgent.GetGymMemberDetailsList(dataTableViewModel, "InActive");
            }
            list.SelectedCentreCode = dataTableViewModel.SelectedCentreCode;
            list.ListType = "InActive";
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Gym/GymMemberDetails/_List.cshtml", list);
            }
            return View($"~/Views/Gym/GymMemberDetails/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult CreateMember()
        {
            GymCreateEditMemberViewModel viewModel = new GymCreateEditMemberViewModel();
            return View(createEditGymMember, viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult CreateMember(GymCreateEditMemberViewModel gymCreateEditMemberViewModel)
        {
            if (ModelState.IsValid)
            {
                gymCreateEditMemberViewModel = _gymMemberDetailsAgent.CreateMemberDetails(gymCreateEditMemberViewModel);
                if (!gymCreateEditMemberViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(gymCreateEditMemberViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("UpdateMemberPersonalDetails", new { gymMemberDetailId = gymCreateEditMemberViewModel.EntityId, personId = gymCreateEditMemberViewModel.PersonId });
                    }
                    else if (string.Equals(gymCreateEditMemberViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToList, new DataTableViewModel { SelectedCentreCode = gymCreateEditMemberViewModel.SelectedCentreCode });
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(gymCreateEditMemberViewModel.ErrorMessage));
            return View(createEditGymMember, gymCreateEditMemberViewModel);
        }

        [HttpGet]
        public virtual ActionResult UpdateMemberPersonalDetails(int gymMemberDetailId, long personId)
        {
            GymCreateEditMemberViewModel gymCreateEditMemberViewModel = _gymMemberDetailsAgent.GetMemberPersonalDetails(gymMemberDetailId, personId);
            return ActionView(createEditGymMember, gymCreateEditMemberViewModel);
        }

        [HttpPost]
        public virtual ActionResult UpdateMemberPersonalDetails(GymCreateEditMemberViewModel gymCreateEditMemberViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_gymMemberDetailsAgent.UpdateMemberPersonalDetails(gymCreateEditMemberViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(gymCreateEditMemberViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("UpdateMemberPersonalDetails", new { gymMemberDetailId = gymCreateEditMemberViewModel.GymMemberDetailId, personId = gymCreateEditMemberViewModel.PersonId });
                }
                else if (string.Equals(gymCreateEditMemberViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToList, new DataTableViewModel { SelectedCentreCode = gymCreateEditMemberViewModel.SelectedCentreCode });
                }
            }
            return View(createEditGymMember, gymCreateEditMemberViewModel);
        }

        [HttpGet]
        public virtual ActionResult MemberOtherDetails(int gymMemberDetailId)
        {
            GymMemberDetailsViewModel gymMemberDetailsViewModel = _gymMemberDetailsAgent.GetGymMemberOtherDetails(gymMemberDetailId);
            return View("~/Views/Gym/GymMemberDetails/UpdateGymMemberOtherDetails.cshtml", gymMemberDetailsViewModel);
        }

        [HttpPost]
        public virtual ActionResult MemberOtherDetails(GymMemberDetailsViewModel gymMemberDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_gymMemberDetailsAgent.UpdateGymMemberOtherDetails(gymMemberDetailsViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(gymMemberDetailsViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("MemberOtherDetails", new { gymMemberDetailId = gymMemberDetailsViewModel.GymMemberDetailId });
                }
                else if (string.Equals(gymMemberDetailsViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToList, new DataTableViewModel { SelectedCentreCode = gymMemberDetailsViewModel.CentreCode });
                }
            }
            return View("~/Views/Gym/GymMemberDetails/UpdateGymMemberOtherDetails.cshtml", gymMemberDetailsViewModel);
        }
        public virtual ActionResult Delete(string gymMemberDetailIds, string selectedCentreCode)
        {
            string message = string.Empty;
            bool status = false;

            if (!string.IsNullOrEmpty(gymMemberDetailIds))
            {
                status = _gymMemberDetailsAgent.DeleteGymMembers(gymMemberDetailIds, out message);

                SetNotificationMessage(!status
                    ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                    : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = selectedCentreCode });
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = selectedCentreCode });
        }
        #endregion

        #region GymMember Address
        [HttpGet]
        public virtual ActionResult CreateEditGymMemberAddress(int gymMemberDetailId, long personId)
        {
            GeneralPersonAddressListViewModel model = new GeneralPersonAddressListViewModel()
            {
                EntityId = gymMemberDetailId,
                PersonId = personId,
                EntityType = UserTypeCustomEnum.GymMember.ToString()
            };
            return ActionView("~/Views/Gym/GymMemberDetails/CreateEditGymMemberAddress.cshtml", model);
        }

        [HttpPost]
        public virtual ActionResult CreateEditGeneralPersonalAddress(GeneralPersonAddressViewModel generalPersonAddressViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_userAgent.InsertUpdateGeneralPersonAddress(generalPersonAddressViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
            }
            return RedirectToAction("CreateEditGymMemberAddress", "GymMemberDetails", new { gymMemberDetailId = generalPersonAddressViewModel.EntityId, personId = generalPersonAddressViewModel.PersonId });
        }
        #endregion 

        #region MemberFollowUpList
        public ActionResult MemberFollowUpList(DataTableViewModel dataTableModel)
        {
            GymMemberFollowUpListViewModel list = _gymMemberDetailsAgent.GymMemberFollowUpList(Convert.ToInt32(dataTableModel.SelectedParameter1), Convert.ToInt64(dataTableModel.SelectedParameter2), dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Gym/GymMemberDetails/_GymMemberFollowUpList.cshtml", list);
            }
            list.SelectedParameter1 = dataTableModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableModel.SelectedParameter2;

            return View($"~/Views/Gym/GymMemberDetails/GymMemberFollowUpList.cshtml", list);
        }
        [HttpGet]
        public virtual ActionResult GetMemberFollowUp(int gymMemberDetailId, long gymMemberFollowUpId, long personId)
        {
            GymMemberFollowUpViewModel gymMemberDetailsViewModel = null;
            if (gymMemberFollowUpId > 0)
            {
                gymMemberDetailsViewModel = _gymMemberDetailsAgent.GetMemberFollowUp(gymMemberFollowUpId);
                gymMemberDetailsViewModel.GymMemberDetailId = gymMemberDetailId;
                gymMemberDetailsViewModel.PersonId = personId;
            }
            else
            {
                gymMemberDetailsViewModel = new GymMemberFollowUpViewModel()
                {
                    GymMemberDetailId = gymMemberDetailId,
                    GymMemberFollowUpId = gymMemberFollowUpId
                };
            }
            return PartialView($"~/Views/Gym/GymMemberDetails/_CreateEditMemberFollowUp.cshtml", gymMemberDetailsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult CreatEditMemberFollowUp(GymMemberFollowUpViewModel gymMemberFollowUpViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_gymMemberDetailsAgent.InserUpdateGymMemberFollowUp(gymMemberFollowUpViewModel).HasError
                ? gymMemberFollowUpViewModel.GymMemberFollowUpId > 0 ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage) : GetErrorNotificationMessage(GeneralResources.ErrorFailedToCreate)
                : gymMemberFollowUpViewModel.GymMemberFollowUpId > 0 ? GetSuccessNotificationMessage(GeneralResources.UpdateMessage) : GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
            }
            return RedirectToAction("MemberFollowUpList", new { SelectedParameter1 = gymMemberFollowUpViewModel.GymMemberDetailId, SelectedParameter2 = gymMemberFollowUpViewModel.PersonId });
        }

        public virtual ActionResult DeleteGymMemberFollowUp(string gymMemberFollowUpIdIds, int gymMemberDetailId, long personId)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(gymMemberFollowUpIdIds))
            {
                status = _gymMemberDetailsAgent.DeleteGymMemberFollowUp(gymMemberFollowUpIdIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("MemberFollowUpList", new { SelectedParameter1 = gymMemberDetailId, SelectedParameter2 = personId });
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("MemberFollowUpList", new { SelectedParameter1 = gymMemberDetailId, SelectedParameter2 = personId });
        }
        #endregion

        #region Gym Member Attendance
        public ActionResult MemberAttendanceDetails(DataTableViewModel dataTableModel)
        {
            GeneralPersonAttendanceDetailsListViewModel list = _gymMemberDetailsAgent.GeneralPersonAttendanceDetailsList(Convert.ToInt32(dataTableModel.SelectedParameter1), Convert.ToInt64(dataTableModel.SelectedParameter2), UserTypeCustomEnum.GymMember.ToString(), dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Shared/GeneralPerson/_GeneralPersonAttendanceDetailsList.cshtml", list);
            }
            list.SelectedParameter1 = dataTableModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableModel.SelectedParameter2;
            return View($"~/Views/Gym/GymMemberDetails/GeneralPersonAttendanceDetails/GeneralPersonAttendanceDetailsList.cshtml", list);
        }
        [HttpGet]
        public virtual ActionResult GetGeneralPersonAttendanceDetails(int gymMemberDetailId, long generalPersonAttendanceDetailId)
        {
            GeneralPersonAttendanceDetailsViewModel gymMemberDetailsViewModel = null;
            if (generalPersonAttendanceDetailId > 0)
            {
                gymMemberDetailsViewModel = _gymMemberDetailsAgent.GetGeneralPersonAttendanceDetails(generalPersonAttendanceDetailId);
                gymMemberDetailsViewModel.EntityId = gymMemberDetailId;
            }
            else
            {
                gymMemberDetailsViewModel = new GeneralPersonAttendanceDetailsViewModel()
                {
                    EntityId = gymMemberDetailId,
                    GeneralPersonAttendanceDetailId = generalPersonAttendanceDetailId
                };
            }

            GymMemberDetailsViewModel gymMemberDetailModel = _gymMemberDetailsAgent.GetGymMemberOtherDetails(gymMemberDetailId);

            // Now, set the Firstname property in the ViewModel
            if (gymMemberDetailModel != null)
            {
                gymMemberDetailsViewModel.FirstName = gymMemberDetailModel.FirstName;
                gymMemberDetailsViewModel.LastName = gymMemberDetailModel.LastName;
            }

            return PartialView($"~/Views/Gym/GymMemberDetails/GeneralPersonAttendanceDetails/_CreateEditGeneralPersonAttendanceDetails.cshtml", gymMemberDetailsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult CreatEditGeneralPersonAttendanceDetails(GeneralPersonAttendanceDetailsViewModel generalPersonAttendanceDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_gymMemberDetailsAgent.InserUpdateGeneralPersonAttendanceDetails(generalPersonAttendanceDetailsViewModel).HasError
                ? generalPersonAttendanceDetailsViewModel.GeneralPersonAttendanceDetailId > 0 ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage) : GetErrorNotificationMessage(GeneralResources.ErrorFailedToCreate)
                : generalPersonAttendanceDetailsViewModel.GeneralPersonAttendanceDetailId > 0 ? GetSuccessNotificationMessage(GeneralResources.UpdateMessage) : GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
            }
            return RedirectToAction("MemberAttendanceDetails", new { SelectedParameter1 = generalPersonAttendanceDetailsViewModel.EntityId, SelectedParameter2 = generalPersonAttendanceDetailsViewModel.PersonId });
        }

        public virtual ActionResult DeleteGeneralPersonAttendanceDetails(string generalPersonAttendanceDetailIdIds, int gymMemberDetailId, long personId)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(generalPersonAttendanceDetailIdIds))
            {
                status = _gymMemberDetailsAgent.DeleteGeneralPersonAttendanceDetails(generalPersonAttendanceDetailIdIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("GeneralPersonAttendanceDetailsList", new { gymMemberDetailId = gymMemberDetailId, personId = personId });
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("GeneralPersonAttendanceDetailsList", new { gymMemberDetailId = gymMemberDetailId, personId = personId });
        }
        #endregion

        #region GymMemberBodyMeasurement
        [HttpGet]
        public virtual ActionResult GetBodyMeasurementTypeListByMemberId(int gymMemberDetailId, long personId)
        {
            GymMemberBodyMeasurementListViewModel list = _gymMemberBodyMeasurementAgent.GetBodyMeasurementTypeListByMemberId(gymMemberDetailId, personId, 4);
            return ActionView("~/Views/Gym/GymMemberDetails/GymMemberBodyMeasurement/GymMemberBodyMeasurement.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult GetGymMemberBodyMeasurement(int gymMemberDetailId, long gymMemberBodyMeasurementId, short gymBodyMeasurementTypeId, string bodyMeasurementType, string measurementUnitShortCode, long personId)
        {
            GymMemberBodyMeasurementViewModel gymMemberBodyMeasurementViewModel = null;

            if (gymMemberBodyMeasurementId > 0)
            {
                // Retrieve existing gym member body measurement based on both ID and Type
                gymMemberBodyMeasurementViewModel = _gymMemberBodyMeasurementAgent.GetMemberBodyMeasurement(gymMemberBodyMeasurementId, gymBodyMeasurementTypeId);
            }
            else
            {
                gymMemberBodyMeasurementViewModel = new GymMemberBodyMeasurementViewModel()
                {
                    GymBodyMeasurementTypeId = gymBodyMeasurementTypeId, // Set the GymBodyMeasurementTypeId for new measurement
                    BodyMeasurementDate = Convert.ToDateTime(DateTime.Now.ToCoditechDateFormat())
                };
            }
            gymMemberBodyMeasurementViewModel.GymMemberDetailId = gymMemberDetailId;
            gymMemberBodyMeasurementViewModel.PersonId = personId;
            gymMemberBodyMeasurementViewModel.BodyMeasurementType = bodyMeasurementType;
            gymMemberBodyMeasurementViewModel.MeasurementUnitShortCode = measurementUnitShortCode;
            return PartialView("~/Views/Gym/GymMemberDetails/GymMemberBodyMeasurement/_GymMemberBodyMeasurementPopUp.cshtml", gymMemberBodyMeasurementViewModel);
        }

        [HttpPost]
        public virtual ActionResult AddGymMemberBodyMeasurement(GymMemberBodyMeasurementViewModel gymMemberBodyMeasurementViewModel)
        {
            if (ModelState.IsValid)
            {
                if (gymMemberBodyMeasurementViewModel.GymMemberBodyMeasurementId > 0)
                    gymMemberBodyMeasurementViewModel = _gymMemberBodyMeasurementAgent.UpdateMemberBodyMeasurement(gymMemberBodyMeasurementViewModel);
                else
                    gymMemberBodyMeasurementViewModel = _gymMemberBodyMeasurementAgent.CreateMemberBodyMeasurement(gymMemberBodyMeasurementViewModel);
                if (!gymMemberBodyMeasurementViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(gymMemberBodyMeasurementViewModel.GymMemberBodyMeasurementId > 0 ? GeneralResources.UpdateMessage : GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("GetBodyMeasurementTypeListByMemberId", new { gymMemberDetailId = gymMemberBodyMeasurementViewModel.GymMemberDetailId, personId = gymMemberBodyMeasurementViewModel.PersonId });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(gymMemberBodyMeasurementViewModel.ErrorMessage));
            return PartialView("~/Views/Gym/GymMemberDetails/_GymMemberBodyMeasurementPopUp.cshtml", gymMemberBodyMeasurementViewModel);
        }
        #endregion

        #region GymMemberMembershipPlan

        public ActionResult GetGymMemberMembershipPlan(DataTableViewModel dataTableModel)
        {
            GymMemberMembershipPlanListViewModel list = _gymMemberDetailsAgent.GetGymMemberMembershipPlan(Convert.ToInt32(dataTableModel.SelectedParameter1), Convert.ToInt64(dataTableModel.SelectedParameter2), dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Gym/GymMemberDetails/GymMemberMembershipPlan/_GymMemberMembershipPlanList.cshtml", list);
            }
            list.SelectedParameter1 = dataTableModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableModel.SelectedParameter2;
            return View($"~/Views/Gym/GymMemberDetails/GymMemberMembershipPlan/GymMemberMembershipPlanList.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult AssociateGymMemberMembershipPlan(int gymMemberDetailId, long personId)
        {
            GymMemberDetailsViewModel gymMemberDetailsViewModel = _gymMemberDetailsAgent.GetGymMemberOtherDetails(gymMemberDetailId);
            GymMemberMembershipPlanViewModel gymMemberMembershipPlanViewModel = new GymMemberMembershipPlanViewModel()
            {
                CentreCode = gymMemberDetailsViewModel.CentreCode,
                FirstName = gymMemberDetailsViewModel.FirstName,
                LastName = gymMemberDetailsViewModel.LastName,
                GymMemberDetailId = gymMemberDetailId,
                PersonId = personId
            };
            return View($"~/Views/Gym/GymMemberDetails/GymMemberMembershipPlan/AssociateGymMemberMembershipPlan.cshtml", gymMemberMembershipPlanViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AssociateGymMemberMembershipPlan(GymMemberMembershipPlanViewModel gymMemberMembershipPlanViewModel)
        {
            if (ModelState.IsValid)
            {
                gymMemberMembershipPlanViewModel = _gymMemberDetailsAgent.AssociateGymMemberMembershipPlan(gymMemberMembershipPlanViewModel);
                if (!gymMemberMembershipPlanViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(gymMemberMembershipPlanViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("AssociateGymMemberMembershipPlan", new { gymMemberDetailId = gymMemberMembershipPlanViewModel.GymMemberDetailId, personId = gymMemberMembershipPlanViewModel.PersonId });
                    }
                    else if (string.Equals(gymMemberMembershipPlanViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("GetGymMemberMembershipPlan", new { SelectedParameter1 = gymMemberMembershipPlanViewModel.GymMemberDetailId, SelectedParameter2 = gymMemberMembershipPlanViewModel.PersonId });
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(gymMemberMembershipPlanViewModel.ErrorMessage));
            return View($"~/Views/Gym/GymMemberDetails/GymMemberMembershipPlan/AssociateGymMemberMembershipPlan.cshtml", gymMemberMembershipPlanViewModel);
        }

        public ActionResult MemberPaymentHistoryList(DataTableViewModel dataTableModel)
        {
            GymMemberSalesInvoiceListViewModel list = _gymMemberDetailsAgent.GymMemberPaymentHistoryList(Convert.ToInt32(dataTableModel.SelectedParameter1), Convert.ToInt64(dataTableModel.SelectedParameter2), dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Gym/GymMemberDetails/GymMemberPaymentHistory/_GymMemberPaymentHistory.cshtml", list);
            }
            list.SelectedParameter1 = dataTableModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableModel.SelectedParameter2;
            return View($"~/Views/Gym/GymMemberDetails/GymMemberPaymentHistory/GymMemberPaymentHistory.cshtml", list);
        }

        public virtual ActionResult Cancel(string SelectedCentreCode)
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel() { SelectedCentreCode = SelectedCentreCode };
            return RedirectToAction("List", dataTableViewModel);
        }
        #endregion
    }
}
