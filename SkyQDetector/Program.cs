using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rssdp;

namespace SkyQDetector
{
	class Program
	{
		static List<string> skyList = new List<string>();


		/* Sky Q boxes broadcast their existence through the SSDP protocol. 
		 * This is a small tool, which is designed to act as an extension of an existing project that detects the Q boxes in a network.
		 */
		static void Main(string[] args)
		{
			GetQBoxes().Wait();
		}


		private static async Task GetQBoxes()
		{
			Console.WriteLine("Finding Sky Q Boxes");

			using (var deviceLocator = new SsdpDeviceLocator())
			{
				var foundQ = await deviceLocator.SearchAsync();
				foreach (var device in foundQ)
				{
					string deviceBuffer = device.ToString();
					if (deviceBuffer.Contains("SkyPlay") == true)
					{
						if (device.DescriptionLocation.ToString().Contains("description5") == true)
						{
							continue;
						}
						else
						{
							/*Sky Q Mini boxes seem to broadcast 'description0.xml'
							 * Sky Q Gateways seem to broadcast 'description1.xml'
							 * Through this, we can create a method to determine what kind of boxes are on the network.
							*/
							if (device.DescriptionLocation.ToString().Contains("description0") == true)
							{
								skyList.Add("Sky Q Mini FOUND!");
								skyList.Add(device.DescriptionLocation.ToString());
							}
							else
							{
								skyList.Add("Sky Q Gateway FOUND!");
								skyList.Add(device.DescriptionLocation.ToString());
							}

						}
					}
				}
					if (skyList.Count==0) 
					{
                        Console.WriteLine("No Q boxes found, possibly due to broadcast gap. Trying again.");
						GetQBoxes().Wait();

					}
					for (int i = 0; i < skyList.Count; i++)
					{
						Console.WriteLine(skyList[i]);
					}
				}
			Console.WriteLine();
			Console.WriteLine("Process Done");
			Console.ReadKey();
		}
	}
}
