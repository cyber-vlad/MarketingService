namespace EmailSmsMarketing.Web.Models.ContactList.List
{
    public class ContactListViewModel
    {
        public int ID { get; set; }
        public int ContactListId { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
