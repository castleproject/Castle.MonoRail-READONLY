// Copyright 2004-2005 Castle Project - http://www.castleproject.org/
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

namespace Castle.Windsor.Tests.Configuration.CastleLanguage
{
	using System;
	using System.IO;

	using NUnit.Framework;

	using Castle.Model.Configuration;

	using Castle.Windsor.Configuration.CastleLanguage;


	[TestFixture]
	public class ParserTestCase
	{
		[Test]
		public void SingleNodeAndAttributes()
		{
			String contents = "container: \r\n  item: value\r\n  item2: value2\r\n";

			WindsorConfLanguageLexer l = 
				new WindsorConfLanguageLexer(new StringReader(contents));

			WindsorLanguageParser p = new WindsorLanguageParser(new IndentTokenStream(l));
			
			ConfigurationDefinition conf = p.start();
			Assert.IsNotNull(conf);
			Assert.AreEqual(0, conf.Imports.Count);
			Assert.IsNotNull(conf.Root);
			Assert.IsNotNull(conf.Root.Children["container"]);
			Assert.AreEqual("value", conf.Root.Children["container"].Attributes["item"]);
			Assert.AreEqual("value2", conf.Root.Children["container"].Attributes["item2"]);
			Assert.AreEqual(2, conf.Root.Children["container"].Attributes.Count);
		}

		[Test]
		public void SingleNodeAndAttributesUsingTab()
		{
			String contents = "container:\r\n\titem: value\r\n\titem2: value2\r\n";

			WindsorConfLanguageLexer l = 
				new WindsorConfLanguageLexer(new StringReader(contents));

			WindsorLanguageParser p = new WindsorLanguageParser(new IndentTokenStream(l));
			
			ConfigurationDefinition conf = p.start();
			Assert.IsNotNull(conf);
			Assert.AreEqual(0, conf.Imports.Count);
			Assert.IsNotNull(conf.Root);
			Assert.IsNotNull(conf.Root.Children["container"]);
			Assert.AreEqual("value", conf.Root.Children["container"].Attributes["item"]);
			Assert.AreEqual("value2", conf.Root.Children["container"].Attributes["item2"]);
			Assert.AreEqual(2, conf.Root.Children["container"].Attributes.Count);
		}

		[Test]
		public void Tree3()
		{
			String contents = "container: \r\n  item: value\r\n  item2: value2\r\n  sub: \r\n    other1: value3\r\n    other2: value4\r\n";

			WindsorConfLanguageLexer l = 
				new WindsorConfLanguageLexer(new StringReader(contents));

			WindsorLanguageParser p = new WindsorLanguageParser(new IndentTokenStream(l));
			
			ConfigurationDefinition conf = p.start();
			Assert.IsNotNull(conf);
			Assert.AreEqual(0, conf.Imports.Count);
			Assert.IsNotNull(conf.Root);
			Assert.IsNotNull(conf.Root.Children["container"]);
			Assert.AreEqual("value", conf.Root.Children["container"].Attributes["item"]);
			Assert.AreEqual("value2", conf.Root.Children["container"].Attributes["item2"]);
			Assert.AreEqual(2, conf.Root.Children["container"].Attributes.Count);

			IConfiguration sub = conf.Root.Children["container"].Children["sub"];
			Assert.IsNotNull(sub);
			Assert.AreEqual("value3", sub.Attributes["other1"]);
			Assert.AreEqual("value4", sub.Attributes["other2"]);
			Assert.AreEqual(2, sub.Attributes.Count);
		}

		[Test]
		public void Dots()
		{
			String contents = "container: \r\n  item.1: my.key\r\n  item.2: value2\r\n  sub.sub: \r\n    other1: value3\r\n    other2: value4\r\n";

			WindsorConfLanguageLexer l = 
				new WindsorConfLanguageLexer(new StringReader(contents));

			WindsorLanguageParser p = new WindsorLanguageParser(new IndentTokenStream(l));
			
			ConfigurationDefinition conf = p.start();
			Assert.IsNotNull(conf);
			Assert.AreEqual(0, conf.Imports.Count);
			Assert.IsNotNull(conf.Root);
			Assert.IsNotNull(conf.Root.Children["container"]);
			Assert.AreEqual("my.key", conf.Root.Children["container"].Attributes["item.1"]);
			Assert.AreEqual("value2", conf.Root.Children["container"].Attributes["item.2"]);
			Assert.AreEqual(2, conf.Root.Children["container"].Attributes.Count);

			IConfiguration sub = conf.Root.Children["container"].Children["sub.sub"];
			Assert.IsNotNull(sub);
			Assert.AreEqual("value3", sub.Attributes["other1"]);
			Assert.AreEqual("value4", sub.Attributes["other2"]);
			Assert.AreEqual(2, sub.Attributes.Count);
		}

		[Test]
		public void Comments1()
		{
			String contents = "// comments\r\ncontainer: \r\n  item.1: my.key\r\n  item.2: value2\r\n";

			WindsorConfLanguageLexer l = 
				new WindsorConfLanguageLexer(new StringReader(contents));

			WindsorLanguageParser p = new WindsorLanguageParser(new IndentTokenStream(l));
			
			ConfigurationDefinition conf = p.start();
			Assert.IsNotNull(conf);
			Assert.AreEqual(0, conf.Imports.Count);
			Assert.IsNotNull(conf.Root);
			Assert.IsNotNull(conf.Root.Children["container"]);
			Assert.AreEqual("my.key", conf.Root.Children["container"].Attributes["item.1"]);
			Assert.AreEqual("value2", conf.Root.Children["container"].Attributes["item.2"]);
			Assert.AreEqual(2, conf.Root.Children["container"].Attributes.Count);
		}

		[Test]
		public void Comments2()
		{
			String contents = "container: \r\n// comments\r\n  item.1: my.key\r\n  item.2: value2\r\n";

			WindsorConfLanguageLexer l = 
				new WindsorConfLanguageLexer(new StringReader(contents));

			WindsorLanguageParser p = new WindsorLanguageParser(new IndentTokenStream(l));
			
			ConfigurationDefinition conf = p.start();
			Assert.IsNotNull(conf);
			Assert.AreEqual(0, conf.Imports.Count);
			Assert.IsNotNull(conf.Root);
			Assert.IsNotNull(conf.Root.Children["container"]);
			Assert.AreEqual("my.key", conf.Root.Children["container"].Attributes["item.1"]);
			Assert.AreEqual("value2", conf.Root.Children["container"].Attributes["item.2"]);
			Assert.AreEqual(2, conf.Root.Children["container"].Attributes.Count);
		}

		[Test]
		public void Comments3()
		{
			String contents = "container: \r\n  // comments\r\n  item.1: my.key\r\n  item.2: value2\r\n";

			WindsorConfLanguageLexer l = 
				new WindsorConfLanguageLexer(new StringReader(contents));

			WindsorLanguageParser p = new WindsorLanguageParser(new IndentTokenStream(l));
			
			ConfigurationDefinition conf = p.start();
			Assert.IsNotNull(conf);
			Assert.AreEqual(0, conf.Imports.Count);
			Assert.IsNotNull(conf.Root);
			Assert.IsNotNull(conf.Root.Children["container"]);
			Assert.AreEqual("my.key", conf.Root.Children["container"].Attributes["item.1"]);
			Assert.AreEqual("value2", conf.Root.Children["container"].Attributes["item.2"]);
			Assert.AreEqual(2, conf.Root.Children["container"].Attributes.Count);
		}
	}
}
