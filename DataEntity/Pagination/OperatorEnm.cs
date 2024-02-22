namespace DataEntity.Pagination
{
    public enum OperatorEnm
    {
        None,

        Equal,
        NotEqual,
        Not,

        Between,
        NotBetween,

        StartWith,
        EndWith,

        NotStartWith,
        NotEndWith,

        InSet,
        NotInSet,

        GreaterThan,
        GreaterThanOrEqual,

        NotGreaterThan,
        NotGreaterThanOrEqual,

        LessThan,
        LessThanOrEqual,

        NotLessThan,
        NotLessThanOrEqual,

        Contain,
        NotContain
    }
}
