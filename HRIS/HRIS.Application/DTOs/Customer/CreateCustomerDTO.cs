﻿namespace HRIS.Application.DTOs.Customer
{
    public class CreateCustomerDTO
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int BranchId { get; set; }
    }
}
