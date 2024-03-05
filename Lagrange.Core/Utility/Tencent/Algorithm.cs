using System.Runtime.CompilerServices;

namespace Lagrange.Core.Utils.Tencent;

public static partial class Algorithm
{
	private static void SubA(byte[] a, uint[] b)
	{
		var byteB = ToBytes(b);
		var seq = new byte[] {3, 2, 1, 0, 7, 6, 5, 4, 11, 10, 9, 8, 15, 14, 13, 12};
		for (var i = 0; i < seq.Length; i++) a[i] = (byte)(a[i] ^ byteB[seq[i]]);
	}

	private static void SubB(byte[] a, uint[] b)
	{
		var byteB = ToBytes(b);
		var seq = new byte[] {3, 6, 9, 12, 7, 10, 13, 0, 11, 14, 1, 4, 15, 2, 5, 8};
		for (var i = 0; i < seq.Length; i++) a[i] = (byte)(a[i] ^ byteB[seq[i]]);
	}

	private static void SubC(byte[][] a, byte[] b)
	{
		for (int i = 0; i < 16; i++)
		{
			int t = b[i];
			t =  (int)(t & 0xffffffff);
			t = (t & 15) + ((t >> 4) << 4);
			b[i] = a[t / 16][t % 16];
		}
	}

	private static void SubD(byte[] a, byte[] b)
	{
		var tb = new byte[b.Length];
		b.CopyTo(tb, 0);
		
		for (int i = 0; i < a.Length; i++) b[i] = tb[a[i]];
	}

	private static void SubE(byte[][] di, byte[] p)
	{
		for (int i = 4; i > 0; i--)
		{
			byte a = p[4 * i - 4];
			byte b = p[4 * i - 3];
			byte c = p[4 * i - 2];
			byte d = p[4 * i - 1];
			p[4 * i - 4] = (byte)((c ^ d) ^ (di[a][0] ^ di[b][1]));
			p[4 * i - 3] = (byte)((a ^ d) ^ (di[b][0] ^ di[c][1]));
			p[4 * i - 2] = (byte)((a ^ b) ^ (di[d][1] ^ di[c][0]));
			p[4 * i - 1] = (byte)((b ^ c) ^ (di[a][1] ^ di[d][0]));
		}
	}

	private static uint[] SubF(byte[] a, uint[] b, byte[][] c)
	{
		var w = new uint[44];
		w[0] = Read(a, 0);
		w[1] = Read(a, 4);
		w[2] = Read(a, 8);
		w[3] = Read(a, 12);

		for (var bx = 4; bx < 44; bx++)
		{
			var ax = w[bx - 1] >> 0;
			if (bx % 4 == 0)
			{
				ax = ax << 8 | (ax >> 24) & 0xff;
				ax = SubAb(c, ax);
				int dx = bx - 1;
				dx >>= 2;
				ax ^= b[dx];
			}

			ax ^= w[bx - 4];
			w[bx] = ax;
		}

		return w;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static uint Read(byte[]a, int i) => (a[i] & 0xffffffff) << 24 | (a[i + 1] & 0xffffffff) << 16 | (a[i + 2] & 0xffffffff) << 8 | (a[i + 3] & 0xffffffff);
	}
	
	private static byte SubAa(int a, byte[,,,] b, byte[] c, byte[] d)
	{
		int dx = ((a & 15) << 9) + ((d[a] & 15) << 4);
		int di = (c[a & 15] >> 4) & 15;
		int ax = (((d[a] >> 4) & 15) << 4) + ((a & 15) << 9) + di;
		byte res = b[ax / 512, ax % 512 / 256, ax % 512 % 256 / 16, ax % 512 % 256 % 16 % 16];
		res = (byte)(res << 4);
		dx = dx + (c[a & 15] & 15) + 256;
		res = (byte)(res | b[dx / 512, dx % 512 / 256, dx % 512 % 256 / 16, dx % 512 % 256 % 16 % 16]);
		return res;
	}

	private static uint SubAb(byte[][] a, uint b)
	{
		uint ia = ((b >> 28) << 4) + ((b >> 24) & 15);
		byte ax = a[ia >> 4][ia & 15];
		uint ib = b & 0xff;
		byte bx = a[ib >> 4][ib & 15];
		uint ic = ((b >> 8) & 15) + ((b >> 8) & 240);
		byte cx = a[ic >> 4][ic & 15];
		uint id = ((b >> 16) & 15) + ((b >> 16) & 240);
		byte dx = a[id >> 4][id & 15];
		return (uint)((ax << 24) | bx | (cx << 8) | (dx << 16));
	}

	private static void SubAd(ref State a)
	{
		uint r10 = a.StateList[10];
		uint r12 = a.StateList[3];
		uint bp = a.StateList[11];
		uint dx = a.StateList[4];
		uint r15 = a.StateList[0];
		uint r9 = a.StateList[12];
		uint si = a.StateList[5];
		uint r11 = a.StateList[8];
		r15 += dx;
		uint r14 = a.StateList[1];
		uint r8 = a.StateList[13];
		r9 ^= r15;
		uint cx = a.StateList[6];
		uint r13 = a.StateList[2];
		r9 = Roll(r9, 16);
		r14 += si;
		uint bx = a.StateList[9];
		uint di = a.StateList[14];
		r11 += r9;
		r8 ^= r14;
		r13 += cx;
		dx ^= r11;
		r8 = Roll(r8, 16);
		di ^= r13;
		dx = Roll(dx, 12);
		bx += r8;
		di = Roll(di, 16);
		r15 += dx;
		si ^= bx;
		r10 += di;
		r9 ^= r15;
		si = Roll(si, 12);
		cx ^= r10;
		r9 = Roll(r9, 8);
		r14 += si;
		cx = Roll(cx, 12);
		r11 += r9;
		r8 ^= r14;
		r13 += cx;
		dx ^= r11;
		r8 = Roll(r8, 8);
		di ^= r13;
		dx = Roll(dx, 7);
		bx += r8;
		di = Roll(di, 8);
		uint tmp0 = dx;
		dx = a.StateList[7];
		si ^= bx;
		uint tmp1 = bx;
		bx = r10;
		r10 = a.StateList[15];
		si = Roll(si, 7);
		bx += di;
		r12 += dx;
		r15 += si;
		r10 ^= r12;
		cx ^= bx;
		r10 = Roll(r10, 16);
		cx = Roll(cx, 7);
		bp += r10;
		r14 += cx;
		dx ^= bp;
		r9 ^= r14;
		dx = Roll(dx, 12);
		r9 = Roll(r9, 16);
		r12 += dx;
		r10 ^= r12;
		r10 = Roll(r10, 8);
		bp += r10;
		r10 ^= r15;
		r10 = Roll(r10, 16);
		dx ^= bp;
		bp += r9;
		bx += r10;
		dx = Roll(dx, 7);
		cx ^= bp;
		si ^= bx;
		si = Roll(si, 12);
		r15 += si;
		r10 ^= r15;
		a.StateList[0] = r15;
		r10 = Roll(r10, 8);
		bx += r10;
		a.StateList[15] = r10;
		si ^= bx;
		uint x1 = bx;
		si = Roll(si, 7);
		cx = Roll(cx, 12);
		r13 += dx;
		r8 ^= r13;
		r14 += cx;
		a.StateList[5] = si;
		r8 = Roll(r8, 16);
		r9 ^= r14;
		a.StateList[1] = r14;
		r11 += r8;
		r9 = Roll(r9, 8);
		dx ^= r11;
		bp += r9;
		a.StateList[12] = r9;
		dx = Roll(dx, 12);
		cx ^= bp;
		uint x2 = bp;
		r13 += dx;
		cx = Roll(cx, 7);
		r8 ^= r13;
		a.StateList[6] = cx;
		r8 = Roll(r8, 8);
		a.StateList[2] = r13;
		r11 += r8;
		dx ^= r11;
		uint x0 = r11;
		dx = Roll(dx, 7);
		a.StateList[7] = dx;
		a.StateList[13] = r8;
		si = tmp0;
		cx = tmp1;
		r12 += si;
		di ^= r12;
		di = Roll(di, 16);
		cx += di;
		si ^= cx;
		dx = si;
		dx = Roll(dx, 12);
		r12 += dx;
		di ^= r12;
		a.StateList[3] = r12;
		di = Roll(di, 8);
		cx += di;
		a.StateList[14] = di;
		uint x3 = cx;
		dx ^= cx;
		dx = Roll(dx, 7);
		a.StateList[4] = dx;
		a.StateList[8] = x0;
		a.StateList[9] = x3;
		a.StateList[10] = x1;
		a.StateList[11] = x2;
	}

	private static void TransformInner(byte[] a, uint[,] b)
	{
		uint si = 0;
		for (uint cnt = 1; cnt < 43;)
		{
			byte ai = (byte)(((cnt - 1 & 31) << 4) + (a[si] >> 4));
			byte bi = (byte)(((cnt & 31) << 4) + (a[si] & 15));
			a[si] = (byte)(b[ai >> 4, ai & 15] << 4 | b[bi >> 4, bi & 15]);
			si++;
			cnt += 2;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint Roll(uint a, int n) => a << n | a >> (32 - n);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static byte[] ToBytes(uint[] a)
	{
		var ans = new byte[16];
		for (int i = 0; i < a.Length; i++)
		{
			uint val = a[i];
			ans[4 * i] = (byte)(val & 0xff);
			ans[4 * i + 1] = (byte)(val >> 8 & 0xff);
			ans[4 * i + 2] = (byte)(val >> 16 & 0xff);
			ans[4 * i + 3] = (byte)(val >> 24 & 0xff);
		}
		return ans;
	}
}