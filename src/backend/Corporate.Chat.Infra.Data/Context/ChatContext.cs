﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Corporate.Chat.Domain.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Corporate.Chat.Infra.Data.MapConfigs;

namespace Corporate.Chat.Infra.Data.Context
{
	public class ChatContext : DbContext
	{
		private readonly IHostEnvironment _env;

		public ChatContext(IHostEnvironment env)
		{
			_env = env;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// get the configuration from the app settings
			var config = new ConfigurationBuilder()
				.SetBasePath(_env.ContentRootPath)
				.AddJsonFile($"appsettings.json")
				.AddJsonFile($"appsettings.{_env.EnvironmentName}.json", true)
				.Build();

			// define the database to use
			optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserChatConfig());
			modelBuilder.ApplyConfiguration(new MessageConfig());

		}

		public DbSet<UserChat> UsersChat { get; set; }
		public DbSet<Message> Messages { get; set; }
	}
}