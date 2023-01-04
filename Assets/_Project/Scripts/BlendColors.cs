using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BlendColors
{
    public static int ColorCompare(Color one, Color two)
	{
        Pt3 first = new Pt3(one.r, one.g, one.b);
        Pt3 second = new Pt3(two.r, two.g, two.b);
        var oneColor = RGB2XYZ(first);
        var twoColor = RGB2XYZ(second);

        return Convert.ToInt32(100 - (100 * Compare(oneColor, twoColor)));
    }

    public static Color BlendMultipleColor(this List<Color> colors)
    {
        List<Color> temp = new List<Color>();
        for (int i = 0; i < colors.Count; i++)
        {
            temp.Add(colors[i].ConvertColorInt());
        }
        var color = (temp.Aggregate((p, x) => p + x) / colors.Count);
        Color returned = new Color(Convert.ToInt32(color.r), Convert.ToInt32(color.g), Convert.ToInt32(color.b));
        return returned;
    }

    public static Color Blend(Color color, Color backColor, double amount)
    {
        color.ConvertColorInt();
        backColor.ConvertColorInt();
        byte r = (byte)(color.r * amount + backColor.r * (1 - amount));
        byte g = (byte)(color.g * amount + backColor.g * (1 - amount));
        byte b = (byte)(color.b * amount + backColor.b * (1 - amount));
        var returned = new Color(r, g, b).ConvertColorUnity();
        returned.a = 1;
        
        return returned;
    }

    public static Color BlendInt(Color color, Color backColor, double amount)
    {
        color.ConvertColorInt();
        backColor.ConvertColorInt();
        byte r = (byte)(color.r * amount + backColor.r * (1 - amount));
        byte g = (byte)(color.g * amount + backColor.g * (1 - amount));
        byte b = (byte)(color.b * amount + backColor.b * (1 - amount));
        return new Color(r, g, b);
    }

	


	private static double ColorCompareChromatic(Color one, Color two)
    {
        Pt3 first = new Pt3(one.r, one.g, one.b);
        Pt3 second = new Pt3(two.r, two.g, two.b);
        var oneColor = RGB2XYZ(first);
        var twoColor = RGB2XYZ(second);

        return (100 - (100 * ChromaticCompare(oneColor, twoColor)));
    }

    private static double ColorComparer(Color one, Color two)
    {
        return Math.Sqrt((one.r - two.r) * (one.r - two.r) + (one.g - two.g) * (one.g - two.g) + (one.b - two.b) * (one.b - two.b));
    }

    private static double ChromaticDistance(Color one, Color two)
    {
        return (Math.Max(one.r, two.r) - Math.Min(one.r, two.r)) + (Math.Max(one.g, two.g) - Math.Min(one.g, two.g)) +
                (Math.Max(one.b, two.b) - Math.Min(one.b, two.b));
    }

    private static Pt3 RGB2XYZ(Pt3 x)
    {
        Pt3 srgb = x / 255.0;
        var result = new Pt3(0.4124 * srgb.x + 0.3576 * srgb.y + 0.1805 * srgb.z,
                            0.2126 * srgb.x + 0.7152 * srgb.y + 0.0722 * srgb.z,
                            0.0193 * srgb.x + 0.1192 * srgb.y + 0.9505 * srgb.z);

        return result;
    }

    private static Pt3 XYZ2RGB(Pt3 x)
    {
        double x1 = 3.2410 * x.x - 1.5374 * x.y - 0.4986 * x.z;
        double y1 = -0.9692 * x.x + 1.8760 * x.y + 0.0416 * x.z;
        double z1 = 0.0556 * x.x - 0.2040 * x.y + 1.0570 * x.z;
        var result = new Pt3(x1, y1, z1) * 255.0;
        return result;

    }

    private static double Compare(Pt3 one, Pt3 two)
	{
        return Math.Sqrt((one.x - two.x) * (one.x - two.x) + (one.y - two.y) * (one.y - two.y) + (one.z - two.z) * (one.z - two.z));
    }

    private static double ChromaticCompare(Pt3 one, Pt3 two)
	{
        return  (Math.Max(one.x, two.x) - Math.Min(one.x, two.x)) + (Math.Max(one.y, two.y) - Math.Min(one.y, two.y)) +
                (Math.Max(one.z, two.z) - Math.Min(one.z, two.z));
    }

   // public static string DebColor(this Color color, Deb.ColorText colorText = Deb.ColorText.blue)
	//{
      //  Deb.Debag(color.r + " | " + color.g + " | " + color.b, colorText);
     //   return color.r + " | " + color.g + " | " + color.b;
   // }

    public static Color ConvertColorInt(this Color color)
	{
        return color * 255;
	}

    public static Color ConvertColorUnity(this Color color)
    {
        var z = color / 255;
        z.a = 1;
        return z;
    }
}

public class Pt3
{
    public double x;
    public double y;
    public double z;

	public Pt3()
	{
	}

    public Pt3(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }


    public static Pt3 operator *(Pt3 counter1, double counter2)
    {
        var x = counter1.x * counter2;
        var y = counter1.y * counter2;
        var z = counter1.z * counter2;

        return new Pt3 (x,y,z);
    }

    public static Pt3 operator /(Pt3 counter1, double counter2)
    {
        var x = counter1.x / counter2;
        var y = counter1.y / counter2;
        var z = counter1.z / counter2;

        return new Pt3(x, y, z);
    }

}