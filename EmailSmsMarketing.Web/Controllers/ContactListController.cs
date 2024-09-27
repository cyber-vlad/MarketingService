using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmailSmsMarketing.Domain.Enum;
using EmailSmsMarketing.BusinessLogic.Queries;
using EmailSmsMarketing.Web.Models.ContactLists.List;
using EmailSmsMarketing.Web.Models.ContactLists.Create;
using DevExpress.ClipboardSource.SpreadsheetML;
using EmailSmsMarketing.Web.Models.ContactLists.Delete;
using Newtonsoft.Json;
using EmailSmsMarketing.Web.Models.ContactList.List;
using EmailSmsMarketing.Domain.Entities.ContactLists;
using EmailSmsMarketing.Web.Models.ContactList.Delete;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using EmailSmsMarketing.Web.Models.ContactList.Upsert;
using Azure;
using System;
using OfficeOpenXml;
using System.Text;

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
        public async Task<IActionResult> CreateContactList(int idList)
        {
            RefreshToken();
            ImportContactListViewModel importContactListViewModel = new ImportContactListViewModel()
            {
                IdList = idList
            };
            return PartialView("~/Views/ContactList/Partial/ContactsList/_CreateList.cshtml", importContactListViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContactList(ImportContactListViewModel data)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return PartialView("~/Views/ContactList/Partial/ContactsList/_CreateList.cshtml", data);
                }
                var token = GetToken();

                ContactListsData contactListsUpsertViewModel = new ContactListsData()
                {
                    ID = 0,
                    Name = data.Name,
                    Email = 0,
                    Phone = 0,
                    Token = token,
                };

                var responsePost = await contactListQuery.UpdateContactList(contactListsUpsertViewModel);

                if (responsePost.ErrorCode == ErrorCode.Expired_token)
                {
                    if (RefreshToken())
                    {
                        return await CreateContactList(data);
                    }
                }
                else if (responsePost.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonKo(responsePost.ErrorMessage, true);
                }

                return CreateJsonOk("Created successfully!", true);
            }
            catch (Exception ex)
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
                    if (RefreshToken())
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
            catch (Exception ex)
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

                List<ContactDataViewModel> contactsDataViews = new List<ContactDataViewModel>();

                if (response.ContactsList != null)
                {
                    if (!string.IsNullOrEmpty(response.ContactsList.ContactsData))
                    {
                        var contactList = Base64Decode(response.ContactsList.ContactsData);
                        contactsDataViews = JsonConvert.DeserializeObject<List<ContactDataViewModel>>(contactList);
                    }
                }
                return CreateJsonOkForSingle(contactsDataViews);
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

                    return CreateJsonOkForSingle(data, "Saved", true);

                }

                return PartialView("~/Views/ContactList/Partial/ContactList/_ChangeTitle.cshtml", data);
            }
            catch (Exception ex)
            {
                return CreateJsonException(ex, true);
            }

        }

        [HttpGet]
        public async Task<IActionResult> UpsertContactData(int id, int parentId)
        {
            try
            {
                UpsertContactDataViewModel upsertContactDataViewModel = new UpsertContactDataViewModel();
                if (id == 0)
                {
                    upsertContactDataViewModel.ContactListId = parentId;
                    upsertContactDataViewModel.ID = id;

                    return PartialView("~/Views/ContactList/Partial/ContactList/_UpsertContact.cshtml", upsertContactDataViewModel);
                }

                var token = GetToken();
                List<ContactDataViewModel> contactsDataViews = new List<ContactDataViewModel>();
                var response = await contactListQuery.GetContactListById(token, parentId);

                if (response.ErrorCode == ErrorCode.Expired_token)
                {
                    if (RefreshToken())
                    {
                        return await UpsertContactData(id, parentId);
                    }
                }
                else if (response.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonKo(response.ErrorMessage, true);
                }

                if (response.ContactsList != null)
                {
                    if (!string.IsNullOrEmpty(response.ContactsList.ContactsData))
                    {
                        var contactList = Base64Decode(response.ContactsList.ContactsData);
                        contactsDataViews = JsonConvert.DeserializeObject<List<ContactDataViewModel>>(contactList);
                    }
                }

                var contact = contactsDataViews.FirstOrDefault(x => x.ID == id);
                contact.ContactListId = parentId;

                upsertContactDataViewModel.ID = contact.ID;
                upsertContactDataViewModel.ContactListId = parentId;
                upsertContactDataViewModel.Name = contact.Name;
                upsertContactDataViewModel.Email = contact.Email;
                upsertContactDataViewModel.Phone = contact.Phone;

                return PartialView("~/Views/ContactList/Partial/ContactList/_UpsertContact.cshtml", upsertContactDataViewModel);
            }
            catch (Exception ex)
            {
                return CreateJsonException(ex, true);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpsertContactData(UpsertContactDataViewModel upsertContactDataViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("~/Views/ContactList/Partial/ContactList/_UpsertContact.cshtml", upsertContactDataViewModel);
                }
                var token = GetToken();

                List<ContactDataViewModel> contactsDataViews = new List<ContactDataViewModel>();
                var response = await contactListQuery.GetContactListById(token, upsertContactDataViewModel.ContactListId);

                if (response.ErrorCode == ErrorCode.Expired_token)
                {
                    if (RefreshToken())
                    {
                        return await UpsertContactData(upsertContactDataViewModel);
                    }
                }
                else if (response.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonKo(response.ErrorMessage, true);
                }

                if (response.ContactsList != null)
                {
                    if (!string.IsNullOrEmpty(response.ContactsList.ContactsData))
                    {
                        var contactList = Base64Decode(response.ContactsList.ContactsData);
                        contactsDataViews = JsonConvert.DeserializeObject<List<ContactDataViewModel>>(contactList);
                        contactsDataViews = contactsDataViews.OrderBy(x => x.ID).ToList();
                    }
                }

                List<ContactDataCreate> contactListUpsertViewModels = new List<ContactDataCreate>();

                if (contactsDataViews.Count() > 0)
                {
                    foreach (var item in contactsDataViews)
                    {
                        ContactDataCreate contactListUpsertViewModel = new ContactDataCreate();

                        if (item.ID == upsertContactDataViewModel.ID)
                        {
                            contactListUpsertViewModel.ID = upsertContactDataViewModel.ID;
                            contactListUpsertViewModel.Name = upsertContactDataViewModel.Name;
                            contactListUpsertViewModel.Email = upsertContactDataViewModel.Email;
                            contactListUpsertViewModel.Phone = upsertContactDataViewModel.Phone;
                        }
                        else
                        {
                            contactListUpsertViewModel.ID = item.ID;
                            contactListUpsertViewModel.Name = item.Name;
                            contactListUpsertViewModel.Email = item.Email;
                            contactListUpsertViewModel.Phone = item.Phone;
                        }

                        contactListUpsertViewModels.Add(contactListUpsertViewModel);
                    }
                    if (upsertContactDataViewModel.ID == 0)
                    {
                        ContactDataCreate contactListUpsertViewModelNew = new ContactDataCreate()
                        {
                            ID = contactsDataViews.LastOrDefault().ID + 1,
                            Name = upsertContactDataViewModel.Name,
                            Email = upsertContactDataViewModel.Email,
                            Phone = upsertContactDataViewModel.Phone,
                        };

                        contactListUpsertViewModels.Add(contactListUpsertViewModelNew);
                    }
                }
                else
                {
                    ContactDataCreate contactListUpsertViewModel = new ContactDataCreate()
                    {
                        ID = 1,
                        Name = upsertContactDataViewModel.Name,
                        Email = upsertContactDataViewModel.Email,
                        Phone = upsertContactDataViewModel.Phone,
                    };

                    contactListUpsertViewModels.Add(contactListUpsertViewModel);
                }

                ContactListsData contactListsUpsertViewModel = new ContactListsData()
                {
                    ID = response.ContactsList.ID,
                    CreateDate = response.ContactsList.CreateDate,
                    Description = response.ContactsList.Description,
                    Name = response.ContactsList.Name,
                    ContactsData = Base64Encode(JsonConvert.SerializeObject(contactListUpsertViewModels)),
                    Email = contactListUpsertViewModels.Where(x => x.Email != null).Count(),
                    Phone = contactListUpsertViewModels.Where(x => x.Phone != null).Count(),
                    Token = token,
                };

                var responsePost = await contactListQuery.UpdateContactList(contactListsUpsertViewModel);

                if (responsePost.ErrorCode == ErrorCode.Expired_token)
                {
                    if (RefreshToken())
                    {
                        return await UpsertContactData(upsertContactDataViewModel);
                    }
                }
                else if (responsePost.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonKo(responsePost.ErrorMessage, true);
                }

                return CreateJsonOk("Saved", true);
            }
            catch (Exception ex)
            {
                return CreateJsonException(ex, true);
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteContactData(int id, int parentId)
        {
            try
            {
                var token = GetToken();
                var response = await contactListQuery.GetContactListById(token, parentId);

                if (response.ErrorCode == ErrorCode.Expired_token)
                {
                    if (RefreshToken())
                    {
                        return await DeleteContactData(id, parentId);
                    }
                }
                else if (response.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonKo(response.ErrorMessage, true);
                }

                List<ContactDataViewModel> contactsDataViews = new List<ContactDataViewModel>();
                if (response.ContactsList != null)
                {
                    if (!string.IsNullOrEmpty(response.ContactsList.ContactsData))
                    {
                        var contactList = Base64Decode(response.ContactsList.ContactsData);
                        contactsDataViews = JsonConvert.DeserializeObject<List<ContactDataViewModel>>(contactList);
                    }
                }

                var contact = contactsDataViews.FirstOrDefault(x => x.ID == id);
                contact.ContactListId = parentId;
                DeleteContactDataViewModel contactsDataViewModel = new DeleteContactDataViewModel();
                contactsDataViewModel.ID = contact.ID;
                contactsDataViewModel.ContactListId = parentId;
                contactsDataViewModel.Name = contact.Name;
                contactsDataViewModel.Email = contact.Email;
                contactsDataViewModel.Phone = contact.Phone;
                return PartialView("~/Views/ContactList/Partial/ContactList/_DeleteContact.cshtml", contactsDataViewModel);
            }
            catch (Exception ex)
            {
                return CreateJsonException(ex, true);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteContactData(DeleteContactDataViewModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("~/Views/ContactList/Partial/ContactList/_DeleteContact.cshtml", data);
                }
                var token = GetToken();
                var response = await contactListQuery.GetContactListById(token, data.ContactListId);

                if (response.ErrorCode == ErrorCode.Expired_token)
                {
                    if (RefreshToken())
                    {
                        return await DeleteContactData(data);
                    }
                }
                else if (response.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonKo(response.ErrorMessage, true);
                }
                List<ContactDataViewModel> contactsDataViews = new List<ContactDataViewModel>();

                if (response.ContactsList != null)
                {
                    if (!string.IsNullOrEmpty(response.ContactsList.ContactsData))
                    {
                        var contactList = Base64Decode(response.ContactsList.ContactsData);
                        contactsDataViews = JsonConvert.DeserializeObject<List<ContactDataViewModel>>(contactList);
                        contactsDataViews = contactsDataViews.OrderBy(x => x.ID).ToList();
                    }
                }

                var contact = contactsDataViews.FirstOrDefault(x => x.ID == data.ID);
                contactsDataViews.Remove(contact);

                List<ContactDataCreate> contactsData = new List<ContactDataCreate>();
                foreach (var item in contactsDataViews)
                {
                    ContactDataCreate contactData = new ContactDataCreate();

                    contactData.ID = item.ID;
                    contactData.Name = item.Name;
                    contactData.Email = item.Email;
                    contactData.Phone = item.Phone;

                    contactsData.Add(contactData);
                }
                ContactListsData contactListsData = new ContactListsData()
                {
                    ID = response.ContactsList.ID,
                    CreateDate = response.ContactsList.CreateDate,
                    Description = response.ContactsList.Description,
                    Name = response.ContactsList.Name,
                    ContactsData = Base64Encode(JsonConvert.SerializeObject(contactsData)),
                    Email = contactsData.Where(x => x.Email != null).Count(),
                    Phone = contactsData.Where(x => x.Phone != null).Count(),
                    Token = token,
                };

                var responsePost = await contactListQuery.UpdateContactList(contactListsData);

                if (responsePost.ErrorCode == ErrorCode.Expired_token)
                {
                    if (RefreshToken())
                    {
                        return await DeleteContactData(data);
                    }
                }

                else if (responsePost.ErrorCode != ErrorCode.NoError)
                {
                    return CreateJsonKo(responsePost.ErrorMessage, true);
                }

                return CreateJsonOk("Deleted", true);
            }
            catch (Exception ex)
            {
                return CreateJsonException(ex, true);
            }
        }

    }
}

