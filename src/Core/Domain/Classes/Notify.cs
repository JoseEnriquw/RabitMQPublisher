﻿using System.Runtime.CompilerServices;

namespace Core.Domain.Classes
{
    public class Notify
    {
        public string Code { get; set; }=null!;

        public string Property { get; set; } = null!;
        public string Message { get; set; } = null!;

        public override string ToString()
        {
            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 3);
            defaultInterpolatedStringHandler.AppendLiteral("Notify - Code: ");
            defaultInterpolatedStringHandler.AppendFormatted(Code);
            defaultInterpolatedStringHandler.AppendLiteral(", Property: ");
            defaultInterpolatedStringHandler.AppendFormatted(Property);
            defaultInterpolatedStringHandler.AppendLiteral(", Message: ");
            defaultInterpolatedStringHandler.AppendFormatted(Message);
            return defaultInterpolatedStringHandler.ToStringAndClear();
        }
    }
}
