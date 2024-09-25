using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Formats.Asn1.AsnWriter;
using EmailSmsMarketing.Domain.Enum;
using EmailSmsMarketing.BusinessLogic.Queries;
using EmailSmsMarketing.Web.Models.ContactLists.List;
using EmailSmsMarketing.Web.Models.ContactLists.Create;
using DevExpress.ClipboardSource.SpreadsheetML;
using EmailSmsMarketing.Web.Models.ContactLists.Delete;
using Newtonsoft.Json;
using EmailSmsMarketing.Web.Models.ContactList.List;
using EmailSmsMarketing.Domain.Entities.ContactLists;

namespace EmailSmsMarketing.Web.Controllers
{
    [Authorize]
    public class ContactListController : BaseController
    {
        ContactListQuery contactListQuery = new ContactListQuery();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            RefreshToken();
            return View("~/Views/ContactList/Index.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> ContactsList()
        {
            try
            {
                var token = GetToken();

                var response = await contactListQuery.GetList(token);

                if (response.ErrorCode == ErrorCode.Expired_token)
                {
                    if (RefreshToken())
                    {
                        return await ContactsList();
                    }
                }
                else if (response.ErrorCode == ErrorCode.Internal_error)
                {
                    return CreateJsonError("Something wrong!", true);
                }
                else if (response.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonKo(response.ErrorMessage, true);
                }
                if (response.ContactsLists == null)
                {
                    List<ContactListsViewModel> list = new List<ContactListsViewModel>();
                    return CreateJsonOkForCollection(list);
                }
                else
                {
                    return CreateJsonOkForCollection(response.ContactsLists);
                }
            }
            catch (Exception ex)
            {
                return CreateJsonException(ex, true);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateContactList()
        {
            RefreshToken();
            return PartialView("~/Views/ContactList/Partial/ContactsList/_CreateList.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContactList(ImportContactListViewModel data)
        {
            try
            {
                RefreshToken();
                if (ModelState.IsValid)
                {
                    return CreateJsonOk("Saved", true);
                }
                return PartialView("~/Views/ContactList/Partial/ContactsList/_CreateList.cshtml", data);
            }
            catch(Exception ex)
            {
                return CreateJsonException(ex, true);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteContactList(int id)
        {
            try
            {
                var token = GetToken();
                var response = await contactListQuery.GetContactListById(token, id);

                if (response.ErrorCode == ErrorCode.Expired_token)
                {
                    if(RefreshToken())
                    {
                        return await DeleteContactList(id);
                    }
                }
                else if (response.ErrorCode == ErrorCode.Internal_error)
                {
                    throw new Exception(response.ErrorMessage);
                }
                else if (response.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonKo(response.ErrorMessage, true);
                }

                DeleteContactListViewModel data = new DeleteContactListViewModel()
                {
                   ID = response.ContactsList.ID,
                   Name = response.ContactsList.Name,
                   Email = response.ContactsList.Email,
                   Phone = response.ContactsList.Phone,
                   CreateDate = response.ContactsList.CreateDate,
                };

                return PartialView("~/Views/ContactList/Partial/ContactsList/_DeleteList.cshtml", data);
            }
            catch(Exception ex)
            {
                return CreateJsonException(ex, true);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContactList(DeleteContactListViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = GetToken();
                    var response = await contactListQuery.DeleteContactListById(token, data.ID);
                    if (response.ErrorCode == ErrorCode.Expired_token)
                    {
                        if (RefreshToken())
                        {
                            return await DeleteContactList(data);
                        }
                    }
                    else if (response.ErrorCode == ErrorCode.Internal_error)
                    {
                        return CreateJsonNotValid("Something wrong!", true);
                    }
                    else if (response.ErrorCode == ErrorCode.NoError)
                    {
                        return CreateJsonOk("Deleted", true);
                    }
                }
                return PartialView("~/Views/ContactList/Partial/ContactsList/_DeleteList.cshtml", data);
            }
            catch (Exception ex)
            {
                return CreateJsonException(ex, true);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangeTitleContactList(int id)
        {
            try
            {
                var token = GetToken();
                var response = await contactListQuery.GetContactListById(token, id);
                if (response.ErrorCode == ErrorCode.Expired_token)
                {
                    if (RefreshToken())
                    {
                        return await ChangeTitleContactList(id);
                    }
                }
                else if (response.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonKo(response.ErrorMessage, true);
                }

                ContactListsViewModel data = new ContactListsViewModel()
                {
                    ID = response.ContactsList.ID,
                    Name = response.ContactsList.Name,
                    Email = response.ContactsList.Email,
                    Phone = response.ContactsList.Phone,
                    CreateDate = response.ContactsList.CreateDate,
                    Description = response.ContactsList.Description,
                    ContactsData = response.ContactsList.ContactsData,
                };

                return PartialView("~/Views/ContactList/Partial/ContactList/_ChangeTitle.cshtml", data);
            }
            catch
            {
                return View("~/Views/ContactList/List.cshtml");
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> ChangeTitleContactList(ContactListsViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = GetToken();
                    var contactData = new ContactListsData()
                    {
                        ID = data.ID,
                        Name = data.Name,
                        Email = data.Email,
                        Phone = data.Phone,
                        CreateDate = data.CreateDate,
                        Description = data.Description,
                        ContactsData = data.ContactsData,
                        Token = token,

                    };
                    var response = await contactListQuery.UpdateContactList(contactData);

                    if (response.ErrorCode == ErrorCode.Expired_token)
                    {
                        if (RefreshToken())
                        {
                            return await ChangeTitleContactList(data);
                        }
                    }
                    else if (response.ErrorCode != ErrorCode.NoError)
                    {
                        return CreateJsonKo(response.ErrorMessage, true);
                    }

                    return CreateJsonOk("Saved", true);
                }

                return PartialView("~/Views/ContactList/Partial/ContactList/_ChangeTitle.cshtml", data);
            }
            catch(Exception ex)
            {
                return CreateJsonException(ex, true);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> List(int id)
        {
            try
            {
                var token = GetToken();
                var response = await contactListQuery.GetContactListById(token, id);
                if (response.ErrorCode == ErrorCode.Expired_token)
                {
                    if (RefreshToken())
                    {
                        return await List(id);
                    }
                }
                else if (response.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonKo(response.ErrorMessage, true);
                }

                ContactListsViewModel data = new ContactListsViewModel()
                {
                    ID = response.ContactsList.ID,
                    Name = response.ContactsList.Name,
                    Email = response.ContactsList.Email,
                    Phone = response.ContactsList.Phone,
                    CreateDate = response.ContactsList.CreateDate,
                };

                return View("~/Views/ContactList/List.cshtml", data);
            }
            catch
            {
                return View("~/Views/ContactList/List.cshtml");
            }    
        }

        [HttpGet]
        public async Task<IActionResult> ContactList(int id)
        {
            try
            {
                var token = GetToken();
                var response = await contactListQuery.GetContactListById(token, id);
                if (response.ErrorCode == ErrorCode.Expired_token)
                {
                    if (RefreshToken())
                        return await ContactList(id);
                }
                else if (response.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonError("Something wrong!", true);
                }

                List<ContactListViewModel> contactsDataViews = new List<ContactListViewModel>();

                if (response.ContactsList != null)
                {
                    if (!string.IsNullOrEmpty(response.ContactsList.ContactsData))
                    {
                        var contactList = Base64Decode(response.ContactsList.ContactsData);
                        contactsDataViews = JsonConvert.DeserializeObject<List<ContactListViewModel>>(contactList);
                    }
                }
                return CreateJsonOkForSingle(contactsDataViews);
            }
            catch (Exception ex)
            {
                return CreateJsonException(ex, true);
            }
        }

    }
}

