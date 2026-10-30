using System;
using System.Collections.Generic;

namespace OctTree;

internal static class OctShell
{
	private sealed class Sidebuffer
	{
		public int x;

		public int y;

		public int z;

		public int side;
	}

	private const int X = 0;

	private const int Y = 1;

	private const int Z = 2;

	private const int Xp = 0;

	private const int Xm = 1;

	private const int Yp = 2;

	private const int Ym = 3;

	private const int Zp = 4;

	private const int Zm = 5;

	private static int[] d0 = new int[3] { 1, 0, 0 };

	private static int[] d1 = new int[3] { -1, 0, 0 };

	private static int[] d2 = new int[3] { 0, 1, 0 };

	private static int[] d3 = new int[3] { 0, -1, 0 };

	private static int[] d4 = new int[3] { 0, 0, 1 };

	private static int[] d5 = new int[3] { 0, 0, -1 };

	private static int[][] dirs = new int[6][] { d0, d1, d2, d3, d4, d5 };

	private static testDir[] XP = new testDir[4]
	{
		new testDir
		{
			test1D = new int[3] { 1, 1, 0 },
			test1R = 3,
			test2D = new int[3] { 0, 1, 0 },
			test2R = 0,
			test3R = 2
		},
		new testDir
		{
			test1D = new int[3] { 1, -1, 0 },
			test1R = 2,
			test2D = new int[3] { 0, -1, 0 },
			test2R = 0,
			test3R = 3
		},
		new testDir
		{
			test1D = new int[3] { 1, 0, 1 },
			test1R = 5,
			test2D = new int[3] { 0, 0, 1 },
			test2R = 0,
			test3R = 4
		},
		new testDir
		{
			test1D = new int[3] { 1, 0, -1 },
			test1R = 4,
			test2D = new int[3] { 0, 0, -1 },
			test2R = 0,
			test3R = 5
		}
	};

	private static testDir[] XM = new testDir[4]
	{
		new testDir
		{
			test1D = new int[3] { -1, 1, 0 },
			test1R = 3,
			test2D = new int[3] { 0, 1, 0 },
			test2R = 1,
			test3R = 2
		},
		new testDir
		{
			test1D = new int[3] { -1, -1, 0 },
			test1R = 2,
			test2D = new int[3] { 0, -1, 0 },
			test2R = 1,
			test3R = 3
		},
		new testDir
		{
			test1D = new int[3] { -1, 0, 1 },
			test1R = 5,
			test2D = new int[3] { 0, 0, 1 },
			test2R = 1,
			test3R = 4
		},
		new testDir
		{
			test1D = new int[3] { -1, 0, -1 },
			test1R = 4,
			test2D = new int[3] { 0, 0, -1 },
			test2R = 1,
			test3R = 5
		}
	};

	private static testDir[] YP = new testDir[4]
	{
		new testDir
		{
			test1D = new int[3] { 1, 1, 0 },
			test1R = 1,
			test2D = new int[3] { 1, 0, 0 },
			test2R = 2,
			test3R = 0
		},
		new testDir
		{
			test1D = new int[3] { -1, 1, 0 },
			test1R = 0,
			test2D = new int[3] { -1, 0, 0 },
			test2R = 2,
			test3R = 1
		},
		new testDir
		{
			test1D = new int[3] { 0, 1, 1 },
			test1R = 5,
			test2D = new int[3] { 0, 0, 1 },
			test2R = 2,
			test3R = 4
		},
		new testDir
		{
			test1D = new int[3] { 0, 1, -1 },
			test1R = 4,
			test2D = new int[3] { 0, 0, -1 },
			test2R = 2,
			test3R = 5
		}
	};

	private static testDir[] YM = new testDir[4]
	{
		new testDir
		{
			test1D = new int[3] { 1, -1, 0 },
			test1R = 1,
			test2D = new int[3] { 1, 0, 0 },
			test2R = 3,
			test3R = 0
		},
		new testDir
		{
			test1D = new int[3] { -1, -1, 0 },
			test1R = 0,
			test2D = new int[3] { -1, 0, 0 },
			test2R = 3,
			test3R = 1
		},
		new testDir
		{
			test1D = new int[3] { 0, -1, 1 },
			test1R = 5,
			test2D = new int[3] { 0, 0, 1 },
			test2R = 3,
			test3R = 4
		},
		new testDir
		{
			test1D = new int[3] { 0, -1, -1 },
			test1R = 4,
			test2D = new int[3] { 0, 0, -1 },
			test2R = 3,
			test3R = 5
		}
	};

	private static testDir[] ZP = new testDir[4]
	{
		new testDir
		{
			test1D = new int[3] { 1, 0, 1 },
			test1R = 1,
			test2D = new int[3] { 1, 0, 0 },
			test2R = 4,
			test3R = 0
		},
		new testDir
		{
			test1D = new int[3] { -1, 0, 1 },
			test1R = 0,
			test2D = new int[3] { -1, 0, 0 },
			test2R = 4,
			test3R = 1
		},
		new testDir
		{
			test1D = new int[3] { 0, 1, 1 },
			test1R = 3,
			test2D = new int[3] { 0, 1, 0 },
			test2R = 4,
			test3R = 2
		},
		new testDir
		{
			test1D = new int[3] { 0, -1, 1 },
			test1R = 2,
			test2D = new int[3] { 0, -1, 0 },
			test2R = 4,
			test3R = 3
		}
	};

	private static testDir[] ZM = new testDir[4]
	{
		new testDir
		{
			test1D = new int[3] { 1, 0, -1 },
			test1R = 1,
			test2D = new int[3] { 1, 0, 0 },
			test2R = 5,
			test3R = 0
		},
		new testDir
		{
			test1D = new int[3] { -1, 0, -1 },
			test1R = 0,
			test2D = new int[3] { -1, 0, 0 },
			test2R = 5,
			test3R = 1
		},
		new testDir
		{
			test1D = new int[3] { 0, 1, -1 },
			test1R = 3,
			test2D = new int[3] { 0, 1, 0 },
			test2R = 5,
			test3R = 2
		},
		new testDir
		{
			test1D = new int[3] { 0, -1, -1 },
			test1R = 2,
			test2D = new int[3] { 0, -1, 0 },
			test2R = 5,
			test3R = 3
		}
	};

	private static testDir[][] tests = new testDir[6][] { XP, XM, YP, YM, ZP, ZM };

	public static void shell(OctTree tb, out long Volume, out int surfaceArea, out int pixelCount)
	{
		int num = (int)Math.Pow(2.0, OctTree.Depth);
		pixelCount = 0;
		int[] array = new int[6];
		bool[] pSides = null;
		List<Sidebuffer> list = new List<Sidebuffer>();
		long volume = 0L;
		bool flag = true;
		for (double num2 = 0.1; num2 <= 0.9; num2 += 0.01)
		{
			int y = (int)((double)num * num2);
			for (double num3 = 0.1; num3 <= 0.9; num3 += 0.01)
			{
				int z = (int)((double)num * num3);
				for (int i = 0; i < num; i++)
				{
					if (!tb.GetPixel(i, y, z, out pSides))
					{
						continue;
					}
					if (flag)
					{
						if (!pSides[1])
						{
							pSides[1] = true;
							array[1]++;
							pSides[6] = true;
							pixelCount++;
							list.Add(new Sidebuffer
							{
								x = i,
								y = y,
								z = z,
								side = 1
							});
							processQueue(tb, list, array, ref pixelCount, ref volume);
						}
						flag = false;
					}
					if (!flag && pSides[0])
					{
						flag = true;
					}
				}
			}
		}
		Volume = volume;
		surfaceArea = array[0] + array[1] + array[2] + array[3] + array[4] + array[5];
	}

	private static void processQueue(OctTree tb, List<Sidebuffer> queue, int[] sideTotals, ref int pixelCount, ref long volume)
	{
		while (queue.Count > 0)
		{
			Sidebuffer sidebuffer = queue[0];
			queue.RemoveAt(0);
			if (tb.GetPixel(sidebuffer.x + dirs[sidebuffer.side][0], sidebuffer.y + dirs[sidebuffer.side][1], sidebuffer.z + dirs[sidebuffer.side][2], out var pSides))
			{
				Console.WriteLine("LogicError");
			}
			testDir[] array = tests[sidebuffer.side];
			for (int i = 0; i < 4; i++)
			{
				testDir testDir2 = array[i];
				int x = sidebuffer.x + testDir2.test1D[0];
				int y = sidebuffer.y + testDir2.test1D[1];
				int num = sidebuffer.z + testDir2.test1D[2];
				int test1R = testDir2.test1R;
				if (tb.GetPixel(x, y, num, out pSides))
				{
					if (!pSides[test1R])
					{
						pSides[test1R] = true;
						sideTotals[test1R]++;
						if (!pSides[6])
						{
							pixelCount++;
							pSides[6] = true;
						}
						switch (test1R)
						{
						case 4:
							volume += num + 1;
							break;
						case 5:
							volume -= num;
							break;
						}
						queue.Add(new Sidebuffer
						{
							x = x,
							y = y,
							z = num,
							side = test1R
						});
					}
					continue;
				}
				x = sidebuffer.x + testDir2.test2D[0];
				y = sidebuffer.y + testDir2.test2D[1];
				num = sidebuffer.z + testDir2.test2D[2];
				test1R = testDir2.test2R;
				if (tb.GetPixel(x, y, num, out pSides))
				{
					if (!pSides[test1R])
					{
						pSides[test1R] = true;
						sideTotals[test1R]++;
						if (!pSides[6])
						{
							pixelCount++;
							pSides[6] = true;
						}
						switch (test1R)
						{
						case 4:
							volume += num + 1;
							break;
						case 5:
							volume -= num;
							break;
						}
						queue.Add(new Sidebuffer
						{
							x = x,
							y = y,
							z = num,
							side = test1R
						});
					}
					continue;
				}
				x = sidebuffer.x;
				y = sidebuffer.y;
				num = sidebuffer.z;
				test1R = testDir2.test3R;
				if (tb.GetPixel(x, y, num, out pSides) && !pSides[test1R])
				{
					pSides[test1R] = true;
					sideTotals[test1R]++;
					if (!pSides[6])
					{
						pixelCount++;
						pSides[6] = true;
					}
					switch (test1R)
					{
					case 4:
						volume += num + 1;
						break;
					case 5:
						volume -= num;
						break;
					}
					queue.Add(new Sidebuffer
					{
						x = x,
						y = y,
						z = num,
						side = test1R
					});
				}
			}
		}
	}
}
