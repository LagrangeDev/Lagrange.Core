using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.System;

internal class FetchFullSysFacesEvent : ProtocolEvent
{
    public List<SysFacePackEntry> FacePacks { get; set; }

    private FetchFullSysFacesEvent(List<SysFacePackEntry> facePacks) : base(true)
    {
        FacePacks = facePacks;
    }

    private FetchFullSysFacesEvent(int resultCode, List<SysFacePackEntry> facePacks) : base(resultCode)
    {
        FacePacks = facePacks;
    }
    
    public static FetchFullSysFacesEvent Create() => new(new List<SysFacePackEntry>());
    
    public static FetchFullSysFacesEvent Result(int resultCode, List<SysFacePackEntry> emojiPacks) => new(resultCode, emojiPacks);
}