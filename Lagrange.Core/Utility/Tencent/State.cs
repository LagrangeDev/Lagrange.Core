namespace Lagrange.Core.Utils.Tencent;

public static partial class Algorithm
{
    private struct State
    {
        public uint[] StateList;
        public uint[] OrgState;
        public int Nr;
        public int P;

        public State(uint[] stateList, uint[] orgState, int nr, int p)
        {
            StateList = stateList;
            OrgState = orgState;
            Nr = nr;
            P = p;
        }

        public void Init(byte[] a, byte[] b, uint[] c, int d)
        {
            Nr = d;
            P = 0;
            InitState(ref this, a, b, c);
        }
        
        public void Encrypt(byte[] data)
        {
            int bp = 0;
            int dataLen = data.Length;
            while (dataLen > 0)
            {
                if (P == 0) RefreshState(ref this);

                var sb = new byte[64];
                for (var i = 0; i < StateList.Length; i++)
                {
                    var v = StateList[i];
                    sb[i * 4] = (byte) (v & 0x000000ff);
                    sb[i * 4 + 1] = (byte) (v >> 8 & 0x000000ff);
                    sb[i * 4 + 2] = (byte) (v >> 16 & 0x000000ff);
                    sb[i * 4 + 3] = (byte) (v >> 24 & 0x000000ff);
                }

                while (P != 64 && dataLen != 0)
                {
                    data[bp] ^= sb[P];
                    P++;
                    bp++;
                    dataLen--;
                }

                if (P >= 64)
                {
                    P = 0;
                    OrgState[12]++;
                    StateList = OrgState;
                }
            }
        }
    }

    private static void InitState(ref State a, byte[] b, byte[] c, uint[] d)
    {
        a.OrgState[0] = a.StateList[0] = 1634760805;
        a.OrgState[1] = a.StateList[1] = 857760878;
        a.OrgState[2] = a.StateList[2] = 2036477234;
        a.OrgState[3] = a.StateList[3] = 1797285236;
        
        var w1 = ReadUint128(b, 0);
        for (var i = 0; i < 4; i++)
        {
            a.StateList[4 + i] = w1[i];
            a.OrgState[4 + i] = w1[i];
        }
        
        a.StateList[12] = d[1];
        a.StateList[13] = d[0];
        var w2 = ReadUint128(b, 16);
        for (var i = 0; i < 4; i++)
        {
            a.StateList[8 + i] = w2[i];
            a.OrgState[8 + i] = w2[i];
        }
        
        var w3 = ReadUint128(c, 0);
        a.StateList[14] = w3[0];
        a.StateList[15] = w3[1];
    }
    
    // function refreshState(a: state) {
    //     let cx = Math.floor(a.nr / 2)
    //     for (let i: number = 0; i < cx; i++) {
    //         sub_ad(a)
    //     }
    //     for (let i: number = 0; i < 16; i++) {
    //         a.state[i] = a.state[i] + a.orgState[i]
    //     }
    // }

    private static void RefreshState(ref State state)
    {
        int cx = state.Nr / 2;
        for (var i = 0; i < cx; i++) SubAd(ref state);
        for (var i = 0; i < 16; i++) state.StateList[i] += state.OrgState[i];
    }

    private static uint[] ReadUint128(byte[] a, int i)
    {
        uint ans = 0;
        var w = new uint[4];
        for (var cnt = 1; cnt <= 16 && i + cnt <= a.Length; cnt++)
        {
            ans = (uint)(ans | a[i + cnt - 1] << (8 * ((cnt - 1) % 4)) & 0xffffffff);
            if (cnt % 4 == 0)
            {
                w[cnt / 4 - 1] = ans;
                ans = 0;
            }
        }
        
        return w;
    }
}