namespace MyServiceGraphQL
{
    public class Department
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Manager { get; set; }
    }

    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string DepartmentId { get; set; }
        public string JobTitle { get; set; }
    }

}
