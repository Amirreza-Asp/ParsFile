﻿namespace ParsFile.Domain.Dtos.Account
{
    public class AccountInfoDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }
        public string Subscription { get; set; }
    }
}
