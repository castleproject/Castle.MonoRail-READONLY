﻿// Copyright 2004-2008 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.Facilities.WcfIntegration.Tests.Rest
{
	using System;
	using System.ServiceModel;
	using Castle.MicroKernel.Registration;
	using Castle.Windsor;
	using Castle.Facilities.WcfIntegration.Rest;
	using NUnit.Framework;

#if DOTNET35

	[TestFixture]
	public class RestClientFixture
	{
		#region Setup/Teardown

		[SetUp]
		public void TestInitialize()
		{
			windsorContainer = new WindsorContainer()
				.AddFacility<WcfFacility>()
				.Register(Component.For<ICalculator>()
					.ImplementedBy<Calculator>()
					.DependsOn(new { number = 42 })
					.ActAs(new RestServiceModel("http://localhost:27198"))
				);
		}

		[TearDown]
		public void TestCleanup()
		{
			windsorContainer.Dispose();
		}

		#endregion

		private IWindsorContainer windsorContainer;

		[Test]
		public void CanResolveClientAssociatedWithWebChannel()
		{
			windsorContainer.Register(
				Component.For<ICalculator>()
					.Named("calculator")
					.ActAs(new RestClientModel("http://localhost:27198"))
				);

			ICalculator calculator = windsorContainer.Resolve<ICalculator>("calculator");
			Assert.AreEqual(21, calculator.Multiply(3, 7));
		}

		[Test]
		public void CanResolveExplicitClientAssociatedWithWebChannel()
		{
			windsorContainer.Register(
				Component.For<ICalculator>()
					.Named("calculator")
					.ActAs(new RestClientModel()
					{
						Endpoint = WcfEndpoint
							.BoundTo(new WebHttpBinding())
							.At("http://localhost:27198")
					})
				);

			ICalculator calculator = windsorContainer.Resolve<ICalculator>("calculator");
			Assert.AreEqual(75, calculator.Subtract(100, 25));
		}
	}
#endif // DOTNET35
}