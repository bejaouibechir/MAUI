using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyServiceGraphQL
{
    public class FirstQuery :ObjectType
    {
        private static List<Employee> _employees = new List<Employee>
        {
            new Employee { Id = "3", Name = "Michael Johnson", Age = 35, Email = "michael.johnson@example.com", Address = "789 Elm St, Othertown, USA", DepartmentId = "3", JobTitle = "Sales Manager" },
            new Employee { Id = "bae9", DepartmentId = "1" },
            new Employee { Id = "4", Name = "Magda Steawart", Age = 30, Email = "magda.steawart@example.com", Address = "214 Pine St, TinaTawon, USA", DepartmentId = "3", JobTitle = "HR Specialist" },
            new Employee { Id = "5", Name = "Jhon Smith", Age = 35, Email = "john.smith@example.com", Address = "215 Pine St, TinaTawon, USA", DepartmentId = "3", JobTitle = "HR Specialist" },
            new Employee { Id = "6", Name = "Jhon Doe", Age = 35, Email = "john.doe@example.com", Address = "215 Pine St, TinaTawon, USA", DepartmentId = "3", JobTitle = "HR Specialist" }
        };

        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Query");
            descriptor.Field("FirstQuery")
                .Argument("departmentId", arg => arg.Type<NonNullType<StringType>>())
                .Type<ListType<EmployeeType>>()
                .Resolver(context =>
                {
                    var departmentId = context.ArgumentValue<string>("departmentId");
                    return _employees.Where(e => e.DepartmentId == departmentId).ToList();
                });
        }
    }
    public class EmployeeType : ObjectType<Employee>
    {
        protected override void Configure(IObjectTypeDescriptor<Employee> descriptor)
        {
            descriptor.Field(e => e.Id).Type<NonNullType<StringType>>();
            descriptor.Field(e => e.Name).Type<NonNullType<StringType>>();
            descriptor.Field(e => e.Age).Type<NonNullType<IntType>>();
            descriptor.Field(e => e.Email).Type<NonNullType<StringType>>();
            descriptor.Field(e => e.Address).Type<NonNullType<StringType>>();
            descriptor.Field(e => e.DepartmentId).Type<NonNullType<StringType>>();
            descriptor.Field(e => e.JobTitle).Type<NonNullType<StringType>>();
        }
    }

}
