// See https://aka.ms/new-console-template for more information

using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Training.EfCore.ConsoleApp.Models;

EntityFrameworkProfiler.Initialize();

var options = new DbContextOptionsBuilder<StackOverflow2010Context>()
	.LogTo(Console.WriteLine, LogLevel.Information)
	.UseSqlServer("Server=.;Database=StackOverflow2010;Trusted_Connection=True;", opt => opt.CommandTimeout(30))
	.Options;

var context = new StackOverflow2010Context(options);

var result = await context.Users.OrderByDescending(i => i.Id).Take(10).ToListAsync();

Console.WriteLine(result.Count);