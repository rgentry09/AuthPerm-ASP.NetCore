﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using AuthPermissions.AdminCode;
using AuthPermissions.AspNetCore;
using AuthPermissions.SupportCode.DownStatusCode;
using Example4.MvcWebApp.IndividualAccounts.Models;
using Example4.MvcWebApp.IndividualAccounts.PermissionsCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Example4.MvcWebApp.IndividualAccounts.Controllers
{
    public class TenantController : Controller
    {
        private readonly IAuthTenantAdminService _authTenantAdmin;
        private readonly ISetRemoveStatus _upDownService;

        public TenantController(IAuthTenantAdminService authTenantAdmin, ISetRemoveStatus upDownService)
        {
            _authTenantAdmin = authTenantAdmin;
            _upDownService = upDownService;
        }

        [HasPermission(Example4Permissions.TenantList)]
        public async Task<IActionResult> Index(string message)
        {
            var tenantNames = await HierarchicalTenantDto.TurnIntoDisplayFormat( _authTenantAdmin.QueryTenants())
                .OrderBy(x => x.TenantFullName)
                .ToListAsync();

            ViewBag.Message = message;

            return View(tenantNames);
        }

        [HasPermission(Example4Permissions.TenantCreate)]
        public async Task<IActionResult> Create()
        {
            var model = await HierarchicalTenantDto.SetupForCreateAsync(_authTenantAdmin);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Example4Permissions.TenantCreate)]
        public async Task<IActionResult> Create(HierarchicalTenantDto input)
        {
            var status = await _authTenantAdmin
                .AddHierarchicalTenantAsync(input.TenantName, input.ParentId);

            return status.HasErrors
                ? RedirectToAction(nameof(ErrorDisplay),
                    new { errorMessage = status.GetAllErrors() })
                : RedirectToAction(nameof(Index), new { message = status.Message });
        }

        [HasPermission(Example4Permissions.TenantUpdate)]
        public async Task<IActionResult> Edit(int id)
        {
            var status = await _authTenantAdmin.GetTenantViaIdAsync(id);
            if (status.HasErrors)
                return RedirectToAction(nameof(ErrorDisplay),
                    new { errorMessage = status.GetAllErrors() });

            return View(HierarchicalTenantDto.SetupForEdit(status.Result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Example4Permissions.TenantUpdate)]
        public async Task<IActionResult> Edit(HierarchicalTenantDto input)
        {
            var removeDownAsync = await _upDownService.SetTenantDownWithDelayAsync(TenantDownVersions.Update, input.TenantId);
            var status = await _authTenantAdmin
                .UpdateTenantNameAsync(input.TenantId, input.TenantName);
            await removeDownAsync();

            return status.HasErrors
                ? RedirectToAction(nameof(ErrorDisplay),
                    new { errorMessage = status.GetAllErrors() })
                : RedirectToAction(nameof(Index), new { message = status.Message });
        }


        [HasPermission(Example4Permissions.TenantMove)]
        public async Task<IActionResult> Move(int id)
        {
            var status = await _authTenantAdmin.GetTenantViaIdAsync(id);
            if (status.HasErrors)
                return RedirectToAction(nameof(ErrorDisplay),
                    new { errorMessage = status.GetAllErrors() });

            return View(await HierarchicalTenantDto.SetupForMoveAsync(status.Result, _authTenantAdmin));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Example4Permissions.TenantMove)]
        public async Task<IActionResult> Move(HierarchicalTenantDto input)
        {
            //A hierarchical Move requires both the tenant being moved and the tenant receiving the moved tenant 
            var removeDownAsync = await _upDownService.SetTenantDownWithDelayAsync(TenantDownVersions.Update, input.TenantId, input.ParentId);
            var status = await _authTenantAdmin
                .MoveHierarchicalTenantToAnotherParentAsync(input.TenantId, input.ParentId);
            await removeDownAsync();

            if (status.HasErrors)
                return RedirectToAction(nameof(ErrorDisplay),
                    new { errorMessage = status.GetAllErrors() });

            return status.HasErrors
                ? RedirectToAction(nameof(ErrorDisplay),
                    new { errorMessage = status.GetAllErrors() })
                : RedirectToAction(nameof(Index), new { message = status.Message });
        }

        [HasPermission(Example4Permissions.TenantDelete)]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _authTenantAdmin.GetTenantViaIdAsync(id);
            if (status.HasErrors)
                return RedirectToAction(nameof(ErrorDisplay),
                    new { errorMessage = status.GetAllErrors() });

            return View(await HierarchicalTenantDto.SetupForDeleteAsync(status.Result, _authTenantAdmin));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Example4Permissions.TenantDelete)]
        public async Task<IActionResult> Delete(HierarchicalTenantDto input)
        {
            //This will permanently stop logged-in user from accessing the the delete tenant
            var removeDownAsync = await _upDownService.SetTenantDownWithDelayAsync(TenantDownVersions.Deleted, input.TenantId);
            var status = await _authTenantAdmin.DeleteTenantAsync(input.TenantId);
            if (status.HasErrors)
                await removeDownAsync();

            return status.HasErrors
                ? RedirectToAction(nameof(ErrorDisplay),
                    new { errorMessage = status.GetAllErrors() })
                : RedirectToAction(nameof(Index), new { message = status.Message });
        }

        public ActionResult ErrorDisplay(string errorMessage)
        {
            return View((object)errorMessage);
        }
    }
}
