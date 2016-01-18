using Chat.Esperance.Xamarin.Forms;
using EsperanceClient.Pages;
using System;

using Xamarin.Forms;

namespace EsperanceClient
{
	public class App : EsperanceApplication
	{
		public App ()
		{
			// The root page of your application

            if (AppBootstrap.LoggedIn)
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new LoginPage();
            }
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

