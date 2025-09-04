using System;

namespace Barin.Framework.Common.Helpers;

public static class GuidHelper
{
    private const string _guid = "00000000-0000-0000-0000-000000000000";

    public static bool IsGuid(this string guid) => Guid.TryParse(guid, out Guid id);

    public static bool IsZero(this Guid guid) => guid == default(Guid);

    public static bool IsZero(this string guid) => guid == _guid;
  
    public static Guid NewId => Guid.NewGuid();

    public static Guid Default => Guid.Parse(_guid);
}
