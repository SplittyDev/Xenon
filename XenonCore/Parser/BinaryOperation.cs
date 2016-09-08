using System;
namespace XenonCore {
    
    /// <summary>
    /// Binary operation.
    /// </summary>
    public enum BinaryOperation {
        None,
        Assign,
        LogicalOr,
        LogicalAnd,
        BitwiseOr,
        BitwiseXor,
        BitwiseAnd,
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        ShiftLeft,
        ShiftRight,
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulo,
        InclusiveRange,
        ExclusiveRange,
        NullCoalescing,
    }
}

