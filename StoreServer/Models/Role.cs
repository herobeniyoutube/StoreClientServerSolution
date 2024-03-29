namespace StoreServer.Models
{
    /// <summary>
    /// Model for user Roles
    /// </summary>
    public class Role
    {
        public Role(string name) 
        {
            Name = name;
        }
        public string Name { get; set; }    
    }
}
