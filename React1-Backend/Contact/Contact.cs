using System;

namespace React1_Backend.Contact;

public class ContactData
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
}

public class Mail(ContactData contactData)
{
    public string Name { get; set; } = contactData.Name;
    public string Email { get; set; } = contactData.Email;
    public string Message { get; set; } = contactData.Message;
    public DateTime Date { get; set; } = DateTime.UtcNow;
}
