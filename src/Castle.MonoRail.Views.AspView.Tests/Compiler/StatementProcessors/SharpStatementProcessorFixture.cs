// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
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

namespace Castle.MonoRail.Views.AspView.Tests.Compiler.StatementProcessors
{
	using AspView.Compiler.StatementProcessors;
	using AspView.Compiler.StatementProcessors.OutputMethodGenerators;
	using NUnit.Framework;

	[TestFixture]
	public class SharpStatementProcessorFixture
	{
		SharpStatementProcessor processor;

		public SharpStatementProcessorFixture()
		{
			SetUp();
		}

		[SetUp]
		public void SetUp()
		{
			processor = new SharpStatementProcessor();
		}

		[Test]
		public void CanHandle_WhenStartsWithSharpSign_Matches()
		{
			string statement = "# foo";
			Assert.IsTrue(processor.CanHandle(statement));
		}

		[Test]
		public void CanHandle_WhenStartsWithWhitespaceAndSharpSign_Matches()
		{
			string statement = "  # foo";
			Assert.IsTrue(processor.CanHandle(statement));
		}

		[Test]
		public void CanHandle_WhenDoesNotStartsWithArbitraryWhitespaceAndSharpSign_DoNotMatches()
		{
			string statement = " foo";
			Assert.IsFalse(processor.CanHandle(statement));
		}

		[Test]
		public void GetInfo_WhenStartsWithSharpSign_GetsCorrectInfo()
		{
			string statement = "# foo";

			string expectedContent = "foo";

			StatementInfo statementInfo = processor.GetInfoFor(statement);

			Assert.AreEqual(expectedContent, statementInfo.Content);

			Assert.AreEqual(typeof(EncodedOutputMethodGenerator), statementInfo.Generator.GetType());
		}

		[Test]
		public void GetInfo_WhenStartsWithWhitespaceAndSharpSign_GetsCorrectInfo()
		{
			string statement = "  # foo";

			string expectedContent = "foo";

			StatementInfo statementInfo = processor.GetInfoFor(statement);

			Assert.AreEqual(expectedContent, statementInfo.Content);

			Assert.AreEqual(typeof(EncodedOutputMethodGenerator), statementInfo.Generator.GetType());
		}


	}
}
