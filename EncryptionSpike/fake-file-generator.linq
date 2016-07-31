<Query Kind="Program" />

void Main()
{
	var fakeObjectionsFile = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                fakeObjectionsFile.AppendLine("1234567890|this is a line of an input file| heres some more sensitive data");
            }
			
			File.WriteAllText(@".\FakeFile.csv", fakeObjectionsFile.ToString());
}

// Define other methods and classes here
