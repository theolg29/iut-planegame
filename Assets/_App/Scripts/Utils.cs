using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Utils : MonoBehaviour
{

	public static int readDictionary()
	{
		int counter = 0;

		// Read the file and display it line by line.  
		foreach (string line in System.IO.File.ReadLines(@"c:\dictionary.txt")) {
			System.Console.WriteLine(line);
			counter++;
		}

		System.Console.WriteLine("There were {0} lines.", counter);
		// Suspend the screen.  
		System.Console.ReadLine();

		return counter;
	}
}
