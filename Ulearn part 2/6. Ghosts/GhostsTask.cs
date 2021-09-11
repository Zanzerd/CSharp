using System;
using System.Text;

namespace hashes
{
	public class GhostsTask :
		IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
		IMagic
	{
		Vector vec = new Vector(10, 10);
		Segment seg = new Segment(new Vector(10, 10), new Vector(20, 20));
		Cat cat = new Cat("Caesar", "Canadian Sphynx", DateTime.Parse("5/1/2008 8:30:52 AM",
						  System.Globalization.CultureInfo.InvariantCulture));
		Robot rob = new Robot("12", 111);
		static string title = "doom 2";
		static Encoding enc = Encoding.Unicode;
		static byte[] arr = new byte[2] { 10, 20};
		Document doc = new Document(title, enc, arr);
		public void DoMagic()
		{
			vec.Add(vec);
			seg.Start.Add(seg.End);
			cat.Rename("Augustus");
			Robot.BatteryCapacity++;
			//title = "doom 2016";
			//enc = Encoding.Unicode;
			int a1 = doc.GetHashCode();
			//arr = new byte[3] { 1, 212, 123 };
			string str = doc.Content;
			arr[0] = 21;
			arr[0] = 232;
			int a2 = doc.GetHashCode();
		}

		Vector IFactory<Vector>.Create()
		{
			return vec;
		}

		Segment IFactory<Segment>.Create()
		{
			return seg;
		}

		Cat IFactory<Cat>.Create()
        {
			return cat;
		}
		
		Robot IFactory<Robot>.Create()
        {
			return rob;
		}

		Document IFactory<Document>.Create()
        {
			arr[0] = 121;
			return doc;
		}

	}
}