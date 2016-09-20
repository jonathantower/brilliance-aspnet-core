using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Samples.Models;

namespace Samples.ViewComponents
{
	public class NewsViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(int numberOfItems)
		{
			var model = new List<StoryModel>(new []
			{
				new StoryModel { Title = "Local Man Writes Story", Url = "http://news.yahoo.com" },
				new StoryModel { Title = "Your Teams Falls to Other Team", Url = "http://news.google.com" },
				new StoryModel { Title = "Latest News Causes Latest Market Changes", Url = "http://cnn.com" }
			});

			return View(model);
	}
}
}
